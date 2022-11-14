using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManage : Singleton<GameManage>
{

    public GameStatusEnum gameStatus = GameStatusEnum.WAIT;
    public PlayerInfoModel playerInfo;

    //  初始化游戏，第一次启动的时候运行
    private void StartGame(int type)
    {
        //  开始新游戏
        if (type == 1)
        {
            this.playerInfo = new PlayerInfoModel();
        }
        else
        {
            //  使用旧数据开始
            //  从文件中加载用户信息
            var service = new PlayerService();
            var infoFromFile = service.load();
            if (infoFromFile != null && infoFromFile.life > 0)
            {
                this.playerInfo = infoFromFile;
            }
            else
            {
                this.playerInfo = new PlayerInfoModel();
            }
        }
        this.gameStatus = GameStatusEnum.PLAY;
    }

    public bool IsRunning()
    {
        //  懒加载初始游戏
        if(this.gameStatus == GameStatusEnum.WAIT)
        {
            var gs = GameSettingConfig.Instance.GetSetting();
            StartGame(gs.startMode);
        }
        return this.gameStatus == GameStatusEnum.PLAY;
    }

    public void SetGameStatus(GameStatusEnum newGameStatus)
    {
        if (newGameStatus == this.gameStatus)
        {
            return;
        }
        //  更新 modal 显示
        EventCenter.Instance.EventTrigger(EventEnum.UPDATE_GAME_STATUS, newGameStatus);
        //  游戏通关
        if (newGameStatus == GameStatusEnum.PASS)
        {
            playerInfo.level += 1;
        }
        //  更新游戏状态
        this.gameStatus = newGameStatus;
    }

}