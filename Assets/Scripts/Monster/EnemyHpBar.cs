using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectHp;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();

    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        if(screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
            // x, y 좌표밖에 필요가 없다. 근데 반환되는 애는 vector 3이므로, z 값도 반환하게 된다.
        }
    }
}
