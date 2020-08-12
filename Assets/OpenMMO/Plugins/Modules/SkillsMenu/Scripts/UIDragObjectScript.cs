using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.SkillsMenu
{
    public class UIDragObjectScript : MonoBehaviour
    {

        public Image icon;

        [HideInInspector]
        public SkillTemplate skill;

        public void Init(SkillTemplate _skill)
        {
            skill = _skill;
            icon.sprite = _skill.icon;
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
