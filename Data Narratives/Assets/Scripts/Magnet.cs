using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    //cursor states
    public Texture2D defaultHand;
    public Texture2D openHand;
    public Texture2D grabbedHand;
    public Vector2 hotSpot = Vector2.zero;

    public string countryID; //for countries dictionary
    private Vector3 offset = new Vector3 (0.4f, 0.4f, 0); //offset when magnet is grabbed
    private Vector3 initialPosition; //initial position for each magnet
    private bool isDragging = false;
    
    void Start() {
        initialPosition = transform.position; //set magnet's initialPosition to current position
        SetCursor(defaultHand);
    }

    void OnMouseEnter() { //when cursor hovers over magnets
        if (!isDragging && !FridgeManager.Instance.isDoorOpen) {SetCursor(openHand);}
    }

    void OnMouseExit() { //when cursor moves away from magnets
        if (!isDragging) {SetCursor(defaultHand);}
    }

    void OnMouseDown() { //when clicking magnets
        if (FridgeManager.Instance.isDoorOpen) {return;} //if door is open do nothing
        isDragging = true;
        SetCursor(grabbedHand);
    }

    void OnMouseDrag() {
        if (FridgeManager.Instance.isDoorOpen) {return;}
        Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition); //from pixel coodinates to unity screen ones
        mouse_position.z = 0;
        transform.position = mouse_position + offset;
    }

    void OnMouseUp() { //when magnet dragged to trigger position
        if (!isDragging) {return;} //if it wasn't dragged do nothing
        isDragging = false;
        SetCursor(defaultHand);

        //OverlapPoint instead oncollision2d without using rigidbody (no gravity)
        Collider2D hit = Physics2D.OverlapPoint(transform.position);
        if (hit != null && hit.CompareTag("FridgeSensor")) {
            transform.position = hit.transform.position;
            FridgeManager.Instance.OpenFridge(countryID);
            gameObject.SetActive(false);
        } else {transform.position = initialPosition;}
    }

    public void ResetPosition() { //when door closes
        gameObject.SetActive(true);
        transform.position = initialPosition;
        SetCursor(defaultHand);
    }

    private void SetCursor(Texture2D texture) {
        Cursor.SetCursor(texture, hotSpot, CursorMode.Auto);
    }
}
