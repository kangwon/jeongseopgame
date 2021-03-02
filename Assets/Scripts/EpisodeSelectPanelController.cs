using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class EpisodeSelectPanelController : MonoBehaviour
{
    Vector2 achorPosition = new Vector2(0, 0);
    RectTransform rectTransform;
    Button openPanelButton;
    Button[] episodeButton = new Button[3];
    IntroPanelController introPanel;
    List<Episode> episodes = new List<Episode> { };
    bool firstStart = true;
    void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        introPanel = GameObject.Find("Canvas").transform.Find("EpisodeSelectPanel/IntroPanel").gameObject.GetComponent<IntroPanelController>();
        openPanelButton = GameObject.Find("Canvas/EpisodeSelectButton").GetComponent<Button>();
        openPanelButton.onClick.AddListener(OnClickEpisodeSelectButton);
        for (int i = 0; i < 3; i++)
        {
            episodeButton[i] = GameObject.Find($"Canvas/EpisodeSelectPanel/ButtonPanel/Episode{i + 1}Button").GetComponent<Button>();
        }
        this.gameObject.SetActive(false);
        firstStart = false;
    }
    private void OnEnable()
    {
        if (!firstStart)
        {
            rectTransform.anchoredPosition = achorPosition;
            episodes = SelectTutorialEpisode();
            bool isTutorial = true;
            if (episodes.Count == 0) //남은 튜토리얼이 없을 때
            {
                isTutorial = false;
                episodes = SelectRandomEpisode();
            }
            for (int i = 0; i < 3; i++)
            {
                if (i > episodes.Count - 1)
                {
                    episodeButton[i].onClick.RemoveAllListeners();
                    if (isTutorial)
                    {
                        episodeButton[i].GetComponentInChildren<Text>().text = "튜토리얼 에피소드 없음";
                        episodeButton[i].onClick.AddListener(() => Debug.Log("모든 튜토리얼을 클리어 해주세요."));
                    }
                    else
                    {
                        episodeButton[i].GetComponentInChildren<Text>().text = "에피소드 없음";
                        episodeButton[i].onClick.AddListener(() => Debug.Log("남은 에피소드가 없습니다."));
                    }
                }
                else
                {
                    var episode = episodes[i];
                    episodeButton[i].GetComponentInChildren<Text>().text = episode.title;
                    episodeButton[i].onClick.AddListener(() => OnClickEpisodeButton(episode));
                }
            }          
        }
    }
    List<Episode> SelectTutorialEpisode() //선택 가능한 에피소드의 수를 리턴 (최대 3)
    {
        var episodeList = new List<Episode> { };
        foreach(var episode in EpisodeCollection.Instance.EpisodeTutorial)
        {
            if(!SaveData.Instance.ClearEpisodeList.Exists(id=>id==episode.id)) // 저장된 데이터에 해당 에피소드가 없을경우
                episodeList.Add(episode); //현재 클리어가 안된 튜토리얼을 리스트에 저장한다.
        }
        return CombinationEpisode(episodeList);
    }
    List<Episode> SelectRandomEpisode() //선택 가능한 에피소드의 수를 리턴 (최대 3)
    {
        var episodeList = new List<Episode> { };
        foreach (var episode in EpisodeCollection.Instance.EpisodeExceptTutorial)
        {
            if (!SaveData.Instance.ClearEpisodeList.Exists(id => id == episode.id)) // 저장된 데이터에 해당 에피소드가 없을경우
                episodeList.Add(episode); //현재 클리어가 안된 에피소드을 리스트에 저장한다.
        }
        return CombinationEpisode(episodeList);
    }
    List<Episode> CombinationEpisode(List<Episode> episodeList)
    {
        var maxSelect = episodeList.Count > 3 ? 3 : episodeList.Count;
        Combination c = new Combination(episodeList.Count, maxSelect); // nCr = n개 중 r개를 조합
        var tempEpisodes = new List<Episode> { };
        if (c.data.Count != 0)
        {
            int index = Random.Range(0, c.data.Count); //전체 조합중 하나를 랜덤으로 선택
            foreach (var episodeNum in c.data[index]) //선택한 조합 순서로 에피소드를 넣는다.
            {
                tempEpisodes.Add(episodeList[episodeNum]);
            }
        }
        return tempEpisodes;
    }
    void OnClickEpisodeSelectButton()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);       
    }
    void OnClickEpisodeButton(Episode episode)
    {
        introPanel.Display(episode);
    }
}
