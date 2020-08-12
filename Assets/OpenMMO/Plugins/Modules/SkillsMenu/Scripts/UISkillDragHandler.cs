using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISkillDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{

    public GameObject iconPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrag(PointerEventData data)
    {
        Debug.Log("draggign");
        iconPrefab.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData data)
    {
        iconPrefab.transform.localPosition = Vector3.zero;
    }

}
