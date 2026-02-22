using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeManager : MonoBehaviour
{
    public static FridgeManager Instance; //singleton
    public bool isDoorOpen = false;

    public Animator doorAnimator;
    public SpriteRenderer doorRenderer;
    public Sprite doorOutside;
    public Sprite doorInside;

    public GameObject foodContainer; //empty parent containing all food object
    public GameObject magnetContainer; //empty parent containing all magnets
    public GameObject doorHandle; //handle object to click to close

    void Awake() {if (Instance == null) {Instance = this;}}

    public void OpenFridge(string countryName) {
        isDoorOpen = true;
        doorHandle.SetActive(true);

        foreach (Transform child in foodContainer.transform) {
            child.gameObject.SetActive(child.tag == countryName);
        }

        doorAnimator.SetTrigger("Open");
    }

    public void CloseFridge() {
        doorAnimator.SetTrigger("Close");
        StartCoroutine(ResetSequence());
    }

    IEnumerator ResetSequence() { //wait and reset visuals on door
        yield return new WaitForSeconds(0.4f); //door almost closed

        foreach (Transform child in foodContainer.transform) {child.gameObject.SetActive(false);}

        //find all magnets including deactivated ones for foreach
        Magnet[] magnets = magnetContainer.GetComponentsInChildren<Magnet>(true); //"GetComponents" important for saving in array as plural; GetComponent: look for one single script and return it; GetComponents: look for every script of that type in the children and return them asa collection/array
        foreach (Magnet magnet in magnets) {magnet.ResetPosition();} //reset position for all magnets

        isDoorOpen = false;
    }

    public void SwapDoorArt(string side) {
        //open animation visual for door with inside and outside
        if (side == "inside") {doorRenderer.sprite = doorInside;}
        else {doorRenderer.sprite = doorOutside;}
    }
}
