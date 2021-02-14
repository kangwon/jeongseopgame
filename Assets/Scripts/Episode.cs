using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MultiLineText
{
    public List<string> lines;
    public override string ToString() => String.Join("\n", lines);
}

[System.Serializable]
public class Action
{
    public string title;
    public string linkedPageId;
    public int starChange;
}

[System.Serializable]
public class Page
{
    public string id;
    public MultiLineText body;
    public List<Action> actions;
    public bool isEnd;
}

[System.Serializable]
public class Episode
{
    public string id;
    public string title;
    public MultiLineText intro;
    public Dictionary<string, Page> pages;
    public string startPageId;

    public Page startPage { get => pages[startPageId]; }

    public static Episode Load(string id)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Episodes/{id}");
        if(jsonFile == null)
            throw new ArgumentException($"There is no such episode; {id}");

        Episode episode = JsonUtility.FromJson<Episode>(jsonFile.text);
        return episode;
    }
}
