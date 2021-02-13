using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EpisodePanelController : MonoBehaviour
{
    GameObject episodePanel;
    Button okButton;
    Button cancelButton;
    Text episodeText;
    // Start is called before the first frame update
    void Start()
    {
        episodePanel = GameObject.Find("Canvas").transform.Find("EpisodePanel").gameObject;
        okButton = GameObject.Find("Canvas/EpisodePanel/OkButton").GetComponent<Button>();
        okButton.onClick.AddListener(()=> OnClickOkButton());
        cancelButton = GameObject.Find("Canvas/EpisodePanel/CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(()=>OnClickCancelButton());
        episodeText = GameObject.Find("Canvas/EpisodePanel/EpisodeText").GetComponent<Text>();
    }

    void OnClickOkButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    void OnClickCancelButton()
    {
        episodePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
