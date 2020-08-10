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
    // Skills System
    // ===================================================================================
    public partial class PlayerControllerComponent
    {

        bool clickedSkill;

        private void UpdateClient_skills()
        {

            if (isDead())
            {
                return;
            }

            if (Input.GetKeyDown(controllerConfig.skillbarSlot_0))
            {
                if (currentTarget)
                {
                    // Debug.Log("Skill clicked !");
                    clickedSkill = true;
                }
            }

        }

        // updates server
        private void FixedUpdateClient_skills()
        {
            if (isLocalPlayer && clickedSkill)
            {
                if (currentTarget.GetComponent<PlayerControllerComponent>().isDead())
                {
                    onTargetButtonClicked();
                }
                clickedSkill = false;
                onSkillClicked();
            }
        }

        public void onSkillClicked()
        {
            Cmd_UpdateSkill();
        }

        // -------------------------------------------------------------------------------
        // Cmd_UpdateState
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        /// <summary>Sends movement state to the server, where the server updates velocity, then returns updated position info to clients.</summary>
        /// <param name="moveState"></param>
        [Command]
        protected virtual void Cmd_UpdateSkill()
        {

            // validate can hit
            // return casting percent
            // make enemy health lower by skill power
            bool success = false;

            if (currentTarget)
            {

                PlayerControllerComponent pc = currentTarget.GetComponent<PlayerControllerComponent>();

                pc.takeDamage(10);

                success = true;
            }

            RpcCorrectClientSkillHit(success);
        }

        // -------------------------------------------------------------------------------
        // RpcCorrectClientPosition
        // Updates the rotation, position and velocity on clients based on server stats
        // @Server -> @Clients
        // -------------------------------------------------------------------------------
        /// <summary>Corrects the Client's position based upon the Server's interpretation of the simulation.</summary>
        [ClientRpc]
        public void RpcCorrectClientSkillHit(bool clickedSkill)
        {

            if (isLocalPlayer) return; //IGNORE LOCAL CLIENTS //TODO: Are we positive that local player does not need correction?
        }
    }


}