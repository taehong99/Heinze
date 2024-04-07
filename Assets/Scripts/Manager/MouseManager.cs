using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    [SerializeField] RectTransform[] slotBars;

    public bool Left
    {
        get;
        private set;
    }
    public bool Right
    {
        get; 
        private set;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            bool isTouch = false;
            for (int i = 0; i < slotBars.Length; i++)
            {
                isTouch |= RectTransformUtility.RectangleContainsScreenPoint(slotBars[i], Input.mousePosition);
                if (isTouch)
                {
                    Left = false;
                    return;
                }
            }
            Left = true;
        }
    }
    private void LateUpdate()
    {
        Left = false;
    }

    // 말하는 감자 왔다감
}
