using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SelectPanelController : MonoBehaviour
{
    GameObject ButtonPrefab;
    GameObject ButtonHolder;
    StoryPanelController storyPanel;
    List<GameObject> buttonList = new List<GameObject> { };
    void Start()
    {
        ButtonPrefab = Resources.Load<GameObject>("ActionButtonPrefab");
        ButtonHolder = GameObject.Find("ButtonHolder").gameObject;
        storyPanel = GameObject.Find("StoryPanel").GetComponent<StoryPanelController>();
        if (EpisodePlayer.isReady)
            OnPageUpdated(EpisodePlayer.CurrentPage);
    }

    public void OnPageUpdated(Page page)
    {
        for (int i = 0; i < page.actions.Count; i++)
        {
            Action action = page.actions[i];
            GameObject button = Instantiate(ButtonPrefab, ButtonHolder.transform);
            buttonList.Add(button);
            button.transform.GetComponentInChildren<Text>().text = action.title;
            button.GetComponent<Button>().onClick.AddListener(()=> 
            {
                Debug.Log($"선택지 수: {buttonList.Count}");
                while(buttonList.Count !=0)
                {
                    Destroy(buttonList.ElementAt(0));
                    buttonList.RemoveAt(0);
                }
                storyPanel.OnPageUpdated(EpisodePlayer.CurrentEpisode.pages[action.linkedPageId]);
                OnPageUpdated(EpisodePlayer.CurrentEpisode.pages[action.linkedPageId]);
                Debug.Log($"Selected: {action.title}");
            });
        }
    }
}
