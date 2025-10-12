using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Settings : MonoBehaviour
{
    // Section video
    bool fullScreen = false;
    string resolution = "HD";

    // Full Screen
    public void FullScreenSet()
    {
        if (fullScreen == false)
        {
            fullScreen = true;
        }
        else if (fullScreen == true)
        {
            fullScreen = false;
        }
        else
        {
            Debug.LogError("Error, the full screen button has an error");
            return;
        }
        Screen.fullScreen = fullScreen;
        Debug.Log($"Full screen {fullScreen}");
    }

    // Résolution
    public void ResolutionSetDroite()
    {
        if (resolution == "HD") // Set en full hd
        {
            Screen.SetResolution(1920, 1080, fullScreen);
            resolution = "FullHD";
        }
        else if (resolution == "FullHD")
        {
            Screen.SetResolution(2560, 1440, fullScreen);
            resolution = "2K";
        }
        else if (resolution == "2K")
        {
            Screen.SetResolution(3840, 2160, fullScreen);
            resolution = "4K";
        }
        else if (resolution == "4K")
        {
            Screen.SetResolution(3440, 1440, fullScreen);
            resolution = "UltraWide";
        }
        else if (resolution == "UltraWide")
        {
            Screen.SetResolution(1280, 720, fullScreen);
        }
        else
        {
            Debug.LogError("Error, the Resolution Button has an error");
            return;
        }
        Debug.Log("Résolution : " + resolution);
        Debug.Log($"The resolution is : {resolution}");
        Debug.Log($"Weidth = {Screen.width}");
        Debug.Log($"Height = {Screen.height}");
        Debug.Log($"Full Screen = {Screen.fullScreen}");
        GameObject objetText = GameObject.Find("Canvas");
        TextMeshProUGUI texte = objetText.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Debug.Log(objetText.transform.GetChild(1).transform.GetChild(1).name);
        
        texte.text = $"Résolution : {resolution} \n Weidth = {Screen.width} \n Height = {Screen.height} \n Full Screen = {Screen.fullScreen}";
    }


    // Son

    // Son musique
    public void SetSonJeu()
    {
        
    }
}
