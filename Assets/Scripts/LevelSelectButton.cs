using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public string Level;
    public string LevelText;
    public string levelDescription;
    public Sprite LevelImage;
    UIManager uiManager;

    void Start()
    {
        uiManager = TopDownMaster.gm.uiManager;
    }

    public void Pressed() 
    {
        if (uiManager.levelSelectButton != this)
        {
            uiManager.levelSelectButton = this; 
            uiManager.levelImage.sprite = LevelImage;
            uiManager.levelNumber.text = LevelText;
            uiManager.levelDescription.text = levelDescription;
        }
    }
}
