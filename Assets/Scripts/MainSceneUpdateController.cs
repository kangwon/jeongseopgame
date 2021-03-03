using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUpdateController : MonoBehaviour
{
    Button openEpisodeSelectButton;
    GameObject EpisodeSelectPanel;
    Text starText;
    // Start is called before the first frame update
    void Start()
    {
        openEpisodeSelectButton = GameObject.Find("Canvas/EpisodeSelectButton").GetComponent<Button>();
        openEpisodeSelectButton.onClick.AddListener(OnClickEpisodeSelectButton);
        EpisodeSelectPanel = GameObject.Find("Canvas/EpisodeSelectPanel");
        starText = GameObject.Find("Canvas/UpperUI/Star/Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        starText.text = $"{SaveData.Instance.TotalClearStar}";
    }

    void OnClickEpisodeSelectButton()
    {
        EpisodeSelectPanel.SetActive(!EpisodeSelectPanel.activeSelf);
    }
}
