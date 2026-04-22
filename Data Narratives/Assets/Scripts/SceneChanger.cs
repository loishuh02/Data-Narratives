using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    public AudioSource audioSource1;    // background music
    public float musicFadeSpeed = 1f;
    public float musicFadeTime = 2f;

    private bool fadeOutMusic = false;

    void Update() {
        if (fadeOutMusic && audioSource1 != null) {
            audioSource1.volume -= Time.deltaTime * musicFadeSpeed;
            if (audioSource1.volume <= 0f) audioSource1.Stop();
        }
    }

    void OnMouseDown() {
        StartCoroutine(WaitToStartGame());
    }

    private IEnumerator WaitToStartGame() {
        fadeOutMusic = true;
        yield return new WaitForSeconds(musicFadeTime);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone) { yield return null; }
    }
}