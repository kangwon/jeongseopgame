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
        if (StoryPlayer.isReady)
        {
            StartCoroutine(OnPassageUpdatedCoroutine(StoryPlayer.CurrentPassage));
        }
    }
 
    IEnumerator OnPassageUpdatedCoroutine(Passage passage)
    {
        while (!storyPanel.endTyping)
        {
            yield return null;
        }
        storyPanel.endTyping = false;
        if (passage.isEnd) //해당 페이지가 끝인 경우
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
            for (int i = 0; i < passage.links.Count; i++)
            {
                Link link = passage.links[i];
                GameObject button = Instantiate(ButtonPrefab, ButtonHolder.transform);
                buttonList.Add(button);
                button.transform.GetComponentInChildren<Text>().text = link.name;
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    StoryPlayer.SelectLink(link);
                    while (buttonList.Count != 0)
                    {
                        Destroy(buttonList.ElementAt(0));
                        buttonList.RemoveAt(0);
                    }
                    storyPanel.OnPassageUpdated();
                    var temp = OnPassageUpdatedCoroutine(StoryPlayer.CurrentPassage);
                    StartCoroutine(temp);
                    Debug.Log($"Selected: {link.name}");
                });
            }
        }
    }
}
