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
    EpisodePanelController episodePanel;

    Episode[] episodes;

    void Start()
    {
        episodes = new Episode[3] 
        {
            Episode.Load("sample"), Episode.Load("sample"), Episode.Load("sample"),
        };

        episodeSelectPanel = GameObject.Find("Canvas").transform.Find("EpisodeSelectPanel").gameObject;
        episodePanel = GameObject.Find("Canvas").transform.Find("EpisodePanel").gameObject.GetComponent<EpisodePanelController>();
        openPanelButton = GameObject.Find("Canvas/EpisodeSelectButton").GetComponent<Button>();
        openPanelButton.onClick.AddListener(OnClickEpisodeSelectButton);
        for (int i = 0; i < 3; i++)
        {
            var episode = episodes[i];
            episodeButton[i] = GameObject.Find($"Canvas/EpisodeSelectPanel/Episode{i + 1}Button").GetComponent<Button>();
            episodeButton[i].GetComponentInChildren<Text>().text = episode.title;
            episodeButton[i].onClick.AddListener(() => OnClickEpisodeButton(episode));
        }
        episodeSelectPanel.SetActive(false);
    }
    void OnClickEpisodeSelectButton()
    {
        episodeSelectPanel.SetActive(!episodeSelectPanel.activeSelf);       
    }
    void OnClickEpisodeButton(Episode episode)
    {
        episodePanel.Display(episode);
    }
}
