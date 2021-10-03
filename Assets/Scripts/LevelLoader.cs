using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private string Memories;
    [SerializeField] private string MainMenu;
    
    [SerializeField] private string LevelSelect;
    [SerializeField] private string LevelOne;
    [SerializeField] private string House;
    

    public Animator transition;
    public float transitionTime = 1f;
    public Image spriteRenderer;
    public List<Sprite> mySprites =  new List<Sprite>();
    public Sprite newSprite;

    private void Start() {
        spriteRenderer.sprite = GlobalControl.Instance.sprite;
    }

    void ChangeSprite()
    {
        newSprite = mySprites[Random.Range(0, mySprites.Count)];
        spriteRenderer.sprite = newSprite; 
        GlobalControl.Instance.sprite = newSprite;
    }

    public void LoadLevel(string sceneName)
    {
        if(sceneName == LevelSelect && SceneManager.GetActiveScene ().buildIndex==2)
        {
            if (PlayerPrefs.GetInt("savedFirstRun1",0) ==0)
            {
                sceneName = LevelOne;   
                PlayerPrefs.SetInt("savedFirstRun1",1) ;
            }
        }
        Time.timeScale = 1f;
        StartCoroutine(LoadAsynchronously(sceneName)) ;
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        ChangeSprite();
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        
        SceneManager.LoadScene(sceneName);
    }
    public void WhenClicked(Button TheButton, float ButtonReactivateDelay) 
    {
        TheButton.interactable = false;
        StartCoroutine(EnableButtonAfterDelay(TheButton, ButtonReactivateDelay));
    }
    
    IEnumerator EnableButtonAfterDelay(Button button, float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        button.interactable = true;
    }

    public void LoadToMemories()
    {
        WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>(), 1f);
        
        if (SceneManager.GetActiveScene ().buildIndex==0)
        {
            if (PlayerPrefs.GetInt("savedFirstRun2",0) ==0)
            {
                LoadLevel(Memories);  
                PlayerPrefs.SetInt("savedFirstRun2",1) ;
            }
            else
            {
                LoadLevel(LevelSelect);   
            }
        }
        else
        {
            LoadLevel(Memories); 
        }
       
    }

    public void LoadToHouse()
    {
        WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>(), 1f);
        LoadLevel(House);   
    }    
    public void LoadToMainMenu()
    {
      //  WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>(), 1f);
        LoadLevel(MainMenu);
    }

    public void LoadtoLevelSelect()
    {
    
        if (SceneManager.GetActiveScene ().buildIndex==2)
        {
            if (PlayerPrefs.GetInt("savedFirstRun1",0) ==0)
            {
                LoadLevel(LevelOne);   
                PlayerPrefs.SetInt("savedFirstRun1",1) ;
            }
            else
            {
                LoadLevel(LevelSelect);   
            }
        }
        else
        {
            
            LoadLevel(LevelSelect);     
        }
    }

    public void LoadTo(string Level)
    {
        WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>(), 1f);
        LoadLevel(Level);
    }

    public void idxLoadTo(int Level)
    {
        
        string path = SceneUtility.GetScenePathByBuildIndex(Level);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        LoadLevel(name.Substring(0, dot));
    }
}
