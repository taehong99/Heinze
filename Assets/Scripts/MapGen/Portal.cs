using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal destination;
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

        StartCoroutine(RoomTransitionRoutine(other));
    }

    private IEnumerator RoomTransitionRoutine(Collider other) {
        // Disable player movement
        CharacterController cc = other.GetComponent<CharacterController>();
        cc.enabled = false;

        // Screen fade to black
        fader.FadeOut();
        destination.GetComponentInParent<Room>().EnterRoom();
        yield return new WaitForSeconds(fader.FadeDuration);

        // Teleport Player
        other.transform.position = destination.spawnPos.position;
        
        // Screen fade back to white
        fader.FadeIn();

        // Enable player movement
        cc.enabled = true;
    }

}
