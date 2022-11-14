using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : IGroundChild
{

    public GameObject Enemy;

    public override float AddOnGround(GameObject newGround, float startPostion, float endPosition)
    {
        //  随机敌人的 x 坐标
        float randomX = Random.Range(0, endPosition - startPostion) + 0.8f;
        //  创建一个敌人
        var newGroundPos = newGround.transform.position;
        //  挂到 Ground 的父组件身上
        Transform enemy = Instantiate(Enemy, newGround.transform.parent.transform).transform;
        Vector2 position = enemy.position;
        position.x = startPostion + randomX;
        position.y = newGroundPos.y + 1f;
        enemy.position = position;

        //  敌人动画的宽度
        var r = enemy.GetComponent<Renderer>();
        var size = r.bounds.size;
        float enemyWidth = size.x;

        enemy.parent = newGround.transform;
        //  不要超出边界
        if (startPostion + randomX + enemyWidth >= endPosition)
        {
            Destroy(enemy.gameObject);
            return 0f;
        }
        return randomX + enemyWidth;
    }

}