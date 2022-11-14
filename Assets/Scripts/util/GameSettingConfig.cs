using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettingConfig : Singleton<GameSettingConfig>
{
    private GameSetting setting;

    public void SaveSetting(GameSetting setting)
    {
        Debug.Log($"SaveSetting, setting={setting}");
        this.setting = setting;
    }

    public GameSetting GetSetting()
    {
        return this.setting;
    }

}
