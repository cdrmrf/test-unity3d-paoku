using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重新开始
/// </summary>
public class RestartGameModal : IGameModal {

    public override GameStatusEnum GetGameStatus() {
        return GameStatusEnum.DEAD;
    }

}

