using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoControl : MonoBehaviour {
    
    private TMP_Text span;

    void Start() {
        span = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update() {
        var p = GameManage.Instance?.playerInfo;
        if(p == null) {
            return;
        }
        span.text = $"Level:{p.level} - Life:{p.life} - Score:{p.score} - HP:{p.HP}";
    }

}
