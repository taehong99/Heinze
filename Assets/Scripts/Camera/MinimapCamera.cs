using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MinimapCamera : MonoBehaviour
{
    private void Start()
    {
        Manager.Event.dirEventDic["movedRoom"].OnEventRaised += MoveCamera;
    }

    private void OnDestroy()
    {
        Manager.Event.dirEventDic["movedRoom"].OnEventRaised -= MoveCamera;
    }

    private void MoveCamera(Direction dir)
    {
        float offset = Manager.Game.MapGenerator.RoomOffset;
        switch (dir)
        {
            case Direction.North:
                transform.Translate(Vector3.forward * offset, Space.World);
                break;
            case Direction.South:
                transform.Translate(Vector3.back * offset, Space.World);
                break;
            case Direction.East:
                transform.Translate(Vector3.right * offset, Space.World);
                break;
            default:
                transform.Translate(Vector3.left * offset, Space.World);
                break;
        }
    }
}
