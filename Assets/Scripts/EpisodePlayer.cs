﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpisodePlayer
{
    private Episode episode;
    private Page currentPage;

    private static readonly EpisodePlayer instance = new EpisodePlayer();
    static EpisodePlayer() {}
    private EpisodePlayer() {}
    public static EpisodePlayer Instance { get => instance; }

    public static bool isReady { get => Instance.episode != null; }

    public static Episode CurrentEpisode { get => Instance.episode; }
    public static Page CurrentPage { get => Instance.currentPage; }

    public static void SetEpisode(Episode episode)
    {
        Instance.episode = episode;
        Instance.currentPage = episode.startPage;
    }
}
