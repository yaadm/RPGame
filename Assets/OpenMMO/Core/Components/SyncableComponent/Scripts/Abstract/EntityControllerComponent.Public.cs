
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
    public partial class EntityControllerComponent
    {

        // -------------------------------------------------------------------------------
        // GetIsMoving
        // @Server / @Client
        // -------------------------------------------------------------------------------
        public bool GetIsMoving
        {
            get
            {
                // do not animate moving while on air..
                if (!isPlayerGrounded)
                {
                    return false;
                }
                if (agent && agent.isActiveAndEnabled)
                {
                    return agent.velocity != Vector3.zero;
                }
                else
                {
                    // return playerRigidbody.velocity != Vector3.zero;
                    Vector3 simulated = playerRigidbody.velocity;
                    simulated.y = 0;
                    return simulated != Vector3.zero;
                }
            }
        }

        // -------------------------------------------------------------------------------

    }

}

// =======================================================================================