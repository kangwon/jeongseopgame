using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Linq;
using System.IO;

class CheckStory
{
    private bool isClear;
    private int star;
    public CheckStory(bool isClear, int star)
    {
        this.isClear = isClear;
        this.star = star;
    }
    public bool IsClear { get => isClear; }
    public int Star { get => star; }
}

public class SaveData
{
    static string SAVE_PATH = Application.persistentDataPath + "/PlayerSave.json";

    Dictionary<string, CheckStory> clearedStoryDict = new Dictionary<string, CheckStory> { };
    private static readonly SaveData instance = new SaveData();
    public static SaveData Instance { get => instance; }
    public List<string> ClearedStoryList { get => clearedStoryDict.Keys.ToList(); }
    public List<string> Star2Or3EpisodeList //별이 2,3개인 에피소드는 의뢰에 뜨지않게 하기 위한 리스트
    {
        get
        {
            List<string> star2Or3EpisodeList = new List<string> { };
            foreach(var temp in clearedStoryDict)
            {
                if(temp.Value.Star ==2 || temp.Value.Star == 3)
                {
                    star2Or3EpisodeList.Add(temp.Key);
                }
            }
            return star2Or3EpisodeList;
        }
    }
    public int TotalClearStar //클리어한 전체 별 수를 리턴
    {
        get
        {
            int sum = 0;
            foreach (CheckStory star in clearedStoryDict.Values)
            {
                sum += star.Star;
            }
            return sum;
        }
    }
    public static int StoryClearStar(Story story)
    {
        if (Instance.clearedStoryDict.ContainsKey(story.name)) //해당 에피소드 클리어 전적이 있다면
        {
            return Instance.clearedStoryDict[story.name].Star;
        }
        else
        {
            return 0; // 없는 경우 0 리턴
        }
    }
    public static void Save()
    {
        var playerJson = new JSONObject();
        var storyList = new JSONArray();
        foreach(var item in Instance.clearedStoryDict)
        {
            var story = new JSONObject();
            story.Add("id", item.Key);
            story.Add("clear", item.Value.IsClear);
            story.Add("star", item.Value.Star);
            storyList.Add(story);
        }
        playerJson.Add("ClearedStoryList", storyList);
        Debug.Log(playerJson.ToString());
        File.WriteAllText(SAVE_PATH, playerJson.ToString());
    }
    public static void Load()
    {
        string jsonString = File.ReadAllText(SAVE_PATH);
        JSONObject playerJson = (JSONObject)JSON.Parse(jsonString);
        Instance.clearedStoryDict.Clear();
        for (int i = 0; i < playerJson["ClearedStoryList"].AsArray.Count; i++)
        {
            string id = playerJson["ClearedStoryList"].AsArray[i]["id"];
            bool clear = playerJson["ClearedStoryList"].AsArray[i]["clear"];
            int star = playerJson["ClearedStoryList"].AsArray[i]["star"];
            Instance.AddclearEpisodeList(id, clear, star);
        }
        Debug.Log(playerJson.ToString());
    }

    public void AddclearEpisodeList(string id, bool clear, int star)
    {
        if (clearedStoryDict.ContainsKey(id)) //해당키가 있는 경우
        {
            if (clearedStoryDict[id].Star < star)//이미 깼던 에피소드보다 별이 높을 경우
            {
                clearedStoryDict[id] = new CheckStory(clear, star);
            }
        }
        else
        {
            clearedStoryDict[id] = new CheckStory(clear, star);
        }
    }

    public static bool HasCleared(string id)
    {
        return Instance.clearedStoryDict.ContainsKey(id) && Instance.clearedStoryDict[id].IsClear;
    }
}
