using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class LinkHandler : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenWindow(string url);

    // This is the function the buttons will call
    public void OpenURL(string url)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            OpenWindow(url);
        #else
            Application.OpenURL(url);
        #endif
    }
}
