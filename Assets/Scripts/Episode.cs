using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Debug=UnityEngine.Debug;

[System.Serializable]
class MultiLineText
{
    public List<string> lines;
    public override string ToString() => String.Join("\n", lines);
}

[System.Serializable]
class Action
{
    public string title;
    public string linkedPageId;
    public int starChange;
}

[System.Serializable]
class Page
{
    public string id;
    public MultiLineText body;
    public List<Action> actions;
    public bool isEnd;
}

[System.Serializable]
class Episode
{
    public string id;
    public string title;
    public MultiLineText intro;
    public Dictionary<string, Page> pages;
    public string startPageId;

    public Page startPage { get => pages[startPageId]; }
}
