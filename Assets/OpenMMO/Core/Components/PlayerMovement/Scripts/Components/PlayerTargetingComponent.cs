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

        private List<Transform> nearbyPlayers = new List<Transform>();

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

        public virtual void onTargetButtonClicked()
        {
            // If no nearby players.
            if (nearbyPlayers.Count <= 0)
            {
                setTarget(null);
                return;
            }

            if (currentTarget)
            {
                int index = nearbyPlayers.IndexOf(currentTarget);

                // current target doest not exists in nearbyPlayers
                if (index > nearbyPlayers.Count - 1 || index < 0)
                {
                    // current target is not available \ out of range

                    // get closest enemy
                    setTarget(nearbyPlayers[0]);
                }
                else
                {

                    // target next enemy
                    index++;

                    // if next index is out of bounds, start again from index 0;
                    if (index > nearbyPlayers.Count - 1)
                    {
                        index = 0;
                    }

                    // if after target rotation we reached the same target, dont set new target.
                    // could happen if there is only one enemy in range, and we try to get new target. (we dont want the target sound to play again)
                    if (nearbyPlayers[index] != currentTarget)
                    {

                        setTarget(nearbyPlayers[index]);
                    }
                }
            }
            else
            {
                // dont have a target yet, target first available.
                setTarget(nearbyPlayers[0]);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                //TODO: check if enemy

                if (!nearbyPlayers.Contains(other.transform))
                {
                    // add enemy to nearby list.
                    nearbyPlayers.Add(other.transform);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                //TODO: check if enemy ? i dont think we need to check that... maybe...

                if (nearbyPlayers.Contains(other.transform))
                {
                    // remove enemy from nearby list.
                    nearbyPlayers.Remove(other.transform);
                }
            }
        }

        void Sort()
        {
            //TODO: should only occure once you enter combat !
            nearbyPlayers.Sort(delegate (Transform a, Transform b)
            {
                return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position));
            });
        }

        public Transform haveTarget()
        {
            return currentTarget;
        }

    }


}