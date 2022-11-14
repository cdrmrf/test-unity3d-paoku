using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHinder : IGroundChild
{

    public GameObject[] Hinders;

    public override float AddOnGround(GameObject newGround, float startPostion, float endPosition)
    {
        var newGroundPos = newGround.transform.position;
        //  随机 x 坐标
        float randomX = Random.Range(0, endPosition - startPostion) + 0.8f;

        //  创建一个
        var h = Hinders[Random.Range(0, 2)];
        //  挂到 Ground 的父组件身上
        Transform hinder = Instantiate(h, newGround.transform.parent.transform).transform;
        Vector2 position = hinder.position;
        position.x = startPostion + randomX;
        position.y = newGroundPos.y + 0.8f;
        hinder.position = position;

        //  障碍物的宽度
        var r = hinder.GetComponent<Renderer>();
        var size = r.bounds.size;
        float hinderWidth = size.x;

        hinder.parent = newGround.transform;
        //  不要超出边界
        if (startPostion + randomX + hinderWidth >= endPosition)
        {
            Destroy(hinder.gameObject);
            return 0f;
        } else
        {
            return randomX + hinderWidth;
        }
    }

}
