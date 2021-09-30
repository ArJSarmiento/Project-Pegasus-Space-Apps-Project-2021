using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject SettingsUI;
    public GameObject SettingButton;  
    public GameObject MessageUI;
    public GameObject SweetWordsUI;
    public SweetButtons SweetButton;

    public GameObject SweetMessageUI;
    public GameObject MessageObject;
    public GameObject PickupCanvas;

    public GameObject PresstoStartCanvas;

    public Image SweetLogo;
    public Text SweetDetails;
    public Text Message;
    public Text MessagePreview;
    public AudioMixer audioMixer;
    public Slider slider;
    float mysliderValue = 0f;
    public float buttonTimer = 5f;
    public Animator SweetWordsAnimator;
    public bool SweetWordsCanAnimate;
    public float ButtonReactivateDelay = 1f;

    public LevelSelectButton levelSelectButton;
    public Text levelNumber;
    public Text levelDescription;
    public Image levelImage;
    public Animator levelImageAnimator;
    public bool LevelPreview = false;
    public GameObject WinUI;
    public GameObject ActionButton;
    public GameObject Dpad;

    public Button[] buttonsCollection;
    
    public bool isRoom = false;
    private void Start() {
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

        if (isRoom)
        {
            int levelAt = PlayerPrefs.GetInt("levelAt", 4); /* < Change this int value to whatever your
                                                             level selection build index is on your
                                                             build settings */
            for (int i = 0; i < buttonsCollection.Length; i++)
            {
                if (i != 8)
                {
                    if (i + 4 > levelAt)
                        buttonsCollection[i].interactable = false;                    
                }
            }
        }
    }

    private void Update() {
        
        if (SweetMessageUI.activeSelf)
        {
            if(buttonTimer <= 0)
            {
                SweetWordsAnimator.ResetTrigger("ReturnHeart"); 
                SweetWordsAnimator.SetTrigger("InsertHeart");
            }
            else
            {
                buttonTimer -= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
            else
            {
                if (!SettingsUI.activeSelf)
                {
                    AudioManager.instance.playSound("Transition_Open");
                    SettingsUI.SetActive(true);
                    SettingButton.SetActive(false);
                    Time.timeScale = 0;  
                }
                else
                {
                    AudioManager.instance.playSound("Transition_Close");
                    SettingsUI.SetActive(false);
                    SettingButton.SetActive(true);
                    Time.timeScale = 1;
                }                
            }
        }          
    }


    
    // Assign this as your OnClick listener from the inspector
    public void WhenClicked(Button TheButton) 
    {
        TheButton.interactable = false;
        StartCoroutine(EnableButtonAfterDelay(TheButton, ButtonReactivateDelay));
    }
    
    IEnumerator EnableButtonAfterDelay(Button button, float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        button.interactable = true;
    }

    public void TouchSense()
    {
        if (SweetMessageUI.activeSelf)
        {
            SweetWordsAnimator.ResetTrigger("InsertHeart");
            buttonTimer = 5f; 
            SweetWordsAnimator.SetTrigger("ReturnHeart");   
        }
    }

    public void PressToStart()
    {
        if (!PresstoStartCanvas.activeSelf)
        {
            PresstoStartCanvas.SetActive(true);
            AudioManager.instance.playSound("Transition_Open");
        }
        else
        {
            PresstoStartCanvas.SetActive(false);
            AudioManager.instance.playSound("Transition_Close");
        }
    }

    public void SettingsToggle()
    {
        WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        if (!SettingsUI.activeSelf)
        {
            AudioManager.instance.playSound("Transition_Open");
            SettingsUI.SetActive(true);
            SettingButton.SetActive(false);
            Time.timeScale = 0;  
        }
        else
        {
            AudioManager.instance.playSound("Transition_Close");
            SettingsUI.SetActive(false);
            SettingButton.SetActive(true);
            Time.timeScale = 1;
        }
    }

    public void SetVolume(float volume)
    {
        float sliderValue = slider.value;
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void MuteToggle (bool value)
    {
        slider.enabled = !value;
        if (value)
        {
            mysliderValue =  slider.value;
            slider.value = 0.0001f;        
        }
        else
        {
            slider.value= mysliderValue;
        }
    }

    public void LoadLevel()
    {
        if (levelSelectButton!= null)
        {
            TopDownMaster.gm.GetComponent<LevelLoader>().LoadTo(levelSelectButton.Level);
        }
    } 
    
/*    public void SweetMessageToggle()
    {
        if (MessageObject != null)
        {
            
            WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
            if (SweetButton.useList)
            {
                if (!PickupCanvas.activeSelf)
                {
                    AudioManager.instance.playSound("Transition_Open");
                    PickupCanvas.SetActive(true);
                    MessageObject.SetActive(true);
                }
                else
                {
                    AudioManager.instance.playSound("Transition_Close");
                    PickupCanvas.SetActive(false);
                    MessageObject.SetActive(false); 
                }
            }
            else
            {
                if (!SweetMessageUI.activeSelf)
                {
                    AudioManager.instance.playSound("Transition_Open");
                    SweetMessageUI.SetActive(true);
                    MessageObject.SetActive(true);
                }
                else
                {
                    AudioManager.instance.playSound("Transition_Close");
                    SweetMessageUI.SetActive(false);
                    MessageObject.SetActive(false); 
                }                 
            }
        }
    }


    public void WinUiToggle ()
    {
        if (!WinUI.activeSelf)
        {
            AudioManager.instance.playSound("Transition_Open");
            WinUI.SetActive(true);
            SettingButton.SetActive(false); 
            ActionButton.SetActive(false);
            Dpad.SetActive(false);   
        }
        else
        {
            AudioManager.instance.playSound("Transition_Close");
            WinUI.SetActive(false);
            SettingButton.SetActive(true);
            ActionButton.SetActive(true);
            Dpad.SetActive(true);           
        }
    }


    public void MessageToggle()
    {
        WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        if (!MessageUI.activeSelf)
        {
            AudioManager.instance.playSound("Transition_Open");
            MessageUI.SetActive(true);
        }
        else
        {
            AudioManager.instance.playSound("Transition_Close");
            MessageUI.SetActive(false); 
        }
    }
    
    public void SweetWordsToggle()
    {
        WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
        if (!SweetWordsUI.activeSelf)
        {
            AudioManager.instance.playSound("Transition_Open");
            SweetWordsUI.SetActive(true);
            
            SweetButton.GetComponent<Button>().Select();  
           // SweetButton.Pressed();
        }
        else
        {
            AudioManager.instance.playSound("Transition_Close");
            SweetWordsUI.SetActive(false);
        }
    }
    public void OpenLevelPreview () 
    {
        if (!LevelPreview)
        {
            AudioManager.instance.playSound("Transition_Open");
            LevelPreview = true;
        }
        else
        {
            LevelPreview = false;
            AudioManager.instance.playSound("Transition_Close");
        }
        
        levelImageAnimator.SetBool("Open", LevelPreview);
    }

}
