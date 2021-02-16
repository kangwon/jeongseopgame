using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPanelController : MonoBehaviour
{
    GameObject ButtonPrefab;
    GameObject ButtonHolder;

    void Start()
    {
        ButtonPrefab = Resources.Load<GameObject>("AtionButtonPrefab");
        ButtonHolder = GameObject.Find("ButtonHolder").gameObject;
    }
}
