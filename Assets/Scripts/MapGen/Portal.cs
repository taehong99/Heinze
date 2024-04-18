using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal destination;
    public Direction direction;
    [SerializeField] Transform spawnPos;
    ScreenFader fader;

    private void Start()
    {
        fader = Manager.UI.Fader;
    }

    private void OnTriggerEnter(Collider other)
    {
        // teleport player to receiver portal
        if (!other.CompareTag("Player"))
            return;

        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.openDoorSFX);
        StartCoroutine(RoomTransitionRoutine(other));
    }

    private IEnumerator RoomTransitionRoutine(Collider other) {
        // Disable player movement
        CharacterController cc = other.GetComponent<CharacterController>();
        cc.enabled = false;

        // Screen fade to black
        fader.FadeOut();
        destination.GetComponentInParent<Room>(true).EnterRoom();
        yield return new WaitForSeconds(fader.FadeDuration);

        // Move Minimap Camera
        Manager.Event.dirEventDic["movedRoom"].RaiseEvent(direction);

        // Teleport Player
        other.transform.position = destination.spawnPos.position;
        
        // Screen fade back to white
        fader.FadeIn();

        // Enable player movement
        cc.enabled = true;
    }

}
