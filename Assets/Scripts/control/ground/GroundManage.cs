using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundManage : IGameReset {

    private int groundCounter = 1;

    //  背景初始移动速度
    public float speed = 10f;
    //  土地 prefab
    public GameObject groundModel;
    //  终点 prefab
    public GameObject endingModel;
    //  最后一块土地
    private GameObject lastGround = null;
    //  终点生成后不要继续创建新土地了
    private bool stopNewGround = false;

    void Awake() {
        //  注册事件监听
        EventCenter.GetInstance().AddEventListener2(EventEnum.GAME_RESET, Reset);
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(!GameManage.INSTANCE.IsRunning()) {
            return;
        }

        int counter = 0;
        foreach (Transform tran in transform) {
            //  土地向后方移动
            Vector3 objPos = tran.position;
            objPos.x = objPos.x - speed * Time.deltaTime * (1.0f + GameManage.INSTANCE.playerInfo.level / 5.0f);
            tran.position = objPos;

            //  超出可视区域的都清除
            counter++;
            if (objPos.x < -7.2f) {
                counter--;
                Destroy(tran.gameObject);
            }
        }

        //  土地少于 5个就创建新的土地
        if(counter < 5) {
            CreateNewGround();
        }
        
    }

    //  随机土地，随机 y 坐标，随机宽度
    private void CreateNewGround() {
        if(stopNewGround)
        {
            return;
        }
        bool rand = true;
        var lastGround = this.lastGround?.transform;
        //  minGroundCount，每一关要创建的土地数量
        var p = GameManage.INSTANCE.playerInfo;
        int minGroundCount = p.level * 10;
        GameObject newGround = this.groundCounter >= minGroundCount ? Instantiate(endingModel, transform) : Instantiate(groundModel, transform);
        //  土地达到配置，生成终点，停止创建新土地，终点不添加障碍物，不随机宽度
        stopNewGround = this.groundCounter >= minGroundCount;
        rand = this.groundCounter < minGroundCount;

        newGround.name = $"ground_{this.groundCounter}";
        Transform newTran = newGround.transform;

        //  获取土地的宽度，如果只有一个 土地模型，可以缓存起来
        var r = newGround.GetComponent<Renderer>();
        Vector3 newGroundSize = r.bounds.size;
        float newGroundWidth = newGroundSize.x;

        //  获取土地的脚本，设置属性
        GroundControl script = newGround.GetComponent<GroundControl>();
        if(script != null)
        {
            script.groundNo = this.groundCounter;
        }

        //  土地宽度缩放，第一块是正常土地
        if (this.groundCounter != 1 && rand)
        {
            float newGroundScale = Random.Range(3, 12) / 10f;
            newTran.localScale = new Vector3(newGroundScale, 1f, 1f);
            newGroundWidth *= newGroundScale;
        }

        //  新土地坐标
        Vector2 newGroundPos = newTran.position;
        //  获取最后一块土地，两块土地不要距离太远
        if(lastGround != null && newGroundWidth > 0f) {
            //  最后一块土地，x 坐标
            float lastGroundX = lastGround.position.x;
            var r2 = lastGround.GetComponent<Renderer>();
            //  最后一块土地，宽度
            float lastGroundWidth = r2.bounds.size.x;
            //  新土地的 x 坐标 = 最后一块土地的坐标 + 最后一块土地的宽度 + 随机距离
            newGroundPos.x = lastGroundX + lastGroundWidth / 2 + newGroundWidth / 2 + Random.Range(7, 12) / 10f;
        }
        //  随机高度
        newGroundPos.y = rand ? - 1.5f + (Random.Range(1,14) / 10f) : -1.5f;
        newTran.position = newGroundPos;

        this.lastGround = newGround;
        this.groundCounter++;
    }

    /// <summary>
    /// 通关后重置游戏土地
    /// </summary>
    public override void Reset() {
        this.lastGround = null;
        this.groundCounter = 1;
        this.stopNewGround = false;
        //  删除现有的土地
        foreach (Transform tran in transform)
        {
            Destroy(tran.gameObject);
        }

        ////  创建默认的土地
        CreateNewGround();
        CreateNewGround();
        CreateNewGround();
    }

}
