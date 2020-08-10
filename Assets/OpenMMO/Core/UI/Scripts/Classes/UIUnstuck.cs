
using UnityEngine;
using UnityEngine.UI;
using System;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{

    // ===================================================================================
    // UIShortcut
    // ===================================================================================
    [RequireComponent(typeof(Button))]
    public partial class UIUnstuck : MonoBehaviour
    {

        [Header("System Texts")]
        public string popupEnter = "Are you stuck ?";

        private string targetAnchor = "ZoneAnchor"; // this is the default anchor name for all realms.

        // -------------------------------------------------------------------------------
        // OnEnable
        // -------------------------------------------------------------------------------
        void OnEnable()
        {
            GetComponent<Button>().onClick.SetListener(() =>
            {

                if (PlayerComponent.localPlayer)
                {


                    UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetAnchor), OnClickConfirm);


                }
            });

        }

        // -------------------------------------------------------------------------------

        void OnClickConfirm()
        {
            PlayerComponent pc = PlayerComponent.localPlayer.GetComponentInParent<PlayerComponent>();
            pc.Cmd_WarpLocal(targetAnchor);
        }
    }

}

// =======================================================================================