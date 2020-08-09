//using System;
//using System.Text;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
//using Mirror;
//using OpenMMO;

namespace OpenMMO
{

    // ===================================================================================
    // MovementStateInfo
    // ===================================================================================
    /// <summary>Holds data related to the current movement input state of a controlled character.</summary>
    public partial struct MovementStateInfo
    {
        //ORIENTATION
        public Vector3 position;
        public Quaternion rotation;

        public float cameraYRotation;

        //MOVE
        public float verticalMovementInput;
        public float horizontalMovementInput;
        public bool movementRunning;

        public bool movementJump;

        // -------------------------------------------------------------------------------
        // MovementStruct (Constructor)
        // -------------------------------------------------------------------------------
        public MovementStateInfo(Vector3 _position, Quaternion _rotation, float _cameraYRotation, float _verticalMovementInput, float _horizontalMovementInput, bool _movementRunning, bool _movementJump)
        {
            //ORIENTATION
            position = _position;
            rotation = _rotation;
            //CAMERA ORIENTATION
            cameraYRotation = _cameraYRotation;
            //MOVE
            verticalMovementInput = _verticalMovementInput;
            horizontalMovementInput = _horizontalMovementInput;
            movementRunning = _movementRunning;
            //JUMP
            movementJump = _movementJump;
        }

        // -------------------------------------------------------------------------------
    }
}
// =======================================================================================