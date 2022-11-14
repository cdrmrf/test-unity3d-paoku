using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏通关
/// </summary>
public class PassGameModal : IGameModal {

    public override GameStatusEnum GetGameStatus() {
        return GameStatusEnum.PASS;
    }

}
