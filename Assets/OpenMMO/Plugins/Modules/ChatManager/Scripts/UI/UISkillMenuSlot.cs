//BY DX4D
using UnityEngine;
using OpenMMO.UI;
using TMPro;

namespace OpenMMO.Chat
{
    public partial class UISkillMenuSlot : UIButton
    {
#pragma warning disable CS0649
        [Header("TEXT LABEL")]
        [SerializeField] TextMeshProUGUI label;
#pragma warning restore CS0649

        [HideInInspector]
        SkillTemplate skill;

        //INIT
        public void Init(SkillTemplate _skill)
        {
            // set skillTemplate to ui elements
            skill = _skill;
            label.text = _skill.name;
        }
    }
}
