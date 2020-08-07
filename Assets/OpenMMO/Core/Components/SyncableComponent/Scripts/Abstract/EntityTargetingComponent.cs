
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


    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EntityComponent))]
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class EntityTargetingComponent : SyncableComponent
	{
        // -------------------------------------------------------------------------------
		// Start
		// @Server / @Client
		// -------------------------------------------------------------------------------
		protected override void Start()
    	{	
        	base.Start();
		}
		
		// -------------------------------------------------------------------------------
		// UpdateServer
		// @Server
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			
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