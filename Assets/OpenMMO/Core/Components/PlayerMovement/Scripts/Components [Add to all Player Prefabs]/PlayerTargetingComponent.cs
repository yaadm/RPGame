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
    public partial class PlayerTargetingComponent : EntityTargetingComponent
    {
        [Header("Target Settings")]
        [HideInInspector]
        [SyncVar]
        public Transform currentTarget;

        private List<Transform> nearbyPlayers = new List<Transform>();

        private bool switchTarget = false;

        [Header("Player Control Config")]
        public PlayerControlConfig movementConfig;

        private Vector3 mouseDownPos;

        public float minClickDistance;

#if UNITY_EDITOR
        // LOAD DEFAULTS
        private void OnValidate()
        {
            if (!movementConfig) movementConfig = Resources.Load<PlayerControlConfig>("Player/Movement/DefaultPlayerControls"); //LOAD DEFAULT
        }
#endif


        // -------------------------------------------------------------------------------
        // Start
        // -------------------------------------------------------------------------------
        protected override void Start()
        {
            base.Start();
        }

        // -------------------------------------------------------------------------------
        // 
        // -------------------------------------------------------------------------------
        public override void OnStartLocalPlayer()
        {

        }

        // -------------------------------------------------------------------------------
        // 
        // -------------------------------------------------------------------------------
        void OnDestroy()
        {
        }

        // -------------------------------------------------------------------------------
        // UpdateServer
        // @Server
        // -------------------------------------------------------------------------------
        [Server]
        protected override void UpdateServer()
        {
            base.UpdateServer();
            this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK
        }

        // -------------------------------------------------------------------------------
        // UpdateClient
        // @Client
        // -------------------------------------------------------------------------------
        [Client]
        protected override void UpdateClient()
        {


            if (!isLocalPlayer) return;
            if (Tools.AnyInputFocused) return;

            if (Input.GetKeyDown(movementConfig.targetKey))
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

                    // if not clicked on UI
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        RaycastHit hit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.tag == "Player")
                            {

                                currentTarget = hit.transform;
                            }
                            else
                            {
                                currentTarget = null;
                            }
                        }
                        else
                        {
                            currentTarget = null;
                        }
                    }

                }
                else
                {
                    // it was a drag
                }
            }



            base.UpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK

        }

        public virtual void setTarget(Transform target)
        {
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

                currentTarget = null;
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
                    currentTarget = nearbyPlayers[0];
                }
                else
                {

                    // if next index is out of bounds, start again from index 0;
                    index++;
                    if (index > nearbyPlayers.Count - 1)
                    {
                        index = 0;
                    }

                    currentTarget = nearbyPlayers[index];
                }
            }
            else
            {
                currentTarget = nearbyPlayers[0];
            }
        }

        // -------------------------------------------------------------------------------
        // LateUpdateClient
        // @Client
        // -------------------------------------------------------------------------------
        protected override void LateUpdateClient()
        {
            base.LateUpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient)); //HOOK
        }

        // -------------------------------------------------------------------------------


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