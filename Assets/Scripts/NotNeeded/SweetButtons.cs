using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class SweetButtons : MonoBehaviour
{
    UIManager uiManager;
    public string SweetDetails;
    public Image SweetLogo;
    public string Message;
    public GameObject MyMessageObject;
    public bool isBday = false;
    public bool useList = false;
    public String[] textSplit;
    DateTime yourBirthday = new DateTime(2022, 12, 31);
    public Button myButtonComponent;
    public bool isFirst = false;
    

    void Start()
    {
        uiManager = TopDownMaster.gm.uiManager;
        myButtonComponent = this.GetComponent<Button>();
        if (isBday)
        {
            if ((DateTime.Now.Month == yourBirthday.Month) && (DateTime.Now.Day == yourBirthday.Day))
            {
                PlayerPrefs.SetInt("isBday",  1);   
            }
            
            if (PlayerPrefs.GetInt("isBday",0) == 1 )
            {
                myButtonComponent.interactable = true;
            }
            else
            {
                myButtonComponent.interactable = false;
            }
        }     
        

        if (useList)
        {
            textSplit = Message.Split('/');
            Message = textSplit[Random.Range(0, textSplit.Length-1)];
            MyMessageObject.GetComponent<Text>().text = System.Environment.NewLine + Message;
        }   
    }

    public void Pressed()
    {
        if (useList)
        {
            Message = textSplit[Random.Range(0, textSplit.Length-1)];
            MyMessageObject.GetComponent<Text>().text =  Message;
        }   
        if (uiManager.SweetButton != this || (isFirst)){
            string firsthalf = Message.Substring(0, Message.Length/4) + "...";
            Message = System.Environment.NewLine + Message;
            uiManager.SweetButton =  this;
            uiManager.SweetLogo.sprite = SweetLogo.sprite;
            uiManager.SweetDetails.text = SweetDetails;
            uiManager.MessagePreview.text = firsthalf;
            uiManager.MessageObject = MyMessageObject;
            isFirst = false;
        }
    
    }
}
