using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnLoad : MonoBehaviour
{

    private void Awake() {
        gameObject.SetActive(true);
    }
}
