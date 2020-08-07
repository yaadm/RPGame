//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
//using OpenMMO;

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
        public GameObject targetPlayer;

        private bool switchTarget = false;

        [Header("Player Control Config")]
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

            if(Input.GetKeyDown(movementConfig.targetKey)) {
                onTargetButtonClicked();
            }

            base.UpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK

        }

        public virtual GameObject onTargetButtonClicked() {
            // TODO: Aquire Target
            return null;
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