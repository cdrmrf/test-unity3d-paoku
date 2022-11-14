using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettingConfig
{

    public static GameSettingConfig INSTANCE = new GameSettingConfig();

    private GameSetting setting;

    private GameSettingConfig() { }

    public void SaveSetting(GameSetting setting)
    {
        this.setting = setting;
    }

    public GameSetting GetSetting()
    {
        return this.setting;
    }



}
