//BY DX4D
using UnityEngine;
using OpenMMO.UI;

namespace OpenMMO.Settings
{
    [DisallowMultipleComponent]
    public partial class UIWindowSettings : UIRoot
    {
        [Header("Settings Panel")]
        public GameObject settingsPanel;

        public static UIWindowSettings singleton;

        //AWAKE
        protected override void Awake()
        {
            singleton = this;
            base.Awake();
            this.transform.parent.gameObject.SetActive(false);
        }
        //THROTTLED UPDATE
        protected override void ThrottledUpdate()
        {
            if (!networkManager || networkManager.state != Network.NetworkState.Game)
                Hide();
            else
                Show();
        }
    }

}
