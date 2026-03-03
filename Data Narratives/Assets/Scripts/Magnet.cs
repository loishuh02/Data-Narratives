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
        if (FridgeManager.Instance == null) return;
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
        if (FridgeManager.Instance.isDoorOpen) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, -1f);
    }

    void OnTriggerEnter2D(Collider2D other) { //when magnet dragged to trigger position
        if (other.CompareTag("FridgeSensor") && isDragging) {
            FridgeManager.Instance.OpenFridge(countryID);
            gameObject.SetActive(false);
            SetCursor(defaultHand);
            isDragging = false;
        }
    }

    void OnMouseUp() { 
        if (!isDragging) {return;} //if it wasn't dragged do nothing
        isDragging = false;
        SetCursor(defaultHand);
        ResetPosition();
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
