using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public partial class StoryPlayer
{
    private Story story;
    private Passage currentPassage;
    
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
            Instance.currentPassage = value;
            Instance.processedPassageText = Instance.ProcessPassageText(value.text);
        } 
    }
    public static string ProcessedPassageText { get => Instance.processedPassageText; }
    
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

public partial class StoryPlayer
{
    private string processedPassageText;
    private Dictionary<string, object> variables = new Dictionary<string, object>();

    private T GetVariable<T>(string key, object _default)
    {
        object value;
        if(variables.TryGetValue(key, out value))
            return (T)value;
        else
            return (T)_default;
    }

    private object ParseType(string symbol)
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

    private string ProcessSetMacro(string originText)
    {
        Regex rx = new Regex(@"\(set: \$(?<key>[a-zA-Z0-9_-]+) to (?<value>.+)\)");
        MatchCollection matches = rx.Matches(originText);
        foreach (Match match in matches)
        {
            GroupCollection groups = match.Groups;
            variables[groups["key"].Value] = ParseType(groups["value"].Value);
        }
        // For debuging
        foreach (var kv in variables)
        {
            Debug.Log($"{kv.Key}: {kv.Value} ({kv.Value.GetType()})");
        }
        string setRemovedText = rx.Replace(originText, "").Trim();
        return setRemovedText;
    }

    private string ProcessVariable(string originText)
    {
        Regex rx = new Regex(@"\$(?<varname>[a-zA-Z0-9_-]+)");
        MatchCollection matches = rx.Matches(originText);
        string varReplacedText = originText;
        foreach (Match match in matches)
        {
            GroupCollection groups = match.Groups;
            var varname = groups["varname"].Value;
            varReplacedText = varReplacedText.Replace($"${varname}", variables[varname].ToString());
        }
        return varReplacedText;
    }

    private string ProcessPassageText(string rawText)
    {
        string setRemovedText = ProcessSetMacro(rawText);
        string varReplacedText = ProcessVariable(setRemovedText);
        return varReplacedText;
    }
}
