using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EpisodeSelectPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    Button openPanelButton;
    Button[] episodeButton = new Button[3];
    GameObject episodeSelectPanel;
    GameObject episodePanel;
    void Start()
    {
        episodeSelectPanel = GameObject.Find("Canvas").transform.Find("EpisodeSelectPanel").gameObject;
        episodePanel = GameObject.Find("Canvas").transform.Find("EpisodePanel").gameObject;
        openPanelButton = GameObject.Find("Canvas/EpisodeSelectButton").GetComponent<Button>();
        for (int i = 0; i < 3; i++)
        {
            episodeButton[i] = GameObject.Find($"Canvas/EpisodeSelectPanel/Episode{i + 1}Button").GetComponent<Button>();
            episodeButton[i].onClick.AddListener(() => OnClickEpisodeButton());
        }
        episodeSelectPanel.SetActive(false);
        openPanelButton.onClick.AddListener(() => OnClickEpisodeSelectButton());
    }
    void OnClickEpisodeSelectButton()
    {
        if (episodeSelectPanel.activeSelf == false)
        {
            Debug.Log(episodeSelectPanel.activeSelf);
            episodeSelectPanel.SetActive(true);
        }
        else
            episodeSelectPanel.SetActive(false);
    }
    void OnClickEpisodeButton()
    {
        episodePanel.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
