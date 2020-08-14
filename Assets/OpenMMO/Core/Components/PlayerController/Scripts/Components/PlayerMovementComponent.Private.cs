
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
    public partial class PlayerControllerComponent
    {

        [Header("Performance")]
        [Tooltip("How often are movement updates sent to the server (in seconds)?")]
        [Range(0.01f, 99)]
        public double movementUpdateInterval = 0.1f;

        double _timerMovement = 0;

        //MOVE
        protected float verticalMovementInput;
        protected float horizontalMovementInput;

        //TURN
        protected float cameraYRotation;

        //RUN
        protected bool running;
        protected bool jump;
        protected bool isJumpingUp;
        protected bool isFalling;

        private float lerp = 0f;


        private void UpdateClient_movement()
        {

            //MOVE
            horizontalMovementInput = Input.GetAxis(controllerConfig.moveAxisHorizontal.ToString());
            verticalMovementInput = Input.GetAxis(controllerConfig.moveAxisVertical.ToString());

            //RUN - Toggle
            if (Input.GetKeyDown(controllerConfig.runKey))
            {
                running = !running;
            }

            if (Input.GetKeyDown(controllerConfig.jumpKey))
            {
                jump = true;
                isJumpingUp = true;
            }

            UpdateVelocity(); //UPDATE VELOCITY
        }

        // -------------------------------------------------------------------------------
        // UpdateVelocity
        // This recalculates the agent velocity based on the current input axis'
        // @Client / @Server
        // -------------------------------------------------------------------------------
        protected virtual void UpdateVelocity()
        {

            if (isDead())
            {

                if (agent && agent.isActiveAndEnabled)
                {
                    agent.velocity = Vector3.zero;
                }
                else
                {
                    // stop moving (X,Z) axis on death.
                    Vector3 newVelocity = new Vector3(0, playerRigidbody.velocity.y, 0);
                    playerRigidbody.velocity = newVelocity;
                }


                return;
            }

            if (verticalMovementInput != 0 || horizontalMovementInput != 0)
            {

                // make input vector without Y axis
                Vector3 input = new Vector3(horizontalMovementInput, 0, verticalMovementInput);
                if (input.magnitude > 1) input = input.normalized;

                // calc camera rotation to move relative to
                Vector3 angles = new Vector3(0, IsLocalPlayer ? Camera.main.transform.eulerAngles.y : cameraYRotation, 0);

                // actually we dont have to use Quaternions, we rotate only on y axis, so no axis locks possible.. but wth...
                Quaternion rotation = Quaternion.Euler(angles);

                // calc movement direction
                Vector3 direction = rotation * input;

                // calc movement speed factor

                float factor = 0;
                if (isPlayerGrounded || (agent && agent.isActiveAndEnabled)) // Dont slow down to air speed on jump when using agent.
                {
                    factor = running ? controllerConfig.runSpeedScale : controllerConfig.walkSpeedScale;
                }
                else
                {
                    factor = controllerConfig.moveInAirSpeedScale;
                }

                Vector3 newVelocity = direction * agent.speed * factor * controllerConfig.moveSpeedMultiplier;

                // rotate player at X,Z axis to target
                transform.LookAt(transform.position + direction, Vector3.up);

                if (agent && agent.isActiveAndEnabled)
                {
                    agent.velocity = newVelocity;
                }
                else
                {
                    // dont affect current Y axis velocity
                    newVelocity.y = playerRigidbody.velocity.y;
                    playerRigidbody.velocity = newVelocity;
                }
            }
            else
            {


                if (agent && agent.isActiveAndEnabled)
                {
                    // jump in agent is not velocity its baseOffset so.. zero all.
                    agent.velocity = Vector3.zero;
                }
                else
                {
                    // stop movement !
                    // dont affect current Y axis velocity
                    Vector3 newVelocity = new Vector3(0, playerRigidbody.velocity.y, 0);
                    playerRigidbody.velocity = newVelocity;
                }
            }



            if (jump)
            {
                if (agent && agent.isActiveAndEnabled)
                {

                    float speed = agent.speed * controllerConfig.jumpSpeedScale * controllerConfig.jumpSpeedMultiplier;

                    if (isFalling)
                    {
                        // Falling Down
                        lerp += Time.deltaTime / (controllerConfig.jumpAnimationDuration / 2f); // jumping up is half of the animation time
                        agent.baseOffset = Mathf.SmoothStep(controllerConfig.jumpHeight, 0f, lerp);
                        if (lerp >= .99f)
                        {
                            lerp = 0;
                            isJumpingUp = false;
                            isFalling = false;
                        }
                    }
                    else if (isJumpingUp)
                    {
                        // Jumping Up
                        lerp += Time.deltaTime / (controllerConfig.jumpAnimationDuration / 2f); // jumping up is half of the animation time
                        agent.baseOffset = Mathf.SmoothStep(0f, controllerConfig.jumpHeight, lerp);
                        // finished animation
                        if (lerp >= .99f)
                        {
                            lerp = 0;
                            isJumpingUp = false;
                            isFalling = true;
                        }
                        if (isPlayerGrounded)
                        {
                            isPlayerGrounded = false;
                        }

                    }
                    else if (!isJumpingUp && !isFalling)
                    {
                        // stopped jumping
                        jump = false;
                        isPlayerGrounded = true;
                        agent.baseOffset = 0;
                    }
                }
                else
                {
                    jump = false;
                    if (isPlayerGrounded)
                    {
                        playerRigidbody.AddForce(0, agent.speed * controllerConfig.jumpSpeedScale * controllerConfig.jumpSpeedMultiplier, 0, ForceMode.Impulse);
                    }
                }
            }
            else
            {
                if (!isPlayerGrounded && (agent && agent.isActiveAndEnabled))
                {
                    // mostly used for initial value.. could just change the default value there ?
                    agent.baseOffset = 0;
                    isPlayerGrounded = true;
                }
            }
        }


        // S E R V E R  A U T H O R I T A T I V E  M O V E M E N T

        private void FixedUpdateClient_movement()
        {
            if (isLocalPlayer && ReadyToMove() && Camera.main) // CHECK FOR THROTTLING
            {
                Cmd_UpdateMovementState(new MovementStateInfo(transform.position, transform.rotation, Camera.main.transform.eulerAngles.y, verticalMovementInput, horizontalMovementInput, running, jump));
                LogMovement();
            }
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

            if (moveState.movementJump)
            {
                jump = true;
                if (isPlayerGrounded)
                {
                    // activate baseoffset animation
                    isJumpingUp = true;
                }
            }

            cameraYRotation = moveState.cameraYRotation;

            UpdateVelocity();

            if (agent && agent.isActiveAndEnabled)
            {

                RpcCorrectClientPosition(transform.position, transform.rotation, agent.velocity, agent.baseOffset);
            }
            else
            {
                // jump position is irrelevant here, we are usgin forces as Rigidbody
                RpcCorrectClientPosition(transform.position, transform.rotation, playerRigidbody.velocity, 0);
            }
        }

        // -------------------------------------------------------------------------------
        // RpcCorrectClientPosition
        // Updates the rotation, position and velocity on clients based on server stats
        // @Server -> @Clients
        // -------------------------------------------------------------------------------
        /// <summary>Corrects the Client's position based upon the Server's interpretation of the simulation.</summary>
        [ClientRpc]
        public void RpcCorrectClientPosition(Vector3 _position, Quaternion _rotation, Vector3 _velocity, float baseOffset)
        {
            if (isLocalPlayer) return; //IGNORE LOCAL CLIENTS //TODO: Are we positive that local player does not need correction?

            if (agent && agent.isActiveAndEnabled)
            {

                agent.ResetPath();
                // Debug.Log("received baseOffset: " + jumpPosition);

                // if we will need this feature, try using velocity only and remove position.
                // update position only if client _position.DistanceTo(transform.position) > Delta (allowed 

                // disable the velocity, its updated VIA movePosition. ? could 
                //_velocity.y = 0;
                //agent.velocity = _velocity;

                //transform.position = _position;
                agent.velocity = _velocity;
                agent.baseOffset = baseOffset;
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
    }

}

// =======================================================================================