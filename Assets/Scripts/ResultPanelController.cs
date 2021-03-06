﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ResultPanelController : MonoBehaviour
{
    GameObject resultPanel;
    Button resultButton;
    GameObject[] stars = new GameObject[3];
    RectTransform rectTransform;
    Text episodeName;
    Text clearText;
    Text characterText;
    bool firstStart =true;
    // Start is called before the first frame update
    void Start()
    {
        resultPanel = GameObject.Find("ResultPanel").gameObject;
        episodeName = GameObject.Find("ResultPanel/EpisodeName").GetComponent<Text>();
        clearText = GameObject.Find("ResultPanel/ClearText").GetComponent<Text>();
        characterText = GameObject.Find("ResultPanel/CharacterText").GetComponent<Text>();
        resultButton = GameObject.Find("ResultPanel/ResultButton").GetComponent<Button>();
        resultButton.onClick.AddListener(OnClickResultButton);
        for (int i = 0; i < 3; i++)
            stars[i] = GameObject.Find($"ResultPanel/Star{i + 1}/").transform.Find("Image").gameObject;
        rectTransform = resultPanel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = GameObject.Find("Canvas/VertialLayout").GetComponent<RectTransform>().anchoredPosition;
        rectTransform.sizeDelta = GameObject.Find("Canvas/VertialLayout").GetComponent<RectTransform>().sizeDelta;
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
            ResultStar(StoryPlayer.CurrentStar);
            episodeName.text ="Ep."+ StoryPlayer.CurrentStory.name;
            if(StoryPlayer.CurrentStory.introVariables.ContainsKey("CHARACTER"))
                characterText.text = (string)StoryPlayer.CurrentStory.introVariables["CHARACTER"];
            bool clear;
            if (0 < StoryPlayer.CurrentStar)
            {
                clear = true;
                clearText.text = "의뢰 성공!";
            }
            else
            {
                clear = false;
                clearText.text = "의뢰 실패...";
            }
            SaveData.Instance.AddClearStory(StoryPlayer.CurrentStory.name, clear, StoryPlayer.CurrentStar);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
