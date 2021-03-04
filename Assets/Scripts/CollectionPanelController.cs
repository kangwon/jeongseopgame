using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionPanelController : MonoBehaviour
{
    RectTransform rectTransform;
    GameObject buttonPrefab;
    GameObject buttonParent;
    Button backButton;
    List<GameObject> buttonList = new List<GameObject> { };
    // Start is called before the first frame update
    void Start()
    {
        buttonPrefab = Resources.Load<GameObject>("CollectionButtonPrefab");
        buttonParent = GameObject.Find("CollectionPanel/CollectionButtonListPanel/ScrollView/Viewport/Content");
        backButton = GameObject.Find("CollectionPanel/BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(BackButton);
        rectTransform = this.gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -20);
        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if(buttonPrefab != null)
        {
            while (buttonList.Count != 0)
            {
                Destroy(buttonList[0]);
                buttonList.RemoveAt(0);
            }
            AddAllEpisodeButtons();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddAllEpisodeButtons()
    {
        foreach(var item in EpisodeCollection.Instance.AllEpisode)
        {
        GameObject button = Instantiate(buttonPrefab, buttonParent.transform);
        buttonList.Add(button);
        button.transform.GetComponentInChildren<Text>().text = item.title;
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(0,100); // 누르면 버튼 사이즈 커지게
        });
        }
    }
    public void BackButton()
    {
        this.gameObject.SetActive(false);
    }
}
