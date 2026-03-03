using UnityEngine;
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

    public bool isDoorOpen = false;

    void Start() {
        if (Instance == null) Instance = this;
    }

    public void OpenFridge(string countryName) {
        isDoorOpen = true;

        doorOutside.SetActive(false); //door visual
        doorInside.SetActive(true);

        doorSound.PlayOneShot(doorOpen);

        if (DataParser.Instance.countries.ContainsKey(countryName)) {
            float score = DataParser.Instance.countries[countryName];
            ghiText.text = "Global Hunger\nIndex Score:\n" + score.ToString("F1");
            ghiText.gameObject.SetActive(true);
        }

        magnetContainer.SetActive(false);
        foodContainer.SetActive(true); //food visual
        foreach (Transform child in foodContainer.transform) {
            child.gameObject.SetActive(child.name == countryName);
        }

        if(doorHandle != null) doorHandle.SetActive(true);
    }

    public void CloseFridge() {
        isDoorOpen = false;

        doorOutside.SetActive(true);
        doorInside.SetActive(false);

        doorSound.PlayOneShot(doorClose);

        ghiText.gameObject.SetActive(false);

        magnetContainer.SetActive(true);
        foodContainer.SetActive(false);
        if(doorHandle != null) doorHandle.SetActive(false);
        
        Magnet[] magnets = magnetContainer.GetComponentsInChildren<Magnet>(true);
        foreach (Magnet m in magnets) m.ResetPosition();
    }
}