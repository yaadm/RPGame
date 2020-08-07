using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

    // ===================================================================================
    // UIShortcut
    // ===================================================================================
    [RequireComponent(typeof(Button))]
    public class UITargetEnemy : MonoBehaviour
    {
        [Header("Target Settings")]
        public Button targetButton;

        public GameObject targetPannel;

        public GameObject targetName;
        public GameObject healthBar;

        public GameObject manaBar;

        protected PlayerTargetingComponent targetingComponent;

		
#if UNITY_EDITOR
// LOAD DEFAULTS
        private void OnValidate()
        {
            if (!targetButton) {
                Debug.LogError("No Target Button !");
            }

            if (!targetPannel) {
                Debug.LogError("No Target Pannel !");
            }

            if (!targetName) {
                Debug.LogError("No targetName Pannel !");
            }

            if (!healthBar) {
                Debug.LogError("No healthBar Pannel !");
            }

            if (!manaBar) {
                Debug.LogError("No manaBar Pannel !");
            }
        }
#endif

		// -------------------------------------------------------------------------------
        // OnEnable
        // -------------------------------------------------------------------------------
        void OnEnable() 
        {
            if(!targetPannel) {
                Debug.LogError("Targeting Component: no UI Target Pannel !");
                return;
            }

            targetPannel.SetActive(false);
            targetButton.onClick.SetListener(() =>
            {
                if (targetPannel) {
                    
                    targetingComponent.onTargetButtonClicked();
                    
                } else {

                    Debug.LogError("OnClick() - faild no goPanel");
                }
            });

        }

        void LateUpdate() {
            
            if (!targetingComponent) {

                if (!PlayerComponent.localPlayer) {
                    return;
                }
                
                targetingComponent = PlayerComponent.localPlayer.GetComponent<PlayerTargetingComponent>();

                if (!targetingComponent) {
                    return;
                }
            }

            if (targetingComponent.targetPlayer) {

                // TODO: update UI components to match the targeted player

                targetPannel.SetActive(true);
            } else {
                targetPannel.SetActive(false);
            }
        }

    }
}
