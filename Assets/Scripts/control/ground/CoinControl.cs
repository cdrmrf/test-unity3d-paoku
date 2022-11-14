using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControl : MonoBehaviour {
    
    private GameManage manage = GameManage.Instance;
    //  分数是配置的，有的 +1，有的 -5，有的 +3
    public int score = 1;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D c) {
        var player = manage.playerInfo;
        player.score += this.score;
        AudioSourceControl.Instance.Play("金币");
        Destroy(gameObject);
        if(player.score > 0 && player.score % 100 == 0) {
            player.life += 1;
        }
    }

}
