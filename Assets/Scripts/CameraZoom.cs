using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

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
