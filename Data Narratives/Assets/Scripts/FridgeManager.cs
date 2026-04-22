using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FridgeManager : MonoBehaviour
{
    public static FridgeManager Instance;

    public GameObject doorOutside;
    public GameObject doorInside;

    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioSource doorSound;

    public GameObject foodContainer;
    public GameObject magnetContainer;
    public GameObject doorHandle;

    public TMP_Text ghiText;

    public GameObject confirmUI;
    public string endSceneName = "EndScene";

    [Header("Backgrounds")]
    public string[] countryIDs = { "Hungary", "Peru", "Jordan", "India", "Somalia" };
    public GameObject[] countryBackgrounds; // one per country, same order as countryIDs
    public GameObject defaultBackground;    // shown only on very first scene load

    [Header("Flags")]
    // Tag every flag child object with this tag in the Unity Inspector
    public string flagTag = "Flag";

    [Header("Fridge Open Delay")]
    public float openDelay = 2f;
    [Range(0f, 1f)]
    public float backgroundTiming = 0.8f;

    public bool isDoorOpen = false;
    private string currentCountry = "";
    private GameObject activeBackground = null; // tracks which background is currently shown

    void Start() {
        if (Instance == null) Instance = this;
        if (confirmUI != null) confirmUI.SetActive(false);

        // First load: show default background, all flags hidden
        if (string.IsNullOrEmpty(currentCountry)) {
            SetAllBackgroundsActive(false);
            if (defaultBackground != null) {
                defaultBackground.SetActive(true);
                activeBackground = defaultBackground;
                SetFlagsInBackground(defaultBackground, false); // Hungary flag starts hidden
            }
        }
    }

    public void PrepareAndOpenFridge(string countryName) {
        StartCoroutine(OpenFridgeRoutine(countryName));
    }

    private IEnumerator OpenFridgeRoutine(string countryName) {
        yield return new WaitForSeconds(openDelay * backgroundTiming);

        SwapBackground(countryName);

        yield return new WaitForSeconds(openDelay * (1f - backgroundTiming));

        OpenFridge(countryName);
    }

    private void SwapBackground(string countryName) {
        // Hide flags on whichever background is currently showing before we swap
        if (activeBackground != null)
            SetFlagsInBackground(activeBackground, false);

        SetAllBackgroundsActive(false);

        for (int i = 0; i < countryIDs.Length; i++) {
            if (countryIDs[i] == countryName) {
                if (i < countryBackgrounds.Length && countryBackgrounds[i] != null) {
                    countryBackgrounds[i].SetActive(true);
                    activeBackground = countryBackgrounds[i];
                }
                return;
            }
        }

        // Fallback
        if (defaultBackground != null) {
            defaultBackground.SetActive(true);
            activeBackground = defaultBackground;
        }
    }

    public void OpenFridge(string countryName) {
        isDoorOpen = true;
        currentCountry = countryName;

        doorOutside.SetActive(false);
        doorInside.SetActive(true);

        doorSound.PlayOneShot(doorOpen);

        if (DataParser.Instance.countries.ContainsKey(countryName)) {
            float score = DataParser.Instance.countries[countryName];
            ghiText.text = score.ToString("F1");
            ghiText.gameObject.SetActive(true);
        }

        magnetContainer.SetActive(false);
        foodContainer.SetActive(true);
        foreach (Transform child in foodContainer.transform) {
            child.gameObject.SetActive(child.name == countryName);
        }

        if (doorHandle != null) doorHandle.SetActive(true);
        if (confirmUI != null) confirmUI.SetActive(true);

        // Show flags on the now-active background
        if (activeBackground != null)
            SetFlagsInBackground(activeBackground, true);
    }

    public void CloseFridge() {
        isDoorOpen = false;

        doorOutside.SetActive(true);
        doorInside.SetActive(false);

        doorSound.PlayOneShot(doorClose);

        ghiText.gameObject.SetActive(false);

        magnetContainer.SetActive(true);
        foodContainer.SetActive(false);
        if (doorHandle != null) doorHandle.SetActive(false);
        if (confirmUI != null) confirmUI.SetActive(false);

        // Hide flags — background itself stays visible
        if (activeBackground != null)
            SetFlagsInBackground(activeBackground, false);

        Magnet[] magnets = magnetContainer.GetComponentsInChildren<Magnet>(true);
        foreach (Magnet m in magnets) m.ResetPosition();
    }

    public void ConfirmMenu() {
        if (string.IsNullOrEmpty(currentCountry)) return;

        PlayerPrefs.SetString("ChosenCountry", currentCountry);
        PlayerPrefs.Save();

        SceneManager.LoadScene(endSceneName);
    }

    // Finds all children with flagTag inside a background and toggles them
    private void SetFlagsInBackground(GameObject background, bool state) {
        foreach (Transform child in background.transform) {
            if (child.CompareTag(flagTag))
                child.gameObject.SetActive(state);
        }
    }

    private void SetAllBackgroundsActive(bool state) {
        foreach (var bg in countryBackgrounds)
            if (bg != null) bg.SetActive(state);

        if (defaultBackground != null) defaultBackground.SetActive(state);
    }
}