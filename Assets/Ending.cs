using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 终点
/// </summary>
public class Ending : MonoBehaviour
{
    private TMP_Text span;

    void Awake()
    {
        span = GetComponentInChildren<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //  更新土地上 Text 的位置，跟随 ground
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 sv = span.transform.position;
        sv.x = pos.x;
        span.transform.position = sv;
    }
}
