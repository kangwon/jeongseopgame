using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StoryPanelController : MonoBehaviour
{
    Text storyText;
    Button backButton;
    GameObject selectPanel;
    public bool EndTyping { get ; set; }
    private bool CheckSkipTyping { get; set; }
    void Start()
    {
        storyText = GameObject.Find("StoryText").GetComponent<Text>();
        backButton = GameObject.Find("Canvas/BackButton").GetComponent<Button>();
        selectPanel = GameObject.Find("Canvas/VertialLayout/SelectPanel");
        backButton.onClick.AddListener(OnClickBackButton);
        
        if (StoryPlayer.isReady)
            OnPassageUpdated();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // 대충 UI를 포함해서 raycast하는 코드... , physic2D에 있는 ratcast는 UI인식을 못함.
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> raycastResults = new List<RaycastResult> { };
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            foreach(var result in raycastResults)
            {
                if ((result.gameObject == this.gameObject)||(result.gameObject == selectPanel))
                {
                    CheckSkipTyping = true;
                }
            }
        }
    }

    void OnClickBackButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnPassageUpdated()
    {
        CheckSkipTyping = false;
        var temp = TypeCoroutine(storyText, StoryPlayer.CurrentPassage.text);
        StartCoroutine(temp);
    }
    IEnumerator TypeCoroutine(Text text, string description)
    {
        for (int i = 0; i < description.Length; i++)
        {
            if (!CheckSkipTyping)
            {
                text.text = description.Substring(0, i);
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                text.text = description;
                EndTyping = true;
                CheckSkipTyping = false;
                yield break;
            }
        }
        EndTyping = true;
    }
}
