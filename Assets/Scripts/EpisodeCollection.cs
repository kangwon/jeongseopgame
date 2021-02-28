using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class EpisodeCollection
{
    DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Scripts/Resources/Episodes");
    List<Episode> episodes = new List<Episode> { };
    List<Episode> tutorialEpisodes = new List<Episode> { };
    private static readonly EpisodeCollection instance = new EpisodeCollection();
    static EpisodeCollection() {}
    private EpisodeCollection() 
    {
        CollectEpisode();
    }
    public static EpisodeCollection Instance { get => instance; }
    private void CollectEpisode()
    {
        foreach(FileInfo item in dir.GetFiles("*.json"))
        {
            string fileName = item.Name;
            string episodeId = fileName.Substring(0, fileName.IndexOf("."));
            episodes.Add(Episode.Load(episodeId));
            if (episodeId.Contains("tutorial"))
                tutorialEpisodes.Add(Episode.Load(episodeId));
        }
    }
    public void PrintEpisode()
    {
        foreach(var item in episodes)
        {
            Debug.Log(item.id);
        }
    }
    public List<Episode> EpisodeTutorial { get => tutorialEpisodes; }
    public List<Episode> EpisodeExceptTutorial { get => episodes.Except(tutorialEpisodes).ToList(); }
    public List<Episode> AllEpisode { get => episodes; }
}
