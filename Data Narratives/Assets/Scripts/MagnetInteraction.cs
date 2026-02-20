using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetInteraction : MonoBehaviour
{
    public static bool isTriggered = false; //if magnet is on the note

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        Magnet magnet = other.gameObject.GetComponent<Magnet>();
        if (magnet != null) {
            isTriggered = true;
            Debug.Log("note triggered by magnet!");
        }
            
        //if collided object is magnet then set isTriggered true, then play door open animation
        //after animation played, set isTriggered false
        //magnet visibility to false until doorclose animation is done??? or z axis rotation of the door so doesn't matter?
    }
}
