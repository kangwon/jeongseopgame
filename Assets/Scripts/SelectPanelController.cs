using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SelectPanelController : MonoBehaviour
{
    GameObject ButtonPrefab;
    GameObject ButtonHolder;
    GameObject resultPanel;
    StoryPanelController storyPanel;
    List<GameObject> buttonList = new List<GameObject> { };
    void Start()
    {
        ButtonPrefab = Resources.Load<GameObject>("ActionButtonPrefab");
        ButtonHolder = GameObject.Find("ButtonHolder").gameObject;
        storyPanel = GameObject.Find("StoryPanel").GetComponent<StoryPanelController>();
        resultPanel = GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject;
        if (EpisodePlayer.isReady)
        {
            StartCoroutine(OnPageUpdatedCoroutine(EpisodePlayer.CurrentPage));
        }
    }
 
    IEnumerator OnPageUpdatedCoroutine(Page page)
    {
        while (!storyPanel.endTyping)
        {
            yield return null;
        }
        storyPanel.endTyping = false;
        if (page.isEnd) //해당 페이지가 끝인 경우
        {
            GameObject button = Instantiate(ButtonPrefab, ButtonHolder.transform);
            buttonList.Add(button);
            button.transform.GetComponentInChildren<Text>().text = "[엔딩]";
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                while (buttonList.Count != 0)
                {
                    Destroy(buttonList.ElementAt(0));
                    buttonList.RemoveAt(0);
                }
                resultPanel.SetActive(true);
            });
        }
        else //페이지가 끝이 아닐 경우
        {
            for (int i = 0; i < page.actions.Count; i++)
            {
                Action action = page.actions[i];
                GameObject button = Instantiate(ButtonPrefab, ButtonHolder.transform);
                buttonList.Add(button);
                button.transform.GetComponentInChildren<Text>().text = action.title;
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    EpisodePlayer.SelectAction(action);
                    while (buttonList.Count != 0)
                    {
                        Destroy(buttonList.ElementAt(0));
                        buttonList.RemoveAt(0);
                    }
                    storyPanel.OnPageUpdated(EpisodePlayer.CurrentPage);
                    var temp =OnPageUpdatedCoroutine(EpisodePlayer.CurrentPage);
                    StartCoroutine(temp);
                    Debug.Log($"Selected: {action.title}");
                });
            }
        }
    }
}
