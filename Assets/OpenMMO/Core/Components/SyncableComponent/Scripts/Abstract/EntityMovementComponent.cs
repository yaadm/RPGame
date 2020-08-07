
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using OpenMMO;

namespace OpenMMO
{

    // ===================================================================================
    // EntityMovementComponent
    // ===================================================================================
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EntityComponent))]
    [DisallowMultipleComponent]
    [System.Serializable]
    public partial class EntityMovementComponent : SyncableComponent
    {

        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;

        public Rigidbody playerRigidbody;

        // -- Component Cache
        protected EntityComponent entityComponent;
        //public EntityEnergyComponent energyComponent; // TODO: add later to check "death" in states!

        // -------------------------------------------------------------------------------

        [SyncVar] protected int animationState;

        protected bool isPlayerGrounded;

        // -------------------------------------------------------------------------------
        // Start
        // @Server / @Client
        // -------------------------------------------------------------------------------
        protected override void Start()
        {

            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            playerRigidbody.GetComponent<Rigidbody>();
            entityComponent = GetComponent<EntityComponent>();

            base.Start();
        }

        // -------------------------------------------------------------------------------
        // UpdateServer
        // @Server
        // -------------------------------------------------------------------------------
        [Server]
        protected override void UpdateServer()
        {

            foreach (KeyValuePair<int, StateTemplate> stateTemplate in StateTemplate.data)
            {
                if (stateTemplate.Value.GetIsActive(entityComponent))
                {
                    animationState = stateTemplate.Key;
                    return;
                }
            }

            this.InvokeInstanceDevExtMethods(nameof(UpdateServer)); //HOOK

        }

        // -------------------------------------------------------------------------------
        // UpdateClient
        // @Client
        // -------------------------------------------------------------------------------
        [Client]
        protected override void UpdateClient()
        {
            this.InvokeInstanceDevExtMethods(nameof(UpdateClient)); //HOOK
        }

        // -------------------------------------------------------------------------------
        // LateUpdateClient
        // @Client
        // -------------------------------------------------------------------------------
        [Client]
        protected override void LateUpdateClient()
        {

            if (animationState == 0)
                return;

            foreach (KeyValuePair<int, StateTemplate> stateTemplate in StateTemplate.data)  // each state
            {

                string stateName = stateTemplate.Value.name;

                foreach (Animator anim in GetComponentsInChildren<Animator>())              // each animator
                {
                    if (anim.runtimeAnimatorController != null)                             // not null?
                    {
                        if (anim.parameters.Any(x => x.name == stateName))                  // has param?
                        {

                            if (stateTemplate.Value.hash == animationState)                         // is current state?
                            {
                                anim.SetBool(stateName, true);
                            }
                            else
                            {
                                anim.SetBool(stateName, false);                             // otherwise deactivate
                            }
                        }
                    }
                }

            }

            this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient)); //HOOK

        }

        // -------------------------------------------------------------------------------
        // FixedClient
        // @Client
        // -------------------------------------------------------------------------------
        [Client]
        protected override void FixedUpdateClient()
        {

        }

        // -------------------------------------------------------------------------------

    }

}

// =======================================================================================