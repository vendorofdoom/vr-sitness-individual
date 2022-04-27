using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidanceSettings : MonoBehaviour
{
    public bool guidanceEnabled = true;
    public TMPro.TextMeshProUGUI text;

    public string settingsOnText;
    public string settingsOffText;

    private void Awake()
    {
        guidanceEnabled = PlayerPrefs.GetInt("guidanceEnabled", 1) == 1 ? true : false;
    }

    public void ToggleGuidanceSettings()
    {
        guidanceEnabled = !guidanceEnabled;
        PlayerPrefs.SetInt("guidanceEnabled", guidanceEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        if (guidanceEnabled)
        {
            text.text = settingsOnText;
        }
        else
        {
            text.text = settingsOffText;
        }
    }
}
