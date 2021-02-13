using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryPanelController : MonoBehaviour
{
    Button backButton;
    // Start is called before the first frame update
    void Start()
    {
        backButton = GameObject.Find("Canvas/BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(() => OnClickBackButton());
    }
    void OnClickBackButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
