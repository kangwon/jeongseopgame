using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroPanelController : MonoBehaviour
{

    Button okButton;
    Button cancelButton;
    Text introText;
    Episode episode;
    GameObject EpisodeSelectPanel;
    void Start()
    {
        EpisodeSelectPanel = GameObject.Find("Canvas").transform.Find("EpisodeSelectPanel").gameObject;
        okButton = GameObject.Find("Canvas/EpisodeSelectPanel/IntroPanel/OkButton").GetComponent<Button>();
        okButton.onClick.AddListener(OnClickOkButton);
        cancelButton = GameObject.Find("Canvas/EpisodeSelectPanel/IntroPanel/CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(OnClickCancelButton);
        introText = GameObject.Find("IntroText").GetComponent<Text>();
    }

    void OnClickOkButton()
    {
        EpisodePlayer.SetEpisode(episode);
        SceneManager.LoadScene("GameScene");
    }
    void OnClickCancelButton()
    {
        EpisodeSelectPanel.SetActive(false);
    }

    public void Display(Episode episode)
    {
        this.introText.text = episode.intro.ToString();
        this.episode = episode;
    }
}
