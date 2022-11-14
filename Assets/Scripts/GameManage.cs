using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManage : MonoBehaviour {

    public static GameManage INSTANCE;

    public GameStatusEnum gameStatus;
    public PlayerInfoModel playerInfo;

    private void Awake() {
        //  加载设定
        var gs = GameSettingConfig.INSTANCE.GetSetting();
        Debug.Log("Init GameManage");
        Debug.Log($"GameStartMode={gs.startMode}");
        if(INSTANCE != null) {
            Destroy(gameObject);
            return;
        }

        //  初始化，直接开始游戏
        INSTANCE = this;
        INSTANCE.gameStatus = GameStatusEnum.PLAY;
        DontDestroyOnLoad(INSTANCE);
    }

    public static void StartGame(int type) {
        //  开始新游戏
        if(type == 1) {
            INSTANCE.playerInfo = new PlayerInfoModel();
        } else {
            //  使用旧数据开始
            //  从文件中加载用户信息
            var service = new PlayerService();
            var infoFromFile = service.load();
            if (infoFromFile != null && infoFromFile.life > 0) {
                INSTANCE.playerInfo = infoFromFile;
            } else {
                INSTANCE.playerInfo = new PlayerInfoModel();
            }
        }
        INSTANCE.gameStatus = GameStatusEnum.PLAY;
    }

    //  通关分数
    public int GetGoal() {
        return 10 * playerInfo.level;
    }

    public bool IsRunning() {
        return this.gameStatus == GameStatusEnum.PLAY;
    }

    public void SetGameStatus(GameStatusEnum newGameStatus) {
        if(newGameStatus == this.gameStatus) {
            return;
        }
        //  更新 modal 显示
        EventCenter.GetInstance().EventTrigger(EventEnum.UPDATE_GAME_STATUS, newGameStatus);
        //  游戏通关
        if(newGameStatus == GameStatusEnum.PASS) {
            playerInfo.level += 1;
        }
        //  更新游戏状态
        this.gameStatus = newGameStatus;
    }
    
}