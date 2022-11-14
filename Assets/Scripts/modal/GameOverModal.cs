using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏结束
/// </summary>
public class GameOverModal : IGameModal {

    public override GameStatusEnum GetGameStatus() {
        return GameStatusEnum.GAME_OVER;
    }

}