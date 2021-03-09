using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StoryPlayer
{
    private Story story;
    private Passage currentPassage;
    private Dictionary<string, object> variables = new Dictionary<string, object>();
    
    private static readonly StoryPlayer instance = new StoryPlayer();
    static StoryPlayer() {}
    private StoryPlayer() {}
    
    public static StoryPlayer Instance { get => instance; }
    public static int CurrentStar { get => Instance.GetVariable<int>("STAR", 0); }
    public static bool isReady { get => Instance.story != null; }
    public static Story CurrentStory { get => Instance.story; }
    public static Passage CurrentPassage 
    { 
        get => Instance.currentPassage; 
        set
        {
            var processor = PassageProcessor.ProcessPassage(value, Instance.variables);
            Instance.currentPassage = processor.passage;
            Instance.variables = processor.variables;
        } 
    }

    private T GetVariable<T>(string key, object _default)
    {
        object value;
        if(variables.TryGetValue(key, out value))
            return (T)value;
        else
            return (T)_default;
    }
    
    public static void SetStory(Story story)
    {
        Instance.story = story;
        CurrentPassage = story.GetStartPassage();
    }
    
    public static void SelectLink(Link link)
    {
        CurrentPassage = Instance.story.GetPassage(link.pid);
    }
}
