using System.Net.Mime;
//BY DX4D
using UnityEngine.UI;
using UnityEngine;
using OpenMMO.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace OpenMMO.SkillsMenu
{
    // parent handles button clicks
    [RequireComponent(typeof(Button))]
    public partial class UISkillMenuSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
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

        public GameObject skillDragPrefab;

        GameObject draggedSkill;

        //INIT
        public void Init(SkillTemplate _skill)
        {
            // set skillTemplate to ui elements
            skill = _skill;
            nameText.text = _skill.name;
            baseDamageText.text = _skill.baseImpact.ToString();
            skillIcon.sprite = _skill.icon;
        }


        public void OnBeginDrag(PointerEventData data)
        {
            draggedSkill = Instantiate(skillDragPrefab.gameObject, transform, true);

            draggedSkill.GetComponent<UIDragObjectScript>().Init(skill);

            Canvas.ForceUpdateCanvases();
        }

        public void OnDrag(PointerEventData data)
        {
            draggedSkill.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData data)
        {

            List<GameObject> hoveredList = data.hovered;

            foreach (GameObject go in hoveredList)
            {

                UISkillSlot uiSkillSlot = go.GetComponent<UISkillSlot>();
                if (uiSkillSlot)
                {
                    Debug.Log("found uiskillslot");
                    uiSkillSlot.Init(skill);
                }
                else
                {
                    Debug.Log("did NOT found uiskillslot");
                }
            }

            Destroy(draggedSkill);
        }
    }
}
