using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 角色死掉后用户选择是否继续游戏
/// </summary>
public class WaitRebornModal : IGameModal
{

    private TMP_Text span;

    void Awake()
    {
        span = GetComponentInChildren<TMP_Text>();
    }

    public override GameStatusEnum GetGameStatus()
    {
        return GameStatusEnum.WAIT_REBORN;
    }

    private void OnEnable()
    {
        span.text = $"继续游戏？\n还有{GameManage.INSTANCE.playerInfo.HP}次机会";
    }

    public void Yes()
    {
        //  REBORN: 重置角色位置
        GameManage.INSTANCE.SetGameStatus(GameStatusEnum.REBORN);
        UnityAction callback = () => GameManage.INSTANCE.SetGameStatus(GameStatusEnum.PLAY);
        //  3秒倒计时，倒计时结束开始游戏
        EventCenter.GetInstance().EventTrigger<UnityAction>(EventEnum.COUNT_DOWN, callback);
        this.gameObject.SetActive(false);
    }

    public void No()
    {
        GameManage.INSTANCE.SetGameStatus(GameStatusEnum.DEAD);
    }

}
