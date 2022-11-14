using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 土地控制器
/// </summary>
public class GroundControl : MonoBehaviour
{

    public int groundNo = 0;

    //  土地上的一些随机元素
    public IGroundChild[] elements;

    // Start is called before the first frame update
    void Start()
    {
        //  新土地的宽度
        var r = GetComponent<Renderer>();
        Vector3 newGroundSize = r.bounds.size;
        float newGroundWidth = newGroundSize.x;
        //  新土地坐标
        var newGroundTransform = transform;
        var newGroundPosition = newGroundTransform.position;
        //  土地的起始截止范围，不能小于或者超过这个范围
        float eleStartX = newGroundPosition.x - newGroundWidth / 2 + 0.5f;
        float eleEndX = newGroundPosition.x + newGroundWidth / 2 - 0.5f;

        if(groundNo > 2)
        {
            //  随机挑几个元素显示在土地上
            for (int i = 0; i < elements.Length / 2 + 1; i++)
            {
                IGroundChild ele = elements[Random.Range(0, elements.Length)];
                var w = ele.AddOnGround(this.gameObject, eleStartX, eleEndX);
                eleStartX += w;
                if (eleStartX >= eleEndX)
                {
                    break;
                }
            }
        }
    }

}
