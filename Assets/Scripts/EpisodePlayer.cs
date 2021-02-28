using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodePlayer
{
    private Episode episode;
    private Page currentPage;
    private int currentStar;
    private Dictionary<string, int> pageHistory;
    
    private static readonly EpisodePlayer instance = new EpisodePlayer();
    static EpisodePlayer() {}
    private EpisodePlayer() {}
    
    public static EpisodePlayer Instance { get => instance; }
    public static int CurrentStar 
    {
         get {
            if (Instance.currentStar < 0) return 0;
            else if (Instance.currentStar < 3) return Instance.currentStar;
            else return 3;
        }
    }
    public static bool isReady { get => Instance.episode != null; }
    public static Episode CurrentEpisode { get => Instance.episode; }
    public static Page CurrentPage { get => Instance.currentPage; }
    
    public static void SetEpisode(Episode episode)
    {
        Instance.episode = episode;
        Instance.currentPage = episode.startPage;
        Instance.pageHistory = new Dictionary<string, int> { { Instance.currentPage.id, 1 } };
        Instance.currentStar = 3;
    }
    
    public static void SelectAction(Action action)
    {
        Instance.currentStar += action.starChange;
        Instance.currentPage = Instance.episode.pages[action.linkedPageId];
        
        if (Instance.pageHistory.TryGetValue(Instance.currentPage.id, out int value))
            Instance.pageHistory[Instance.currentPage.id] = value + 1;
        else
            Instance.pageHistory[Instance.currentPage.id] = 1;
    }
}
