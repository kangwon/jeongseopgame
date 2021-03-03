using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class StoryCollection
{
    DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Scripts/Resources/Stories");
    List<Story> stories = new List<Story> { };
    List<Story> tutorialStories = new List<Story> { };
    
    private static readonly StoryCollection instance = new StoryCollection();
    static StoryCollection() {}
    private StoryCollection() 
    {
        CollectStories();
    }
    public static StoryCollection Instance { get => instance; }
    private void CollectStories()
    {
        foreach(FileInfo item in dir.GetFiles("*.json"))
        {
            string fileName = item.Name;
            string storyId = fileName.Substring(0, fileName.IndexOf("."));
            stories.Add(Story.Load(storyId));
            if (storyId.Contains("tutorial"))
                tutorialStories.Add(Story.Load(storyId));
        }
    }
    public void PrintStories()
    {
        foreach(var story in stories)
        {
            Debug.Log(story.name);
        }
    }
    public static List<Story> TutorialStories { get => instance.tutorialStories; }
    public static List<Story> NotTutorialStories 
    { 
        get => instance.stories.Except(instance.tutorialStories).ToList(); 
    }
    public static List<Story> AllStory { get => instance.stories; }
}
