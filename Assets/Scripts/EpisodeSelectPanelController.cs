using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EpisodeSelectPanelController : MonoBehaviour
{
    Vector2 achorPosition = new Vector2(0, 0);
    RectTransform rectTransform;
    
    Button[] episodeButton = new Button[3];
    IntroPanelController introPanel;
    List<Story> stories = new List<Story>();
    
    bool firstStart = true;
    void Start()
    {
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        introPanel = GameObject.Find("Canvas").transform.Find("EpisodeSelectPanel/IntroPanel").gameObject.GetComponent<IntroPanelController>();
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
            stories = SelectTutorialStory();
            bool isTutorial = true;
            if (stories.Count == 0) //남은 튜토리얼이 없을 때
            {
                isTutorial = false;
                stories = SelectRandomStory();
            }
            for (int i = 0; i < 3; i++)
            {
                if (i > stories.Count - 1)
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
                    var story = stories[i];
                    episodeButton[i].GetComponentInChildren<Text>().text = story.name;
                    episodeButton[i].onClick.AddListener(() => OnClickEpisodeButton(story));
                }
            }          
        }
    }
    List<Story> SelectTutorialStory()
    {
        var storyList = new List<Story>();
        foreach(var story in StoryCollection.TutorialStories)
        {
            if(!SaveData.HasCleared(story.name)) // 해당 스토리를 클리어한 적이 없을 경우
                storyList.Add(story); //현재 클리어가 안 된 튜토리얼을 리스트에 저장한다.
        }
        return CombinationStory(storyList);
    }
    List<Story> SelectRandomStory()
    {
        var storyList = new List<Story>();
        foreach (var story in StoryCollection.NotTutorialStories)
        {
            if(!SaveData.HasCleared(story.name)) // 해당 스토리를 클리어한 적이 없을 경우
                storyList.Add(story); //현재 클리어가 안된 에피소드을 리스트에 저장한다.
        }
        return CombinationStory(storyList);
    }
    List<Story> CombinationStory(List<Story> storyList)
    {
        var maxSelect = storyList.Count > 3 ? 3 : storyList.Count;
        Combination c = new Combination(storyList.Count, maxSelect); // nCr = n개 중 r개를 조합
        var randomStories = new List<Story>();
        if (c.data.Count != 0)
        {
            int index = Random.Range(0, c.data.Count); //전체 조합중 하나를 랜덤으로 선택
            foreach (var i in c.data[index]) //선택한 조합 순서로 에피소드를 넣는다.
            {
                randomStories.Add(storyList[i]);
            }
        }
        return randomStories;
    }

    void OnClickEpisodeButton(Story story)
    {
        introPanel.Display(story);
    }
}
