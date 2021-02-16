using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanelController : MonoBehaviour
{
    GameObject ButtonPrefab;
    GameObject ButtonHolder;

    void Start()
    {
        ButtonPrefab = Resources.Load<GameObject>("ActionButtonPrefab");
        ButtonHolder = GameObject.Find("ButtonHolder").gameObject;

        if (EpisodePlayer.isReady)
            OnPageUpdated(EpisodePlayer.CurrentPage);
    }

    void OnPageUpdated(Page page)
    {
        for (int i = 0; i < page.actions.Count; i++)
        {
            Action action = page.actions[i];
            GameObject button = Instantiate(ButtonPrefab, ButtonHolder.transform);

            button.transform.GetComponentInChildren<Text>().text = action.title;
            button.GetComponent<Button>().onClick.AddListener(()=> 
            { 
                Debug.Log($"Selected: {action.title}");
            });
        }
    }
}
