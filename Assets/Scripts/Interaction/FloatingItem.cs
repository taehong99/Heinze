using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
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

        // ť�갡 ��ǥ ��ġ�� �����ϸ�, ���� ��ġ�� ��ǥ ��ġ�� ��ü�Ͽ� �ݺ��Ѵ�.
        if(transform.position == targetPos)
        {
            Vector3 temp = startPos;
            startPos = targetPos;
            targetPos = temp;
        }

        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
