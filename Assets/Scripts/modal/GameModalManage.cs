using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameModalManage : MonoBehaviour
{

    //  游戏状态 -> 弹窗
    Dictionary<GameStatusEnum, IGameModal> gameModalMap = new Dictionary<GameStatusEnum, IGameModal>();

    void Awake() {
        int cc = transform.childCount;
        for(int i=0;i<cc;i++) {
            GameObject child = transform.GetChild(i).gameObject;
            IGameModal gm = child.GetComponent<IGameModal>();
            if(gm != null)
            {
                gameModalMap.Add(gm.GetGameStatus(), gm);
            }
        }
        //  事件订阅，状态改变后更新弹窗的展示
        EventCenter.Instance.RemoveEventListener<GameStatusEnum>(EventEnum.UPDATE_GAME_STATUS, GameStatusChange);
        EventCenter.Instance.AddEventListener<GameStatusEnum>(EventEnum.UPDATE_GAME_STATUS, GameStatusChange);
    }

    public void GameStatusChange(GameStatusEnum newStatus) 
    {
        var keys = gameModalMap.Keys;
        foreach(GameStatusEnum s in keys) 
        {
            if (newStatus == s) 
            {
                gameModalMap[s].gameObject.SetActive(true);
            } else 
            {
                gameModalMap[s].gameObject.SetActive(false);
            }
        }
    }

    
}