using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BgControl : IGameReset {
    
    public float Speed = 1f;
    private List<Vector3> defaultPositions = new List<Vector3>();

    void Awake() {
        //  注册事件监听
        EventCenter.GetInstance().AddEventListener2(EventEnum.GAME_RESET, Reset);
        //  记录原始坐标
        int cc = transform.childCount;
        for(int i=0;i<cc;i++) {
            GameObject child = transform.GetChild(i).gameObject;
            Vector3 pos = child.transform.position;
            defaultPositions.Add(pos);
        }
    }

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if(!GameManage.INSTANCE.IsRunning()) {
            return;
        }
        foreach (Transform tran in transform) {
            Vector3 objPos = tran.position;
            //  移动速度
            objPos.x = objPos.x - Speed * Time.deltaTime * (1.0f + GameManage.INSTANCE.playerInfo.level / 5.0f);
            if(objPos.x < -7.2f) {
                objPos.x += 7.2f * defaultPositions.Count;
            }
            tran.position = objPos;
        }
    }

    //  重置背景图片坐标
    public override void Reset() {
        int cc = transform.childCount;
        for(int i=0;i<cc;i++) {
            GameObject child = transform.GetChild(i).gameObject;
            child.transform.position = defaultPositions[i];
        }
    }

}
