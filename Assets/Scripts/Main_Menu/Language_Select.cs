using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class For The Lenguage Selection
/// </summary>
public class Language_Select : MonoBehaviour
{
    private const string Key = "Lenguaje";
    [SerializeField] private GameObject ls_Screen;
    enum Language
    {
        Spanish,English
    }
    public void Start()
    {
        if (PlayerPrefs.HasKey(Key))
        {
            ls_Screen.SetActive(false);
        }
        else 
        {
            ls_Screen.SetActive(true);
        }
    }

    /// <summary>
    /// Function For The Spanish Language Selection
    /// </summary>
    public void SelectSpanish() 
    {
        //TODO - Fix - Hardcoded value
        PlayerPrefs.SetString(Key, Language.Spanish.ToString());
        ls_Screen.SetActive(false);
    }

    /// <summary>
    /// Function For The English Language Selection
    /// </summary>
    public void SelectEnglish()
    {
        //TODO - Fix - Hardcoded value
        PlayerPrefs.SetString(Key, Language.English.ToString());
        ls_Screen.SetActive(false);
    }
}
