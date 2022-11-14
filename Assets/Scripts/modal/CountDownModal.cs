using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 倒计时复活弹窗
/// </summary>
public class CountDownModal : MonoBehaviour
{

    private TMP_Text span;
    private int totalTime = 3;
    private float intervaleTime = 1;
    private UnityAction callback;

    void Awake()
    {
        //  注册事件监听
        EventCenter.Instance.AddEventListener<UnityAction>(EventEnum.COUNT_DOWN, StartCountDown);
        span = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        if (totalTime <= 0)
        {
            return;
        }
        intervaleTime -= Time.deltaTime;
        if (intervaleTime <= 0)
        {
            intervaleTime += 1;
            totalTime--;
            span.text = $"{totalTime}";
            if (totalTime <= 0)
            {
                //  隐藏倒计时，开始游戏
                gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
                if(callback != null)
                {
                    callback();
                    this.callback = null;
                }
            }
        }
    }

    private void StartCountDown(UnityAction cb)
    {
        //  显示倒计时
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        totalTime = 3;
        intervaleTime = 1;
        span.text = $"{totalTime}";
        this.callback = cb;
    }

}
