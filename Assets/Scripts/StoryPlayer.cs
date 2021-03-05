using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPlayer
{
    private Story story;
    private Passage currentPassage;
    private int currentStar;
    
    private static readonly StoryPlayer instance = new StoryPlayer();
    static StoryPlayer() {}
    private StoryPlayer() {}
    
    public static StoryPlayer Instance { get => instance; }
    public static int CurrentStar 
    {
         get {
            if (Instance.currentStar < 0) return 0;
            else if (Instance.currentStar < 3) return Instance.currentStar;
            else return 3;
        }
    }
    public static bool isReady { get => Instance.story != null; }
    public static Story CurrentStory { get => Instance.story; }
    public static Passage CurrentPassage { get => Instance.currentPassage; }
    
    public static void SetStory(Story story)
    {
        Instance.story = story;
        Instance.currentPassage = story.GetStartPassage();
        Instance.currentStar = 3;
    }
    
    public static void SelectLink(Link link)
    {
        Instance.currentPassage = Instance.story.GetPassage(link.pid);
    } 
}
