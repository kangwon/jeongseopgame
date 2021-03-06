using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StoryPlayer
{
    private Story story;
    private Passage currentPassage;
    private string processedPassageText;
    private Dictionary<string, object> variables = new Dictionary<string, object>();
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
    public static Passage CurrentPassage 
    { 
        get => Instance.currentPassage; 
        set
        {
            var passage = value;
            Instance.currentPassage = passage;

            Regex setRegex = new Regex(@"\(set: \$(?<key>\w+) to (?<value>.+)\)");
            MatchCollection matches = setRegex.Matches(passage.text);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Instance.variables[groups["key"].Value] = ParseType(groups["value"].Value);
            }
            // For debuging
            foreach (var kv in Instance.variables)
            {
                Debug.Log($"{kv.Key}: {kv.Value} ({kv.Value.GetType()})");
            }
            string setRemovedText = setRegex.Replace(passage.text, "").Trim();
            Instance.processedPassageText = setRemovedText;
        } 
    }
    public static string ProcessedPassageText { get => Instance.processedPassageText; }

    public static object GetVariableValue(string varname) => Instance.variables[varname];
    
    public static void SetStory(Story story)
    {
        Instance.story = story;
        CurrentPassage = story.GetStartPassage();
        Instance.currentStar = 3;
    }
    
    public static void SelectLink(Link link)
    {
        CurrentPassage = Instance.story.GetPassage(link.pid);
    }

    private static object ParseType(string symbol)
    {
        Regex stringRegex = new Regex(@"""(?<value>.+)""");
        Regex intRegex = new Regex(@"(?<value>\d+)");
        Regex floatRegex = new Regex(@"(?<value>[0-9.]+)");
        Regex boolRegex = new Regex(@"(?<value>[false|true]+)");
        
        Match stringMatch = stringRegex.Match(symbol);
        if(stringMatch.Success)
            return stringMatch.Groups["value"].Value;

        Match intMatch = intRegex.Match(symbol);
        if(intMatch.Success)
            return int.Parse(intMatch.Groups["value"].Value);

        Match floatMatch = floatRegex.Match(symbol);
        if(floatMatch.Success)
            return float.Parse(floatMatch.Groups["value"].Value);

        Match boolMatch = boolRegex.Match(symbol);
        if(boolMatch.Success)
            return bool.Parse(boolMatch.Groups["value"].Value);
        
        throw new NotImplementedException($"Invalid variable value: {symbol}");
    }
}
