using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

    // ===================================================================================
    // UITargetEnemy
    // ===================================================================================
    public partial class UICharacterManager
    {
        [Header("Target Settings")]
        public Button targetButton;

        public GameObject targetPannel;

        public Text targetName;

        public GameObject healthBar;

        public GameObject manaBar;

        protected PlayerControllerComponent targetingComponent;

        public void onTargetLateUpdate()
        {
            // should happen at OnStart ?
            if (!targetingComponent)
            {

                if (!PlayerComponent.localPlayer)
                {
                    Debug.LogWarning("no local player yet...");
                    return;
                }

                targetingComponent = PlayerComponent.localPlayer.GetComponent<PlayerControllerComponent>();

                if (!targetingComponent)
                {
                    Debug.LogError("no targetingComponent !");
                    return;
                }
            }

            if (targetingComponent.currentTarget)
            {

                // TODO: update UI components to match the targeted player

                PlayerComponent pc = targetingComponent.currentTarget.GetComponent<PlayerComponent>();

                if (pc)
                {

                    targetName.text = pc.name;

                    targetPannel.SetActive(true);
                }
            }
            else
            {
                targetPannel.SetActive(false);
            }
        }
    }
}
