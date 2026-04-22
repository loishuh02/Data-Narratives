using UnityEngine;

public class Magnet : MonoBehaviour
{
    [Header("Cursors")]
    public Texture2D defaultHand;
    public Texture2D openHand;
    public Texture2D grabbedHand;
    public Vector2 hotSpot = Vector2.zero;

    [Header("Snap")]
    // Assign a child or separate empty GameObject in the Inspector as the snap target.
    // If left null, the magnet snaps to the sensor's position instead.
    public Transform snapPoint;

    public string countryID;

    private Vector3 offset = new Vector3(0.4f, 0.4f, 0);
    private Vector3 initialPosition;
    private bool isDragging = false;
    private bool isPlaced = false; // locked after snapping

    void Start() {
        initialPosition = transform.position;
        SetCursor(defaultHand);
    }

    void OnMouseEnter() {
        if (FridgeManager.Instance == null) return;
        if (isPlaced) return; // no cursor change once placed
        if (!isDragging && !FridgeManager.Instance.isDoorOpen) { SetCursor(openHand); }
    }

    void OnMouseExit() {
        if (isPlaced) return;
        if (!isDragging) { SetCursor(defaultHand); }
    }

    void OnMouseDown() {
        if (isPlaced) return;                           // ignore clicks once placed
        if (FridgeManager.Instance.isDoorOpen) return;
        isDragging = true;
        SetCursor(grabbedHand);
    }

    void OnMouseDrag() {
        if (isPlaced) return;
        if (FridgeManager.Instance.isDoorOpen) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x + offset.x, mousePos.y + offset.y, -1f);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("FridgeSensor") && isDragging) {
            isDragging = false;
            isPlaced = true;
            SetCursor(defaultHand);

            // Snap to the defined snapPoint, or fall back to the sensor's position
            Vector3 target = snapPoint != null
                ? new Vector3(snapPoint.position.x, snapPoint.position.y, -1f)
                : new Vector3(other.transform.position.x, other.transform.position.y, -1f);

            transform.position = target;

            FridgeManager.Instance.PrepareAndOpenFridge(countryID);
        }
    }

    void OnMouseUp() {
        if (!isDragging) return;
        isDragging = false;
        SetCursor(defaultHand);
        ResetPosition();
    }

    public void ResetPosition() {
        isPlaced = false; // allow dragging again after fridge closes
        gameObject.SetActive(true);
        transform.position = initialPosition;
        SetCursor(defaultHand);
    }

    private void SetCursor(Texture2D texture) {
        Cursor.SetCursor(texture, hotSpot, CursorMode.Auto);
    }
}