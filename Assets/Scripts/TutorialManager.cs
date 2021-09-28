using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("General")]

    public bool isPlatformer = false;
    bool canNext = false;



	[Header("Platformer")] 
    public Transform target;
    public Transform target2;
    public GameObject joystick;
    public GameObject shootbutton;
    public GameObject[] PlatformerpopUps;
    private int PlatformerpopUpIndex;
    public GameObject Messages;
    public GameObject Pointer;
    public GameObject  RaycastBlocker;
    public Collider2D colliderLoad;
    public GameObject settingsButton;
    

    void Start()
    { 
        if(isPlatformer && PlayerPrefs.GetInt("savedFirstRunTutorial",0) == 0)
        {
            joystick.SetActive(false);
            shootbutton.SetActive(false); 
            settingsButton.SetActive(false);
            colliderLoad. isTrigger = false;
        }
        RaycastBlocker.SetActive(false);  
        target.gameObject.SetActive(false);
        target2.gameObject.SetActive(false);
    }

    void Update() 
   {
       if(isPlatformer && PlayerPrefs.GetInt("savedFirstRunTutorial",0) == 0)
       {
            for (int i = 0; i < PlatformerpopUps.Length; i++)
            {
                if (i == PlatformerpopUpIndex)
                {
                    PlatformerpopUps[i].SetActive(true);
                }
                else
                {
                    PlatformerpopUps[i].SetActive(false);
                }
            }
            if ( PlatformerpopUpIndex == 0)
            {
                if (canNext)   
                {
                    PlatformerpopUpIndex++; 
                    canNext = false;  
                }   
            }
            else if ( PlatformerpopUpIndex == 1)
            {
                
                target.gameObject.SetActive(true);
                joystick.SetActive(true);
                if (canNext)   
                    {
                        target.gameObject.SetActive(false);
                        PlatformerpopUpIndex++; 
                        canNext = false;     
                    }
            }
            else if ( PlatformerpopUpIndex == 2)
            {
                RaycastBlocker.SetActive(true);
                if (canNext)   
                {
                    PlatformerpopUpIndex++; 
                    canNext = false;     
                }
            }
            else if ( PlatformerpopUpIndex == 3)
            {
                shootbutton.SetActive(true);
                target2.gameObject.SetActive(true);

                if(Messages.activeSelf)
                {
                    target2.gameObject.SetActive(false);
                    PlatformerpopUpIndex++; 
                    canNext = false; 
                }
            }
            else if ( PlatformerpopUpIndex == 4)
            {
                if (canNext)   
                {
                    PlatformerpopUpIndex++; 
                    
                    canNext = false; 
                }    
            }
            else if (PlatformerpopUpIndex == 5)
            {
                settingsButton.SetActive(true);
                RaycastBlocker.SetActive(false);  
                colliderLoad. isTrigger = true;
                PlayerPrefs.SetInt("savedFirstRunTutorial",1);
                    isPlatformer = false;

            }
       }
   }

   

    public void nextTutorial()
    {
        if (!canNext)
        {
            canNext = true;
        }      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isPlatformer && other.gameObject.name == "Player")
        {
            if ( PlatformerpopUpIndex ==1 )
            {
                if (!canNext)
                {
                    canNext = true;
                }    
            }      
        }
    }
    public void skipTutorial()
    {
        Time.timeScale = 1f;
        if (isPlatformer)
        {
            isPlatformer = false;
        }
    }
    public void turnOff()
    {
        Pointer.SetActive(false);
    }
}

/*PEST PATROL
Objective
Your mission is to collect all the seeds and prevent the pest from attacking you. You can get the power sprays  that can help you eradicate the incoming pests for a small duration of time.	
Controls
The movements are associated with a joystick on the lower left side of the screen.	

SPACE SHOOTER	
Objective
Another minigame to encounter is space shooter. The minigame is a platformer shooter game where you must destroy and survive incoming hazards. Destroy multiple alien ships to collect Alien Bucks which can be used for upgrades. The upgrade option can be found on the menu screen. 	
Controls
The controls for the movement of this game is associated with a joystick where pressing the upper buttons will make your character jump. The left and right buttons will determine your movement directions.	The button in the right side of the screen will be used to command shooting.	

VACCINE VOYAGE	
You will play as a weak prototype of the mysterious virus and get as far as possible to trigger memory cells improve and strengthen the bodys immune system.	
Controls
Press the left side of the screen to move down. Press the right side of the screen to move up. */
