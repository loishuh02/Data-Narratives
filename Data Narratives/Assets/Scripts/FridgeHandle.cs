using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeHandle : MonoBehaviour
{
    public Texture2D openHand;
    public Texture2D defaultHand;
    public Vector2 hotSpot = Vector2.zero;

    void OnMouseEnter() {
        if (FridgeManager.Instance.isDoorOpen) {
            Cursor.SetCursor(openHand, hotSpot, CursorMode.Auto);
        }
    }

    void OnMouseExit() {
        Cursor.SetCursor(defaultHand, hotSpot, CursorMode.Auto);
    }
    
    void OnMouseDown() {
        if (FridgeManager.Instance.isDoorOpen) {
            Cursor.SetCursor(defaultHand, hotSpot, CursorMode.Auto);
            FridgeManager.Instance.CloseFridge();
        }
    }
}
