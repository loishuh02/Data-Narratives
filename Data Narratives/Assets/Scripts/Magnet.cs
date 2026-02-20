using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private Vector3 initialPosition; //initial position for each magnet
    
    // Start is called before the first frame update
    void Start()
    {
        //set magnet's initialPosition to current position
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if isTriggered is true, stay in note position
        //if isTriggered is false, move back the magnet to its initial position
    }
}
