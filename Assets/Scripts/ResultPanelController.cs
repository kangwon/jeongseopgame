using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ResultPanelController : MonoBehaviour
{
    GameObject resultPanel;
    Button resultButton;
    GameObject[] stars = new GameObject[3];
    bool firstStart =true;
    // Start is called before the first frame update
    void Start()
    {
        resultPanel = GameObject.Find("ResultPanel").gameObject;
        resultButton = GameObject.Find("ResultPanel/ResultButton").GetComponent<Button>();
        resultButton.onClick.AddListener(OnClickResultButton);
        for (int i = 0; i < 3; i++)
            stars[i] = GameObject.Find($"ResultPanel/Star{i + 1}/").transform.Find("Image").gameObject;
        resultPanel.transform.localPosition = new Vector3(0,0,0);
        resultPanel.SetActive(false);
        firstStart = false;
    }
    void OnClickResultButton()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ResultStar(int starCount)
    {
        foreach(var star in stars)
            star.SetActive(false);
        if(3<=starCount)
                stars[2].SetActive(true);
        if(2<=starCount)
                stars[1].SetActive(true);
        if(1<=starCount)
                stars[0].SetActive(true);
    }

    private void OnEnable()
    {
        if (!firstStart)
        {
            ResultStar(EpisodePlayer.CurrentStar);
            var clear = false;
            if (0<EpisodePlayer.CurrentStar) clear = true;
            SaveData.Instance.AddclearEpisodeList(EpisodePlayer.CurrentEpisode.id,clear,EpisodePlayer.CurrentStar);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
