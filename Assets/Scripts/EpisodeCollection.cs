using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class EpisodeCollection
{
    DirectoryInfo dir = new DirectoryInfo(Application.dataPath+"/Scripts/Resources/Episodes");
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
        foreach(var item in dir.GetFiles())
        {
            var temp = item.Name;
            if (!temp.Contains("meta"))
            {
                temp = temp.Substring(0, temp.IndexOf("."));
                episodes.Add(Episode.Load(temp));
                if (temp.Contains("tutorial"))
                    tutorialEpisodes.Add(Episode.Load(temp));
            }
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
