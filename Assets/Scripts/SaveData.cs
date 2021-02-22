using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Linq;
using System.IO;
class checkEpisode
{
    private bool isClear;
    private int star;
    public checkEpisode(bool isClear, int star)
    {
        this.isClear = isClear;
        this.star = star;
    }
    public bool IsClear { get => isClear; }
    public int Star { get => star; }
}
public class SaveData
{
    Dictionary<string, checkEpisode> clearEpisodeList = new Dictionary<string, checkEpisode> { };
    private static readonly SaveData instance = new SaveData();
    public static SaveData Instance { get => instance; }
    public List<string> ClearEpisodeList { get =>clearEpisodeList.Keys.ToList(); }
    public void Save()
    {
        var playerJson = new JSONObject();
        var episodeList = new JSONArray();
        foreach(var epi in clearEpisodeList)
        {
            var episode = new JSONObject();
            episode.Add("id", epi.Key);
            episode.Add("clear", epi.Value.IsClear);
            episode.Add("star", epi.Value.Star);
            episodeList.Add(episode);
        }
        playerJson.Add("ClearEpisodeList", episodeList);
        Debug.Log(playerJson.ToString());
        string path = Application.persistentDataPath + "/PlayerSave.json"; //데이터 저장
        File.WriteAllText(path, playerJson.ToString());
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/PlayerSave.json"; //데이터 불러오기
        string jsonString = File.ReadAllText(path);
        JSONObject playerJson = (JSONObject)JSON.Parse(jsonString);
        clearEpisodeList.Clear();
        for (int i = 0; i < playerJson["ClearEpisodeList"].AsArray.Count; i++)
        {
            string id = playerJson["ClearEpisodeList"].AsArray[i]["id"];
            bool clear = playerJson["ClearEpisodeList"].AsArray[i]["clear"];
            int star = playerJson["ClearEpisodeList"].AsArray[i]["star"];
            AddclearEpisodeList(id, clear, star);
        }
        Debug.Log(playerJson.ToString());
    }
    public void AddclearEpisodeList(string id,bool clear,int star)
    {
        clearEpisodeList.Add(id, new checkEpisode(clear, star));
    }
}
