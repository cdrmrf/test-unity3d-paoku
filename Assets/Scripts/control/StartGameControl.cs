using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartGameControl : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    //  开始游戏
    public void StartGame() {
        //  更新 modal 显示
        EventCenter.GetInstance().EventTrigger(EventEnum.UPDATE_GAME_STATUS, GameStatusEnum.PLAY);
        //  重置 角色，背景，土地
        EventCenter.GetInstance().EventTrigger2(EventEnum.GAME_RESET);

        //  倒计时 3秒，开始游戏
        UnityAction cb = () => GameManage.INSTANCE.SetGameStatus(GameStatusEnum.PLAY);
        EventCenter.GetInstance().EventTrigger<UnityAction>(EventEnum.COUNT_DOWN, cb);
    }

}
