using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {

    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        var prevSpeed = animator.speed;
        if(GameManage.INSTANCE.IsRunning() && prevSpeed <= 0) {
            animator.speed = 1f;
        } else if(!GameManage.INSTANCE.IsRunning() && prevSpeed > 0) {
            animator.speed = 0f;
        }
        if (!GameManage.INSTANCE.IsRunning()) {
            return;
        }
    }

}
