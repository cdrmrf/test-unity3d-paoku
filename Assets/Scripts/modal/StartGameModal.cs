using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 开始游戏
/// </summary>
public class StartGameModal : IGameModal {

    public override GameStatusEnum GetGameStatus() {
        return GameStatusEnum.WAIT;
    }

}
