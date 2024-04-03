using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconData : MonoBehaviour
{
    Object[] objects;

    public void Awake()
    {
        objects = new Object[5];
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = new Object();
        }
    }
    public void SwapSlot(int idx1, int idx2)
    {
        int maxNum = Mathf.Max(idx1, idx2);
        if (maxNum >= objects.Length)
            return;
        if (idx1 == idx2)
            return;


    }
}
