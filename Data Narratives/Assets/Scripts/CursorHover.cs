using UnityEngine;

// Attach to any GameObject with a collider on Title or End scenes.
// Swap the same cursor textures you use in the Interaction scene.
// Set the default cursor globally via one CursorHover with setGlobalDefault = true,
// then attach it (without that flag) to each interactable object.
public class CursorHover : MonoBehaviour
{
    [Header("Cursor Textures")]
    public Texture2D defaultCursor;
    public Texture2D openHandCursor;
    public Vector2 hotSpot = Vector2.zero;

    [Header("Global Default")]
    // Enable this on ONE object per scene (e.g. a manager/canvas root, no collider needed).
    // It sets the default cursor when the scene loads so every non-interactable area
    // shows the default hand without needing a collider to trigger it.
    public bool setGlobalDefault = false;

    void Start() {
        if (setGlobalDefault)
            Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
    }

    void OnMouseEnter() {
        Cursor.SetCursor(openHandCursor, hotSpot, CursorMode.Auto);
    }

    void OnMouseExit() {
        Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
    }
}
