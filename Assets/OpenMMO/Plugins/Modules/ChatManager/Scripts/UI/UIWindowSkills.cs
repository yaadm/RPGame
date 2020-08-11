//IMPROVEMENTS BY DX4D
using System;
using UnityEngine;
using UnityEngine.UI;
using OpenMMO.UI;

namespace OpenMMO.Chat
{

    // ===================================================================================
    // UIWindowChat
    // ===================================================================================
    [DisallowMultipleComponent]
    public partial class UIWindowSkills : UIRoot
    {

        [Header("skill Slot Prefab")]
        public UISkillMenuSlot skillEntryPrefab;

        [Header("Chat Content View")]
        public Transform contentViewport;
        public ScrollRect scrollRect;

        public static UIWindowSkills singleton;

        // -------------------------------------------------------------------------------
        // Awake
        // -------------------------------------------------------------------------------
        protected override void Awake()
        {
            singleton = this;
            base.Awake();
            this.transform.parent.gameObject.SetActive(false);
        }

        void OnEnable()
        {


        }

        void addSkills()
        {

            if (!PlayerComponent.localPlayer || (!networkManager || networkManager.state != Network.NetworkState.Game))
            {
                return;
            }

            // clear skills list
            foreach (Transform child in contentViewport.transform)
            {
                Destroy(child.gameObject);
            }

            PlayerControllerComponent pcc = PlayerComponent.localPlayer.GetComponent<PlayerControllerComponent>();

            Debug.Log("skills count: " + pcc.characterClass.skillsList.Count);

            foreach (SkillTemplate skill in pcc.characterClass.skillsList)
            {
                GameObject go = Instantiate(skillEntryPrefab.gameObject, contentViewport.transform, false);

                go.GetComponent<UISkillMenuSlot>().Init(skill);

                go.GetComponent<Button>().onClick.SetListener(() => { OnSkillSelected(skill); });

                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0;
            }
        }

        void OnSkillSelected(SkillTemplate skill)
        {
            // populate skill details screen
        }

        // -------------------------------------------------------------------------------
        // Show
        // -------------------------------------------------------------------------------
        public override void Show()
        {
            base.Show();

            if (contentViewport.childCount <= 0)
            {
                addSkills();
            }
        }

        // -------------------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------------------
        // protected override void Update()
        // {
        //     base.Update();
        // }

        // -------------------------------------------------------------------------------
        // ThrottledUpdate
        // -------------------------------------------------------------------------------
        protected override void ThrottledUpdate()
        {
            if (!networkManager || networkManager.state != Network.NetworkState.Game)
                Hide();
            else
                Show();
        }
    }

}

// =======================================================================================