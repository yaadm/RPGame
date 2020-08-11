using System.Net.Mime;
//BY DX4D
using UnityEngine.UI;
using UnityEngine;
using OpenMMO.UI;
using TMPro;

namespace OpenMMO.Chat
{
    public partial class UISkillMenuSlot : UIButton
    {
#pragma warning disable CS0649
        [Header("Name LABEL")]
        [SerializeField] TextMeshProUGUI nameText;

        [Header("Base Damage LABEL")]
        [SerializeField] TextMeshProUGUI baseDamageText;
#pragma warning restore CS0649

        public Image skillIcon;

        [HideInInspector]
        SkillTemplate skill;

        //INIT
        public void Init(SkillTemplate _skill)
        {
            // set skillTemplate to ui elements
            skill = _skill;
            nameText.text = _skill.name;
            baseDamageText.text = _skill.baseImpact.ToString();
            skillIcon.sprite = _skill.icon;

        }
    }
}
