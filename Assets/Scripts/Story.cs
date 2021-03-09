using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Link
{
    public string name;
    public string link;
    public string pid;
}

[System.Serializable]
public class Passage
{
    public string pid;
    public string name;
    public string text;
    public List<Link> links;
    public bool isEnd 
    { 
        get => links == null || links.Count == 0; 
    }
}

[System.Serializable]
public class Story : IEquatable<Story>
{
    public string name;
    public string startnode;
    public Passage introPassage;
    public Dictionary<string, object> introVariables;
    public List<Passage> passages;
    private Dictionary<string, Passage> passageDict;

    public bool Equals(Story epi)
    {
        if (epi == null) 
            return false;
        return epi.GetHashCode() == this.GetHashCode();
    }
    public override bool Equals(object obj) => Equals(obj as Story);
    public override int GetHashCode() => name.GetHashCode();

    public static Story Load(string id)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Stories/{id}");
        if(jsonFile == null)
            throw new ArgumentException($"There is no such story; {id}");
    
        Story story = JsonUtility.FromJson<Story>(jsonFile.text);
        story.passageDict = new Dictionary<string, Passage>();
        foreach (Passage passage in story.passages)
        {
            story.passageDict[passage.pid] = passage;
            if (passage.name == "의뢰서")
            {
                var introProcessor = PassageProcessor.Process(passage, new Dictionary<string, object>());
                story.introPassage = introProcessor.passage;
                story.introVariables = introProcessor.variables;
                if (passage.links.Count == 1)
                    story.startnode = passage.links[0].pid;
            }
        }
        return story;
    }

    public Passage GetPassage(string pid) => passageDict[pid];
    public Passage GetStartPassage() => GetPassage(startnode);
}
