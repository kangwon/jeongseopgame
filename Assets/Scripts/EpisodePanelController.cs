using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EpisodePanelController : MonoBehaviour
{
    public static Vector3 DisplayPosition = new Vector3(0, 0, 0);

    Button okButton;
    Button cancelButton;
    Text episodeText;

    Episode episode;

    void Start()
    {
        okButton = GameObject.Find("Canvas/EpisodePanel/OkButton").GetComponent<Button>();
        okButton.onClick.AddListener(OnClickOkButton);
        cancelButton = GameObject.Find("Canvas/EpisodePanel/CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(OnClickCancelButton);
        episodeText = GameObject.Find("Canvas/EpisodePanel/EpisodeText").GetComponent<Text>();
    }

    void OnClickOkButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    void OnClickCancelButton()
    {
        this.gameObject.SetActive(false);
    }

    public void Display(Episode episode)
    {
        this.episodeText.text = episode.intro.ToString();
        this.episode = episode;

        this.gameObject.transform.localPosition = DisplayPosition;
        this.gameObject.SetActive(true);
    }
}
