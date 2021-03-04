using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectionPanelController : MonoBehaviour
{
    Vector2 defaultButtonSize;
    RectTransform rectTransform;
    GameObject buttonPrefab;
    GameObject buttonParent;
    Button backButton;
    List<GameObject> buttonList = new List<GameObject> { };
    // Start is called before the first frame update
    void Start()
    {
        buttonPrefab = Resources.Load<GameObject>("CollectionButtonPrefab");
        defaultButtonSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta;
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
        button.transform.GetChild(1).GetComponent<Text>().text =$"{ SaveData.Instance.EpisodeClearStar(item)}";
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            foreach(var otherButton in buttonList) //다른 버튼 사이즈는 그대로 바꾸기
            {
                otherButton.GetComponent<RectTransform>().sizeDelta = defaultButtonSize;
                otherButton.transform.GetChild(2).gameObject.SetActive(false);
                otherButton.transform.GetChild(3).gameObject.SetActive(false);
            }
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(0,250); // 누르면 해당 버튼 사이즈 커지게
            if (SaveData.Instance.EpisodeClearStar(item) > 0)
            {
                button.transform.GetChild(3).GetComponent<Text>().text = item.intro.ToString(); //별이 1개라도 있으면 intro는 보이게 
                button.transform.GetChild(3).gameObject.SetActive(true);
                if (SaveData.Instance.EpisodeClearStar(item) > 1) //클리어 별이 2개 이상일때 도감에서 플레이 가능
                {
                    button.transform.GetChild(2).gameObject.SetActive(true);
                    button.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => OnClickEpisodeButton(item));
                }
            }
            else 
            {
                button.transform.GetChild(3).GetComponent<Text>().text = "[잠김]";
                button.transform.GetChild(3).gameObject.SetActive(true);
            }
        });
        }
    }
    public void BackButton()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickEpisodeButton(Episode episode)
    {
        EpisodePlayer.SetEpisode(episode);
        SceneManager.LoadScene("GameScene");
    }
}
