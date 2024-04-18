using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    [SerializeField] float minOrthoSize;
    [SerializeField] float maxOrthoSize;
    [SerializeField] float zoomSensitivity;
    private float zoomScroll;


    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(FindPlayerRoutine());
    }

    IEnumerator FindPlayerRoutine()
    {
        while(GameObject.FindGameObjectsWithTag("Player").Length < 1)
        {
            yield return null;
        }

        vcam.Follow = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        vcam.m_Lens.OrthographicSize = Mathf.Clamp(vcam.m_Lens.OrthographicSize + -zoomScroll * zoomSensitivity, minOrthoSize, maxOrthoSize);
    }

    private void OnZoom(InputValue value)
    {
        zoomScroll = value.Get<Vector2>().y;
    }
}
