using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUpdateController : MonoBehaviour
{
    Button openEpisodeSelectButton;
    Button collectionButton;
    GameObject collectionPanel;
    GameObject episodeSelectPanel;
    Text starText;
    // Start is called before the first frame update
    void Start()
    {
        openEpisodeSelectButton = GameObject.Find("Canvas/EpisodeSelectButton").GetComponent<Button>();
        openEpisodeSelectButton.onClick.AddListener(OnClickEpisodeSelectButton);
        episodeSelectPanel = GameObject.Find("Canvas/EpisodeSelectPanel");
        starText = GameObject.Find("Canvas/UpperUI/Star/Text").GetComponent<Text>();
        collectionPanel = GameObject.Find("Canvas/CollectionPanel");
        collectionButton = GameObject.Find("Canvas/CollectionButton").GetComponent<Button>();
        collectionButton.onClick.AddListener(() => collectionPanel.SetActive(true));
    }

    // Update is called once per frame
    void Update()
    {
        starText.text = $"{SaveData.Instance.TotalClearStar}";
    }

    void OnClickEpisodeSelectButton()
    {
        episodeSelectPanel.SetActive(!episodeSelectPanel.activeSelf);
    }
}
