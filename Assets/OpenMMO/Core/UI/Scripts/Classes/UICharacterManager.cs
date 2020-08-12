using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

    // ===================================================================================
    // UITargetEnemy
    // ===================================================================================
    public partial class UICharacterManager : UIManager
    {

        protected PlayerControllerComponent playerController;

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

            if (!targetHealthBar)
            {
                Debug.LogError("No healthBar Pannel !");
                return;
            }

            if (!targetManaBar)
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

                    if (playerController)
                    {

                        playerController.onTargetButtonClicked();
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

            // BasicAttack.onClick.SetListener(() =>
            // {
            //     if (playerController)
            //     {

            //         playerController.onSkillClicked();
            //     }
            //     else
            //     {

            //         Debug.LogError("No targetingComponent !");
            //     }
            // });
        }

        public void onSkillClicked(SkillTemplate skill)
        {
            playerController.onSkillClicked(skill);
        }

        void LateUpdate()
        {
            if (!playerController)
            {

                if (!PlayerComponent.localPlayer)
                {
                    Debug.LogWarning("no local player yet...");
                    return;
                }

                playerController = PlayerComponent.localPlayer.GetComponent<PlayerControllerComponent>();

                if (!playerController)
                {
                    Debug.LogError("no targetingComponent !");
                    return;
                }
            }

            onTargetLateUpdate();

            onPortraitLateUpdate();
        }

    }
}
