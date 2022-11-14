using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService {

    private string JSON_FILE = Application.persistentDataPath + "/PlayerInfo.json";

    public void save(PlayerInfoModel model) {
        string json = JsonUtility.ToJson(model);
        System.IO.File.WriteAllText(JSON_FILE, json);
    }

    public PlayerInfoModel load() {
        bool e = System.IO.File.Exists(JSON_FILE);
        if(!e) {
            return null;
        }
        string json = System.IO.File.ReadAllText(JSON_FILE);
        if(json == null || json.Trim().Length == 0) {
            return null;
        }
        var o = JsonUtility.FromJson<PlayerInfoModel>(json);
        return o;
    }

}
