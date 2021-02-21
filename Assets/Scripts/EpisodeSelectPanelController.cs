using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
public class EpisodeSelectPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    Button openPanelButton;
    Button[] episodeButton = new Button[3];
    GameObject episodeSelectPanel;
    IntroPanelController introPanel;

    Episode[] episodes;

    void Start()
    {
        episodes = new Episode[3] 
        {
            Episode.Load("sample"), Episode.Load("sample"), Episode.Load("sample"),
        };
        EpisodeCollection.Instance.PrintEpisode(); // 테스트용 //나중에 지우기
        episodeSelectPanel = GameObject.Find("Canvas").transform.Find("EpisodeSelectPanel").gameObject;
        introPanel = GameObject.Find("Canvas").transform.Find("IntroPanel").gameObject.GetComponent<IntroPanelController>();
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
    List<Episode> RandomEpisodeSelect(int index)
    {
        var episodes = EpisodeCollection.Instance.EpisodeExceptTutorial;
        var tempEpisodes = new List<Episode> { };
        
        tempEpisodes.Add(episodes.ElementAt(Random.Range(0, episodes.Count)));
        return tempEpisodes;
    }

    void OnClickEpisodeSelectButton()
    {
        episodeSelectPanel.SetActive(!episodeSelectPanel.activeSelf);       
    }
    void OnClickEpisodeButton(Episode episode)
    {
        introPanel.Display(episode);
    }
}
