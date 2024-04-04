using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.TextCore.Text;

public class ObjectFader : MonoBehaviour
{
    [SerializeField] PlayerController player;
    float originalOpacity;
    Material material;

    void Start()
    {
        //material = GetComponent<Material>();
        //originalOpacity = material.color.a;
    }

    // Ray Version
    Renderer rend;
    public List<Renderer> prev = new List<Renderer>();
    public List<Renderer> cur = new List<Renderer>();
    RaycastHit[] hits = new RaycastHit[10];
    void Update()
    {
        // Cast ray from player to camera
        float distance = (transform.position - player.transform.position).magnitude * 50;
        Vector3 direction = (transform.position - player.transform.position).normalized;
        int count = Physics.RaycastNonAlloc(player.transform.position + direction, direction, hits, distance);
        Debug.DrawRay(player.transform.position, direction * distance, Color.red);
        // Find all renderers hit by ray and make them transparent
        cur.Clear();
        for(int i = 0; i < count; i++)
        {
            RaycastHit hit = hits[i];
            rend = hit.transform.gameObject.GetComponentInChildren<Renderer>();

            if(rend != null)
            {
                if (prev.Contains(rend))
                {
                    cur.Add(rend);
                    continue;
                }
                //Debug.Log(hit.transform.name);
                Material newMat = rend.material;
                Color faded = newMat.color;
                faded.a = 0.3f;
                newMat.color = faded;
                rend.material = newMat;

                cur.Add(rend);
            }
        }

        // Find all previous renderers not hit this frame and set them back to normal
        foreach (var prevRend in prev)
        {
            if (cur.Contains(prevRend))
                continue;

            Material prevMat = prevRend.material;
            Color c = prevMat.color;
            c.a = 1f;
            prevMat.color = c;
            prevRend.material = prevMat;
        }
        prev = new List<Renderer>(cur);
    }
}
