using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCoin : IGroundChild
{

    public GameObject[] StarCoins;
    private Dictionary<string, float> coinWidthMap = new Dictionary<string, float>();

    public override float AddOnGround(GameObject newGround, float startPostion, float endPosition)
    {   
        return RandomStar(newGround, newGround.transform.position, startPostion, endPosition);
    }

    private float RandomStar(GameObject newGround, Vector2 newGroundPos, float minPositionX, float maxPositionX)
    {
        int rand = Random.Range(1, 100) % 3;
        //  随机一个奖励的星星
        var starCoin = StarCoins[Random.Range(0, StarCoins.Length)];

        //  获取宽度
        if(!coinWidthMap.ContainsKey(starCoin.name))
        {
            var r = starCoin.gameObject.GetComponent<Renderer>();
            var newGroundSize = r.bounds.size;
            var starWidth = newGroundSize.x;
            coinWidthMap.Add(starCoin.name, starWidth);
        }

        if (rand == 1)
        {
            //  一个星星
            return RandomStar1(newGround, starCoin, newGroundPos, minPositionX, maxPositionX);
        }
        else if (rand == 2)
        {
            //  四个星星，一排
            return RandomStar2(newGround, starCoin, newGroundPos, minPositionX, maxPositionX);
        }
        else
        {
            //  ^ 形奖励
            return RandomStar3(newGround, starCoin, newGroundPos, minPositionX, maxPositionX);
        }
    }

    private float RandomStar1(GameObject newGround, GameObject starCoin, Vector2 newGroundPos, float minPositionX, float maxPositionX)
    {
        float starWidth = coinWidthMap[starCoin.name];
        float endPosition = 0f;
        //  不要超出边界
        if (minPositionX + starWidth >= maxPositionX)
        {
            return endPosition;
        }

        //  挂到 Ground 的父组件身上
        Transform star = Instantiate(starCoin, newGround.transform.parent.transform).transform;
        //  重新定位
        Vector2 postion = star.position;
        postion.x = minPositionX;
        postion.y = newGroundPos.y + 1f;
        star.position = postion;
        endPosition = starWidth;
        star.parent = newGround.transform;
        //  防止超出边界
        if (minPositionX + starWidth >= maxPositionX)
        {
            Destroy(star.gameObject);
        }
        return endPosition;
    }

    private float RandomStar2(GameObject newGround, GameObject starCoin, Vector2 newGroundPos, float minPositionX, float maxPositionX)
    {
        float starWidth = coinWidthMap[starCoin.name];
        float endPosition = 0f;
        for (int i = 0; i < 4; i++)
        {
            //  不要超出边界
            if (minPositionX + endPosition + starWidth >= maxPositionX)
            {
                return endPosition;
            }
            //  挂到 Ground 的父组件身上
            Transform star = Instantiate(starCoin, newGround.transform.parent.transform).transform;
            Vector2 postion = star.position;
            postion.x = minPositionX;
            postion.y = newGroundPos.y + 1f;
            star.position = postion;
            endPosition += starWidth;
            star.parent = newGround.transform;
        }
        return endPosition;
    }

    private float RandomStar3(GameObject newGround, GameObject starCoin, Vector2 newGroundPos, float minPositionX, float maxPositionX)
    {
        float starWidth = coinWidthMap[starCoin.name];
        float endPosition = 0f;
        //  ^ 形奖励
        float endPosY = 0f;
        for (int i = 0; i < 5; i++)
        {
            //  不要超出边界
            if (minPositionX + endPosition + starWidth >= maxPositionX)
            {
                return endPosition;
            }
            //  挂到 Ground 的父组件身上
            Transform star = Instantiate(starCoin, newGround.transform.parent.transform).transform;
            Vector2 postion = star.position;
            postion.x = minPositionX + endPosition;
            postion.y = newGroundPos.y + 1f + endPosY;
            star.position = postion;
            endPosition += starWidth;
            star.parent = newGround.transform;
            if (i >= 2)
            {
                endPosY -= starWidth;
            }
            else
            {
                endPosY += starWidth;
            }
        }
        return endPosition;
    }

}
