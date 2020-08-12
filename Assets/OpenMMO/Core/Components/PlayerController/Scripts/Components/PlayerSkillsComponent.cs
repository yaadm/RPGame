using System;
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

        SkillTemplate clickedSkill;
        bool isButtonClicked;

        private void UpdateClient_skills()
        {

            if (isDead())
            {
                return;
            }

            // TODO: move this logic to SkillBar Script

            // if (Input.GetKeyDown(controllerConfig.skillbarSlot_0))
            // {
            //     if (currentTarget)
            //     {
            //         // TODO: resolve Actual skill vs button clicked (shortcut screen)
            //         isButtonClicked = true;
            //     }
            // }

        }

        // updates server
        private void FixedUpdateClient_skills()
        {
            if (isLocalPlayer && isButtonClicked)
            {
                if (currentTarget.GetComponent<PlayerControllerComponent>().isDead())
                {
                    onTargetButtonClicked();
                }
                // onSkillClicked();
                isButtonClicked = false;
            }
        }

        public void onSkillClicked(SkillTemplate skill)
        {
            if (skill)
            {
                Cmd_UpdateSkill(skill.hash);
            }
            else
            {
                Debug.Log("no skill to activate !");
            }
        }

        // -------------------------------------------------------------------------------
        // Cmd_UpdateState
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        /// <summary>Sends movement state to the server, where the server updates velocity, then returns updated position info to clients.</summary>
        /// <param name="moveState"></param>
        [Command]
        protected virtual void Cmd_UpdateSkill(int skill_hash)
        {

            // validate can hit
            // return casting percent
            // make enemy health lower by skill power
            string result = "success";

            if (currentTarget)
            {

                PlayerControllerComponent pc = currentTarget.GetComponent<PlayerControllerComponent>();

                SkillTemplate skill = characterClass.skillsList.Find(item => item.hash == skill_hash);

                if (skill)
                {
                    pc.takeDamage(skill.baseImpact);
                }
                else
                {
                    result = "Could not find skill";
                }


            }

            RpcCorrectClientSkillHit(result);
        }

        // -------------------------------------------------------------------------------
        // RpcCorrectClientPosition
        // Updates the rotation, position and velocity on clients based on server stats
        // @Server -> @Clients
        // -------------------------------------------------------------------------------
        /// <summary>Corrects the Client's position based upon the Server's interpretation of the simulation.</summary>
        [ClientRpc]
        public void RpcCorrectClientSkillHit(string result)
        {

            if (isLocalPlayer) return; //IGNORE LOCAL CLIENTS //TODO: Are we positive that local player does not need correction?
        }
    }


}