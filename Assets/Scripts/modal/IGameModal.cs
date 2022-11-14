using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class IGameModal : MonoBehaviour {
    
    public abstract GameStatusEnum GetGameStatus();
    
}