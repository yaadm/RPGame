using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using Mirror;
//using OpenMMO;
using UnityEngine.EventSystems;
using OpenMMO.UI;

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
        public PlayerControlConfig controllerConfig;

        private string targetAnchor = "ZoneAnchor"; // this is the default anchor name for all realms.

        private bool waitingForReviveResponse = false;

#if UNITY_EDITOR
        // LOAD DEFAULTS
        private void OnValidate()
        {
            if (!controllerConfig) controllerConfig = Resources.Load<PlayerControlConfig>("Player/Movement/DefaultPlayerControls"); //LOAD DEFAULT
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

            if (isDead())
            {
                if (!UIPopupConfirm.singleton.IsVisible() && !waitingForReviveResponse)
                {
                    waitingForReviveResponse = true;
                    UIPopupConfirm.singleton.Init(String.Format("Revive at graveyard ?"), OnReviveRequested);
                }
            }

            // ORDER IS IMPORTANT !
            UpdateClient_movement();
            UpdateClient_targeting();
            UpdateClient_skills();

            base.UpdateClient();
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
        }

        private void OnReviveRequested()
        {
            Cmd_RequestRevive();
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

        // -------------------------------------------------------------------------------
        // FixedUpdateClient
        // @Client
        // -------------------------------------------------------------------------------
        [Client]
        protected override void FixedUpdateClient()
        {
            if (!localPlayer) return;

            // ORDER IS IMPORTANT !
            FixedUpdateClient_movement();
            FixedUpdateClient_targeting();
            FixedUpdateClient_skills();

        }

        [Command]
        protected virtual void Cmd_RequestRevive()
        {

            bool success = false;

            if (isDead())
            {
                // order is important !
                // *************************************
                reviveCurrentStats();

                PlayerComponent pc = GetComponentInParent<PlayerComponent>();
                pc.WarpLocal(targetAnchor);
                // *************************************

                success = true;
            }


            RpcReviveResponse(success);
        }

        [ClientRpc]
        public void RpcReviveResponse(bool success)
        {
            if (!localPlayer) return;

            // update health so popup will not bother us.. its takes time for the server to update our health.
            currentHealth = totalHealth;
            waitingForReviveResponse = false;
        }

    }

}

// =======================================================================================