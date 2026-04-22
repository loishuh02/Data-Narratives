using UnityEngine;

// Attach to your door handle GameObject.
// Assign a child "glow" sprite (same sprite, additive material, or a soft glow texture)
// to the glowRenderer field. The original handle sprite is never touched.
public class HandleGlow : MonoBehaviour
{
    [Header("Glow Overlay")]
    // Create a child GameObject on the handle, add a SpriteRenderer to it,
    // assign your glow sprite (can be the same sprite or a soft bloom texture),
    // set its Order In Layer one above the handle, then drag it here.
    public SpriteRenderer glowRenderer;

    [Header("Blink Settings")]
    public float speed = 3f;
    [Range(0f, 1f)]
    public float minAlpha = 0f;   // fully invisible at the dim end
    [Range(0f, 1f)]
    public float maxAlpha = 0.8f; // how opaque the glow gets at its peak

    void Start() {
        if (glowRenderer != null)
            SetAlpha(0f); // start invisible
    }

    void Update() {
        if (FridgeManager.Instance == null || glowRenderer == null) return;

        if (FridgeManager.Instance.isDoorOpen) {
            float t = Mathf.PingPong(Time.time * speed, 1f);
            SetAlpha(Mathf.Lerp(minAlpha, maxAlpha, t));
        } else {
            SetAlpha(0f); // hide glow when door is closed
        }
    }

    private void SetAlpha(float alpha) {
        Color c = glowRenderer.color;
        c.a = alpha;
        glowRenderer.color = c;
    }
}