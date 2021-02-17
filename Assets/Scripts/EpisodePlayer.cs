﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodePlayer
{
    private Episode episode;
    private Page currentPage;
    private int currentStar;
    private static readonly EpisodePlayer instance = new EpisodePlayer();
    static EpisodePlayer() {}
    private EpisodePlayer() {}
    public static EpisodePlayer Instance { get => instance; }
    public static int CurrentStar { get {
            if (Instance.currentStar < 0) return 0;
            else if (Instance.currentStar < 3) return Instance.currentStar;
            else return 3;
        }}
    public static bool isReady { get => Instance.episode != null; }

    public static Episode CurrentEpisode { get => Instance.episode; }
    public static Page CurrentPage { get => Instance.currentPage; }
    public static void StarChange(int index) 
    {
        Instance.currentStar += index;
    }

    public static void SetEpisode(Episode episode)
    {
        Instance.episode = episode;
        Instance.currentPage = episode.startPage;
        Instance.currentStar = 3;
    }
}
