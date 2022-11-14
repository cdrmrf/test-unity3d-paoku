using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 土地上的一些随机元素
/// </summary>
public abstract class IGroundChild : MonoBehaviour
{

    public abstract float AddOnGround(GameObject newGround, float startPostion, float endPosition);

}
