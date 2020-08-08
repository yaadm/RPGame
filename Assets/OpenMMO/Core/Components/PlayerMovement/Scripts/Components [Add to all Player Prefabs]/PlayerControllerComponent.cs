//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using Mirror;
//using OpenMMO;
using UnityEngine.EventSystems;

namespace OpenMMO
{
    // ===================================================================================
    // PlayerMovement
    // ===================================================================================
    [DisallowMultipleComponent]
    [System.Serializable]
    public partial class PlayerControllerComponent : EntityControllerComponent
    {
        [Header("Player Movement Config")]
        public PlayerControlConfig movementConfig;



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
            agent.updateRotation = false;
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

            // ***********************************
            // Movement SYSTEM
            // ***********************************

            //MOVE
            horizontalMovementInput = Input.GetAxis(movementConfig.moveAxisHorizontal.ToString());
            verticalMovementInput = Input.GetAxis(movementConfig.moveAxisVertical.ToString());

            //RUN - Toggle
            if (Input.GetKeyDown(movementConfig.runKey))
            {
                running = !running;
            }
            //running = Input.GetKeyDown(movementConfig.runKey);

            jump = Input.GetKeyDown(movementConfig.jumpKey);

            UpdateVelocity(); //UPDATE VELOCITY

            // ***********************************
            // Targeting SYSTEM
            // ***********************************

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

            base.UpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
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

    }

}

// =======================================================================================