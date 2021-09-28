using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* SUBSCRIBING TO MY YOUTUBE CHANNEL: 'VIN CODES' WILL HELP WITH MORE VIDEOS AND CODE SHARING IN THE FUTURE :) THANK YOU */

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;
    public int prefs;

    // Start is called before the first frame update
    void Start()
    {
        prefs = PlayerPrefs.GetInt("levelAt", 4);
        int levelAt = prefs; /* < Change this int value to whatever your
                                                             level selection build index is on your
                                                             build settings */

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 4 > levelAt)
                lvlButtons[i].interactable = false;
        }
    }

}
