using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroPanelController : MonoBehaviour
{
    public static Vector3 DisplayPosition = new Vector3(0, 0, 0);

    Button okButton;
    Button cancelButton;
    Text introText;

    Episode episode;

    void Start()
    {
        okButton = GameObject.Find("Canvas/IntroPanel/OkButton").GetComponent<Button>();
        okButton.onClick.AddListener(OnClickOkButton);
        cancelButton = GameObject.Find("Canvas/IntroPanel/CancelButton").GetComponent<Button>();
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
        this.gameObject.SetActive(false);
    }

    public void Display(Episode episode)
    {
        this.introText.text = episode.intro.ToString();
        this.episode = episode;

        this.gameObject.transform.localPosition = DisplayPosition;
        this.gameObject.SetActive(true);
    }
}
