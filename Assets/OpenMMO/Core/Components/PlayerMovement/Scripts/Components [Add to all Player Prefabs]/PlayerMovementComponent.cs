//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using Mirror;
//using OpenMMO;

namespace OpenMMO
{
    // ===================================================================================
    // PlayerMovement
    // ===================================================================================
    [DisallowMultipleComponent]
    [System.Serializable]
    public partial class PlayerMovementComponent : EntityMovementComponent
    {
        [Header("Player Movement Config")]
        public PlayerControlConfig movementConfig;

        //MOVE
        protected float verticalMovementInput;
        protected float horizontalMovementInput;

        //TURN
        protected float cameraYRotation;

        //RUN
        protected bool running = true;
        protected bool jump;

        protected bool isGrounded;

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