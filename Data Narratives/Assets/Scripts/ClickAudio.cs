using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClickAudio : MonoBehaviour
{
    public AudioClip clickClip;

    private AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;       // never loops
    }

    void OnMouseDown() {
        if (clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }
}
