
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using OpenMMO;

namespace OpenMMO {
	
	// ===================================================================================
	// EntityMovementComponent
	// ===================================================================================
	public partial class EntityMovementComponent
	{
	
		// -------------------------------------------------------------------------------
		// GetIsMoving
		// @Server / @Client
		// -------------------------------------------------------------------------------
		public bool GetIsMoving
    	{
    		get
    		{
				return (playerRigidbody.velocity != Vector3.zero || (agent && agent.velocity != Vector3.zero));
    		}
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================