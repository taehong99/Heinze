using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToTarget : MonoBehaviour
{
    public float lerpDuration;
    public float lerpDistance;
    public float lerpSpeed;

    private void OnEnable()
    {
        StartCoroutine(MoveRoutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveRoutine()
    {
        Vector3 targetPos = transform.forward * lerpDistance;

        float t = 0;
        while (t < lerpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);
            yield return null;
        }
    }
}
