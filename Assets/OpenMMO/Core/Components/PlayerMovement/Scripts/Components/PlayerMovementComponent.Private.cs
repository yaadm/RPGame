
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
    public partial class PlayerMovementComponent
    {

        [Header("Performance")]
        [Tooltip("How often are movement updates sent to the server (in seconds)?")]
        [Range(0.01f, 99)]
        public double movementUpdateInterval = 1f;

        double _timerMovement = 0;

        // -------------------------------------------------------------------------------
        // UpdateVelocity
        // This recalculates the agent velocity based on the current input axis'
        // @Client / @Server
        // -------------------------------------------------------------------------------
        protected virtual void UpdateVelocity()
        {

            if (verticalMovementInput != 0 || horizontalMovementInput != 0)
            {

                // make input vector without Y axis
                Vector3 input = new Vector3(horizontalMovementInput, 0, verticalMovementInput);
                if (input.magnitude > 1) input = input.normalized;

                // calc camera rotation to move relative to
                Vector3 angles = new Vector3(0, IsLocalPlayer ? Camera.main.transform.eulerAngles.y : cameraYRotation, 0);
                Quaternion rotation = Quaternion.Euler(angles);

                // calc movement direction
                Vector3 direction = rotation * input;

                // calc movement speed factor
                float factor = running ? movementConfig.runSpeedScale : movementConfig.walkSpeedScale;
                Vector3 newVelocity = direction * agent.speed * factor * movementConfig.moveSpeedMultiplier;

                // rotate player at X,Z axis to target
                transform.LookAt(transform.position + direction, Vector3.up);

                if (agent && agent.isActiveAndEnabled)
                {
                    agent.Move(newVelocity);
                }
                else
                {

                    // dont affect current Y axis velocity
                    newVelocity.y = playerRigidbody.velocity.y;
                    playerRigidbody.velocity = newVelocity;
                }
            }



            if (jump && isPlayerGrounded)
            {
                jump = false;

                if (agent && agent.isActiveAndEnabled)
                {

                    // NavMeshAgent cant Jump ! - as far as we know :) 
                    // maybe try YOffset ?
                    agent.velocity += Vector3.up * agent.speed * movementConfig.jumpSpeedScale * movementConfig.jumpSpeedMultiplier;
                }
                else
                {

                    playerRigidbody.AddForce(0, agent.speed * movementConfig.jumpSpeedScale * movementConfig.jumpSpeedMultiplier, 0, ForceMode.Impulse);
                }
            }

        }


        // S E R V E R  A U T H O R I T A T I V E  M O V E M E N T

        // -------------------------------------------------------------------------------
        // FixedUpdateClient
        // @Client
        // -------------------------------------------------------------------------------
        [Client]
        protected override void FixedUpdateClient()
        {
            if (isLocalPlayer && ReadyToMove() && Camera.main) // CHECK FOR THROTTLING
            {
                Cmd_UpdateMovementState(new MovementStateInfo(transform.position, transform.rotation, Camera.main.transform.eulerAngles.y, verticalMovementInput, horizontalMovementInput, running, jump));
                LogMovement();
            }

        }

        // -------------------------------------------------------------------------------
        // ReadyToMove
        // -------------------------------------------------------------------------------
        /// <summary>Movement Throttling</summary>
        /// <returns>Enough time has passed...ready to move again.</returns>
        protected bool ReadyToMove() { return Time.time > _timerMovement; }

        // -------------------------------------------------------------------------------
        // LogMovement
        // -------------------------------------------------------------------------------
        /// <summary>Logs the last time that movement was processed.</summary>
        private void LogMovement()
        {
            _timerMovement = Time.time + movementUpdateInterval;
        }


        // -------------------------------------------------------------------------------
        // Cmd_UpdateState
        // @Client -> @Server
        // -------------------------------------------------------------------------------
        /// <summary>Sends movement state to the server, where the server updates velocity, then returns updated position info to clients.</summary>
        /// <param name="moveState"></param>
        [Command]
        protected virtual void Cmd_UpdateMovementState(MovementStateInfo moveState)
        {

            transform.position = moveState.position;
            transform.rotation = moveState.rotation;

            verticalMovementInput = Mathf.Clamp(moveState.verticalMovementInput, -1, 1);        // good enough for keyboard + controller
            horizontalMovementInput = Mathf.Clamp(moveState.horizontalMovementInput, -1, 1);    // good enough for keyboard + controller
            running = moveState.movementRunning;
            jump = moveState.movementJump;

            cameraYRotation = moveState.cameraYRotation;

            UpdateVelocity();

            if (agent && agent.isActiveAndEnabled)
            {

                RpcCorrectClientPosition(transform.position, transform.rotation, agent.velocity);
            }
            else
            {

                RpcCorrectClientPosition(transform.position, transform.rotation, playerRigidbody.velocity);
            }
        }

        // -------------------------------------------------------------------------------
        // RpcCorrectClientPosition
        // Updates the rotation, position and velocity on clients based on server stats
        // @Server -> @Clients
        // -------------------------------------------------------------------------------
        /// <summary>Corrects the Client's position based upon the Server's interpretation of the simulation.</summary>
        [ClientRpc]
        public void RpcCorrectClientPosition(Vector3 _position, Quaternion _rotation, Vector3 _velocity)
        {
            if (isLocalPlayer) return; //IGNORE LOCAL CLIENTS //TODO: Are we positive that local player does not need correction?

            if (agent && agent.isActiveAndEnabled)
            {

                agent.ResetPath();

                // if we will need this feature, try using velocity only and remove position.
                // update position only if client _position.DistanceTo(transform.position) > Delta (allowed 

                // disable the velocity, its updated VIA movePosition. ? could 
                //_velocity.y = 0;
                //agent.velocity = _velocity;

                //transform.position = _position;
                agent.Move(_velocity);
                transform.rotation = _rotation;

                // TODO: check if Server Player Position is unsynced to the client position and fix position ?
                // If we ever need to make the player character AI UserNavMeshAgent

            }
            else
            {

                // disable the velocity, its updated VIA movePosition.
                _velocity.y = 0;
                playerRigidbody.velocity = _velocity;

                if (!_position.Equals(playerRigidbody.position))
                {

                    playerRigidbody.MovePosition(_position);
                }
                if (!_rotation.Equals(playerRigidbody.rotation))
                {

                    playerRigidbody.rotation = _rotation;
                }

            }
        }

        // -------------------------------------------------------------------------------

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Ground")
            {
                isPlayerGrounded = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.tag == "Ground")
            {
                isPlayerGrounded = false;
            }
        }
    }

}

// =======================================================================================