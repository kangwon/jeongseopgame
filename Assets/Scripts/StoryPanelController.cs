using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryPanelController : MonoBehaviour
{
    Text storyText;
    Button backButton;
    
    void Start()
    {
        storyText = GameObject.Find("StoryText").GetComponent<Text>();
        backButton = GameObject.Find("Canvas/BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(OnClickBackButton);
        
        if (EpisodePlayer.isReady)
            OnPageUpdated(EpisodePlayer.CurrentPage);
    }
    void OnClickBackButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnPageUpdated(Page page)
    {
        storyText.text = page.body.ToString();
    }
}
