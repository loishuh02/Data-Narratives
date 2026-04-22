using UnityEngine;
using UnityEngine.SceneManagement;

// Attach this to any persistent GameObject in your End scene.
// For each country, assign two GameObjects in the Inspector arrays (same order as countryIDs).
public class EndManager : MonoBehaviour
{
    [Header("Country IDs — must match PlayerPrefs key and magnet countryID values")]
    public string[] countryIDs = { "Hungary", "Peru", "Jordan", "India", "Somalia" };

    [Header("Two display objects per country (same order as countryIDs)")]
    public GameObject[] primaryObjects;   // e.g. food plate visuals
    public GameObject[] secondaryObjects; // e.g. nutrition label / info panels

    [Header("Scene to return to")]
    public string interactionSceneName = "Interaction";

    void Start() {
        // Deactivate everything first
        SetAllActive(false);

        // Read which country was confirmed
        string chosen = PlayerPrefs.GetString("ChosenCountry", "");
        if (string.IsNullOrEmpty(chosen)) return;

        // Activate only the matching pair
        for (int i = 0; i < countryIDs.Length; i++) {
            if (countryIDs[i] == chosen) {
                if (i < primaryObjects.Length && primaryObjects[i] != null)
                    primaryObjects[i].SetActive(true);

                if (i < secondaryObjects.Length && secondaryObjects[i] != null)
                    secondaryObjects[i].SetActive(true);

                break;
            }
        }
    }

    private void SetAllActive(bool state) {
        foreach (var obj in primaryObjects)
            if (obj != null) obj.SetActive(state);

        foreach (var obj in secondaryObjects)
            if (obj != null) obj.SetActive(state);
    }
}