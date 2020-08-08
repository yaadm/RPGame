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

        public GameObject myHealthBar;

        public GameObject myManaBar;

        private bool nameUpdated;

        public void onPortraitLateUpdate()
        {
            if (PlayerComponent.localPlayer)
            {
                if (!nameUpdated)
                {
                    myName.text = PlayerComponent.localPlayer.name;
                }
            }
        }
    }
}
