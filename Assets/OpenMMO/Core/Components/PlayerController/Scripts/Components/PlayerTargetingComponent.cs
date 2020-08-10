//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
//using OpenMMO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace OpenMMO
{

    // ===================================================================================
    // Targeting System
    // ===================================================================================
    public partial class PlayerControllerComponent
    {
        [Header("Target Settings")]
        [HideInInspector]
        [SyncVar]
        public Transform currentTarget;

        private List<PlayerControllerComponent> nearbyEnemies = new List<PlayerControllerComponent>();

        private bool switchTarget = false;

        private Vector3 mouseDownPos;

        public float minClickDistance;


        public virtual void setTarget(Transform target)
        {
            //TODO: add change target sound
            currentTarget = target;
        }

        public virtual Transform getTarget()
        {
            // TODO: check if target is dead ? (or it wil be handled in another place ?)
            return currentTarget;
        }

        private void UpdateClient_targeting()
        {
            if (Input.GetKeyDown(controllerConfig.targetKey))
            {
                onTargetButtonClicked();
            }

            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPos = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {

                if (Vector3.Distance(Input.mousePosition, mouseDownPos) < minClickDistance)
                {
                    // it was a click !

                    // if clicked on UI, return
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }

                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    // gets only the first collider it founds ! not all in path.
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            currentTarget = hit.transform;
                        }
                        else
                        {
                            // Clicked on Game, but no "Player" objects found.
                            currentTarget = null;
                        }
                    }
                    else
                    {
                        // Clicked on Game, but no objects found.
                        currentTarget = null;
                    }
                }
                else
                {
                    // it was a drag
                }
            }


        }

        private void FixedUpdateClient_targeting()
        {
            if (isLocalPlayer)
            {
                Cmd_UpdateTarget(currentTarget);
            }
        }

        // -------------------------------------------------------------------------------
        // Cmd_UpdateState
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        /// <summary>Sends movement state to the server, where the server updates velocity, then returns updated position info to clients.</summary>
        /// <param name="moveState"></param>
        [Command]
        protected virtual void Cmd_UpdateTarget(Transform _currentTarget)
        {

            // TODO: validate target in range
            // TODO: validate target alive

            currentTarget = _currentTarget;

            RpcCorrectClientTarget(_currentTarget);
        }

        // -------------------------------------------------------------------------------
        // RpcCorrectClientPosition
        // Updates the rotation, position and velocity on clients based on server stats
        // @Server -> @Clients
        // -------------------------------------------------------------------------------
        /// <summary>Corrects the Client's position based upon the Server's interpretation of the simulation.</summary>
        [ClientRpc]
        public void RpcCorrectClientTarget(Transform _currentTarget)
        {
            if (isLocalPlayer) return; //IGNORE LOCAL CLIENTS //TODO: Are we positive that local player does not need correction?
        }

        public virtual void onTargetButtonClicked()
        {
            clearDeadFromNearbyenemiesList();
            // If no nearby players.
            if (nearbyEnemies.Count <= 0)
            {
                setTarget(null);
                return;
            }

            if (currentTarget)
            {
                PlayerControllerComponent targetPcc = currentTarget.GetComponent<PlayerControllerComponent>();
                int index = nearbyEnemies.IndexOf(targetPcc);

                // current target doest not exists in nearbyPlayers
                if (index > nearbyEnemies.Count - 1 || index < 0)
                {
                    // current target is not available \ out of range

                    // get closest enemy
                    setTarget(nearbyEnemies[0].GetComponent<Transform>());
                }
                else
                {

                    // target next enemy
                    index++;

                    // if next index is out of bounds, start again from index 0;
                    if (index > nearbyEnemies.Count - 1)
                    {
                        index = 0;
                    }

                    // if after target rotation we reached the same target, dont set new target.
                    // could happen if there is only one enemy in range, and we try to get new target. (we dont want the target sound to play again)
                    if (nearbyEnemies[index] != currentTarget)
                    {

                        setTarget(nearbyEnemies[index].GetComponent<Transform>());
                    }
                }
            }
            else
            {
                // dont have a target yet, target first available.
                setTarget(nearbyEnemies[0].GetComponent<Transform>());
            }
        }

        void clearDeadFromNearbyenemiesList()
        {
            List<PlayerControllerComponent> nearbyEnemiesToRemove = new List<PlayerControllerComponent>();
            for (int i = 0; i < nearbyEnemies.Count; i++)
            {
                PlayerControllerComponent pcc = nearbyEnemies[i];
                if (pcc.isDead())
                {
                    nearbyEnemiesToRemove.Add(pcc);
                }
            }
            nearbyEnemies.RemoveAll(item => nearbyEnemiesToRemove.Contains(item));
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                //TODO: check if enemy
                PlayerControllerComponent otherPcc = other.transform.GetComponent<PlayerControllerComponent>();
                if (otherPcc.isDead())
                {
                    return;
                }
                if (!nearbyEnemies.Contains(otherPcc))
                {
                    // add enemy to nearby list.
                    nearbyEnemies.Add(otherPcc);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                //TODO: check if enemy ? i dont think we need to check that... maybe...

                if (nearbyEnemies.Contains(other.transform.GetComponent<PlayerControllerComponent>()))
                {
                    // remove enemy from nearby list.
                    nearbyEnemies.Remove(other.transform.GetComponent<PlayerControllerComponent>());
                }
            }
        }

        // void Sort()
        // {
        //     //TODO: should only occure once you enter combat !
        //     nearbyPlayers.Sort(delegate (Transform a, Transform b)
        //     {
        //         return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position));
        //     });
        // }

        public Transform haveTarget()
        {
            return currentTarget;
        }

    }


}