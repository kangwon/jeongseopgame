using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) SaveData.Save();
        if (Input.GetKeyDown(KeyCode.L)) SaveData.Load();
    }
}
