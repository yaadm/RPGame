using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

    // ===================================================================================
    // UITargetEnemy
    // ===================================================================================
    public class UITargetEnemy : MonoBehaviour
    {
        [Header("Target Settings")]
        public Button targetButton;

        public GameObject targetPannel;

        public Text targetName;

        public GameObject healthBar;

        public GameObject manaBar;

        protected PlayerControllerComponent targetingComponent;


#if UNITY_EDITOR
        // LOAD DEFAULTS
        private void OnValidate()
        {

        }
#endif

        // -------------------------------------------------------------------------------
        // OnEnable
        // -------------------------------------------------------------------------------
        void OnEnable()
        {

            if (!targetName)
            {
                Debug.LogError("No targetName Pannel !");
                return;
            }

            if (!healthBar)
            {
                Debug.LogError("No healthBar Pannel !");
                return;
            }

            if (!manaBar)
            {
                Debug.LogError("No manaBar Pannel !");
                return;
            }

            if (!targetPannel)
            {
                Debug.LogError("Targeting Component: no UI Target Pannel !");
                return;
            }
            targetPannel.SetActive(false);

            if (!targetButton)
            {
                Debug.LogError("No Target Button !");
                return;
            }
            targetButton.onClick.SetListener(() =>
            {
                if (targetPannel)
                {

                    if (targetingComponent)
                    {

                        targetingComponent.onTargetButtonClicked();
                    }
                    else
                    {

                        Debug.LogError("No targetingComponent !");
                    }


                }
                else
                {

                    Debug.LogError("OnClick() - faild no goPanel");
                }
            });

        }

        void LateUpdate()
        {

            // should happen in OnStart ?
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
