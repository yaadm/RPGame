//BY DX4D
using UnityEngine;
using OpenMMO.UI;

namespace OpenMMO.Chracter
{
    [DisallowMultipleComponent]
    public partial class UIWindowCharacter : UIRoot
    {
        [Header("Chracter Panel")]
        public GameObject characterPanel;

        public static UIWindowCharacter singleton;

        //AWAKE
        protected override void Awake()
        {
            singleton = this;
            base.Awake();
            this.transform.parent.gameObject.SetActive(false);
        }

        //UPDATE
        //protected override void Update()
        //{
        //    base.Update();
        //}
        //THROTTLED UPDATE
        protected override void ThrottledUpdate()
        {
            if (!networkManager || networkManager.state != Network.NetworkState.Game)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

}
