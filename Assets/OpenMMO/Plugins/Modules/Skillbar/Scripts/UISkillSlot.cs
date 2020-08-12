using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{
    public class UISkillSlot : MonoBehaviour
    {

        public Image icon;

        [HideInInspector]
        public SkillTemplate skill;

        public void Init(SkillTemplate _skill)
        {
            skill = _skill;
            icon.sprite = _skill.icon;
            icon.gameObject.SetActive(true);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
