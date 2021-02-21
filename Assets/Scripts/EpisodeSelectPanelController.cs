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
        episodes = new Episode[3];
        //TODO: 에피소드가 1~2개 일때 따로 조건문으로 처리하기(그러려면 해결된 에피소드가 저장되어있어야함)
        SelectRandomEpisode();

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
    void SelectRandomEpisode()
    {
        Combination c = new Combination(EpisodeCollection.Instance.EpisodeExceptTutorial.Count, 3); // nC3 = n개(튜토리얼를 제외한 에피소드의 수) 중 3개를 조합
        int index = Random.Range(0, c.data.Count); //전체 조합중 하나를 랜덤으로 선택
        int k = 0;
        foreach (var episodeNum in c.data.ElementAt(index)) //선택한 조합 순서로 에피소드를 넣는다.
        {
            episodes[k] = EpisodeCollection.Instance.EpisodeExceptTutorial.ElementAt(episodeNum);
            k++;
        }
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
