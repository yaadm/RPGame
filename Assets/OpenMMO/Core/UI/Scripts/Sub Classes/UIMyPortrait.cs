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
        [Header("My Portrait")]
        public Text myName;

        public Image myHealthBar;

        public Text myHealthText;

        public Image myManaBar;

        private bool nameUpdated;

        public void onPortraitLateUpdate()
        {
            if (playerController)
            {
                if (!nameUpdated)
                {
                    nameUpdated = true;
                    myName.text = playerController.name;
                }

                // TODO: check only if mana or health are updated (they will not be most of the time)

                myHealthText.text = playerController.currentHealth.ToString() + "/" + playerController.totalHealth.ToString();
                myHealthBar.fillAmount = (float)playerController.currentHealth / (float)playerController.totalHealth;
                myManaBar.fillAmount = (float)playerController.currentMana / (float)playerController.totalMana;
            }
        }
    }
}
