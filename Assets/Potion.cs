using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float floatHieght = 1f;
    public float floatSpeed = 1f;
    public float rotateSpeed = 30f;

    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.up * floatHieght;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(startPos, targetPos, Mathf.PingPong(Time.time * floatSpeed, 1f));

        // 큐브가 목표 위치에 도달하면, 시작 위치와 목표 위치를 교체하여 반복한다.
        if(transform.position == targetPos)
        {
            Vector3 temp = startPos;
            startPos = targetPos;
            targetPos = temp;
        }

        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
