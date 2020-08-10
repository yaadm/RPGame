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

        public Image targetHealthBar;

        public Text targetHealthText;

        public Image targetManaBar;

        public void onTargetLateUpdate()
        {
            if (playerController.currentTarget)
            {

                // TODO: update UI components to match the targeted player

                PlayerControllerComponent pcc = playerController.currentTarget.GetComponent<PlayerControllerComponent>();

                if (pcc)
                {

                    targetName.text = pcc.name;
                    targetHealthText.text = pcc.currentHealth.ToString() + "/" + pcc.totalHealth.ToString();

                    // TODO: animate the bar lowering down.
                    targetHealthBar.fillAmount = (float)pcc.currentHealth / (float)pcc.totalHealth;
                    targetManaBar.fillAmount = (float)pcc.currentMana / (float)pcc.totalMana;

                    targetPannel.SetActive(true);
                }
            }
            else
            {
                // no target
                targetPannel.SetActive(false);
            }
        }
    }
}
