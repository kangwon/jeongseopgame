using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUpdateController : MonoBehaviour
{
    Button openEpisodeSelectButton;
    Button collectionButton;
    Button optionButton;
    GameObject collectionPanel;
    GameObject episodeSelectPanel;
    GameObject optionPanel;
    Text starText;
    // Start is called before the first frame update
    void Start()
    {
        openEpisodeSelectButton = GameObject.Find("Canvas/EpisodeSelectButton").GetComponent<Button>();
        openEpisodeSelectButton.onClick.AddListener(OnClickEpisodeSelectButton);
        episodeSelectPanel = GameObject.Find("Canvas/EpisodeSelectPanel");
        starText = GameObject.Find("Canvas/UpperUI/Star/Text").GetComponent<Text>();
        optionPanel = GameObject.Find("Canvas").transform.Find("OptionPanel").gameObject; //비활성화된 오브젝트를 찾을 땐 이 방법을 쓰는게 편함.
        collectionPanel = GameObject.Find("Canvas/CollectionPanel");
        optionButton = GameObject.Find("Canvas/UpperUI/OptionButton").GetComponent<Button>();
        optionButton.onClick.AddListener(OnClickOptionButton);
        collectionButton = GameObject.Find("Canvas/CollectionButton").GetComponent<Button>();
        collectionButton.onClick.AddListener(() => collectionPanel.SetActive(true));
    }

    // Update is called once per frame
    void Update()
    {
        starText.text = $"({SaveData.Instance.TotalClearStar}/{StoryCollection.AllStory.Count * 3})";
    }

    void OnClickEpisodeSelectButton()
    {
        episodeSelectPanel.SetActive(!episodeSelectPanel.activeSelf);
    }

    void OnClickOptionButton()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
    }

}

