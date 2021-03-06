﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroPanelController : MonoBehaviour
{

    Button okButton;
    Button cancelButton;
    Text introText;
    Story story;
    GameObject EpisodeSelectPanel;
    void Start()
    {
        EpisodeSelectPanel = GameObject.Find("Canvas").transform.Find("EpisodeSelectPanel").gameObject;
        okButton = GameObject.Find("Canvas/EpisodeSelectPanel/IntroPanel/OkButton").GetComponent<Button>();
        okButton.onClick.AddListener(OnClickOkButton);
        cancelButton = GameObject.Find("Canvas/EpisodeSelectPanel/IntroPanel/CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(OnClickCancelButton);
        introText = GameObject.Find("IntroText").GetComponent<Text>();
        introText.text = FlavorText();
        okButton.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        if (introText != null)
        {
            introText.text = FlavorText();
            okButton.gameObject.SetActive(false); //보고서 선택안했으므로 수락버튼을 끈다.
        }
    }
    string FlavorText()
    {
        return "대충 플레이버 텍스트들어갈 곳\nR:(대충 로봇이 하는말)\n재하:(대충 재하가 하는말)";
    }
    void OnClickOkButton()
    {
        StoryPlayer.SetStory(story);
        SceneManager.LoadScene("GameScene");
    }
    void OnClickCancelButton()
    {
        EpisodeSelectPanel.SetActive(false);
    }
    public void Display(Story story)
    {
        this.introText.text = story.introPassage.text;
        this.okButton.gameObject.SetActive(true);
        this.story = story;
    }
}
