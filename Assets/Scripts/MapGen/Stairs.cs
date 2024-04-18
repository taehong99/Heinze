using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField] string sceneName;
    bool triggered;

    private void GoToNextLevel()
    {
        if (triggered)
            return;
        Manager.Scene.LoadScene(sceneName);
        triggered = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.Player.Freeze();
            GoToNextLevel();
        }
    }
}
