using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenMMO.Network;

namespace OpenMMO.UI
{

    public class UISkillBar : UIRoot
    {

        [Header("SkillsBar Settings")]
        public List<UISkillSlot> skillsSlots;

        public UICharacterManager characterManager;

        private void Start()
        {

            // go over children and add to list 
            // add click listener to each
            skillsSlots = new List<UISkillSlot>();
            UISkillSlot[] list = gameObject.GetComponentsInChildren<UISkillSlot>();
            Debug.Log("num of Slots detected: " + list.Length);
            foreach (UISkillSlot skillslot in list)
            {
                skillslot.GetComponent<Button>().onClick.SetListener(() => onSkillbuttonClicked(skillslot));
                skillsSlots.Add(skillslot);
            }
        }

        void onSkillbuttonClicked(UISkillSlot skillslot)
        {
            Debug.Log("onSkillbuttonClicked()");
            characterManager.onSkillClicked(skillslot.skill);
        }

        // -------------------------------------------------------------------------------
        // ThrottledUpdate
        // -------------------------------------------------------------------------------
        protected override void ThrottledUpdate()
        {

            if (!networkManager || networkManager.state != NetworkState.Game)
                Hide();
            else
                Show();

        }

        // -------------------------------------------------------------------------------
    }
}
