using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;


public class PlayerData
{
    public Dictionary<int,StageData> data;

    public PlayerData()
    {
        data = new Dictionary<int,StageData>();
    }

    #region ConvertJson

    static JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    public static string SerializePartyData(PlayerData data)
    {
        if (data == null) return null;
        return JsonConvert.SerializeObject(data, serializerSettings);
    }
    public static PlayerData DeserializePartyData(string json)
    {
        if (json == null) return null;
        return JsonConvert.DeserializeObject<PlayerData>(json, serializerSettings);
    }

    public void SavePlayerData(string fileName)
    {
        string data = SerializePartyData(this);
        if (!Directory.Exists(Application.dataPath +"/Data"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data");
        }
        string path = Path.Combine(Application.dataPath + "/Data", fileName + ".Json");
        File.WriteAllText(path, data);
        Debug.Log(data);
        Debug.Log("Save");
    }
    public static PlayerData LoadPlayerData(string fileName)
    {
        
        string path = Path.Combine(Application.dataPath + "/Data", fileName + ".Json");
        string data = null;
        if (File.Exists(path))
        {
            data = File.ReadAllText(path);
        }
        if (data == null)
        {
            return new PlayerData();
        }
        return DeserializePartyData(data);
    }

    #endregion
}
public struct StageData
{
    public StageData(int _point = 0, float _time = 0)
    {
        point = _point;
        time = _time;
    }
    [JsonProperty("stage_point", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int point;
    [JsonProperty("stage_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public float time;
}
