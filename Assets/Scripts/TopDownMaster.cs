using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine; 
using UnityEngine.Playables;

public class TopDownMaster : MonoBehaviour {
//singleton instance for other scripts to access this
    public static TopDownMaster gm;

    [Header("Messaging System")]
    [Space(10)]
    [SerializeField]
    List<Message> messageList =  new List<Message>();
    public List<string> myMessages =  new List<string>();
    public List<Sprite> myPictures =  new List<Sprite>();
    public int maxMassages = 25;
    public string username = "";
    public GameObject chatPanel, textObject, pictureObject;
    public InputField chatBox;
    public Color playerMessage, info;
    private TouchScreenKeyboard touchScreenKeyboard;
    private string inputText = string.Empty;

    [Header("Player")] 
    [Space(10)]
    public GameObject Character;
    public GameObject activePlayer;
    public PlayerTopDown Player;   
    public bool signRange;
    private PlayerController controller;   

    public Animator PlayerAnimator;
    public Animator NPCAnimator;

    [Header("Timeline")]
    [Space(10)]
    public  GameObject TimelineManager;
    public PlayableDirector timeline;
    public bool zoomStart = false;
    double lastTime;
 
    [Header("UI")]
    [Space(10)]
    public GameObject[] UIs;
    public GameObject Greetings;
    public GameObject Transition;
    public GameObject StartMessage;
    public bool isFirstRun = false;
    public UIManager uiManager;

    [Header("Audio")]
    [Space(10)]
    public bool isMainMenu = false;

    [Header("LevelLoader")]
    [Space(10)]
    public LevelLoader levelLoader;
    

	void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<TopDownMaster>();
        }    
        if (gm == null) Debug.Log("ada");

        Application.targetFrameRate = 60;
        
        if (Character.GetComponent<PlayerController>()!= null)
        {
            controller = Character.GetComponent<PlayerController>();            
        }

        int firstRun = PlayerPrefs.GetInt("savedFirstRun",0); // variable inside the class, but not inside a function.
        timeline = TimelineManager.GetComponent<PlayableDirector>();  

        if(SceneManager.GetActiveScene ().buildIndex==0)
        {
            if ((firstRun == 0) || PlayerPrefs.GetInt("quizDone",0) == 1) 
            {
                Debug.Log("On");
                firstRun = 1;
                GlobalControl.Instance.firstRun = firstRun;
                Transition.SetActive(false);
                PlayerPrefs.SetInt("savedFirstRun",firstRun);
                isFirstRun = true;
            }
            else
            {
                Debug.Log("Off");
                Greetings.SetActive(false);
                StartMessage.SetActive(false);
                Transition.SetActive(true);
            }
        }            
    }



    private void Update() 
    {
        if (chatBox.text != "")
        {
            if(touchScreenKeyboard == null)
                touchScreenKeyboard = chatBox.touchScreenKeyboard;
              //  inputText = touchScreenKeyboard.text;

        }
        else
        {
            if(!chatBox.isFocused&&Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField(); 
            }
        }

        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessagetoChat("paboritong anak: " + myMessages[Random.Range(0, myMessages.Count-1)] , Message.MessageType.info, null);
                Debug.Log("Space");
            }        
        }

        if (zoomStart && timeline.time >= (timeline.duration/2) && lastTime <  (timeline.duration/2))
        {
            timeline.Pause();
            zoomStart = false;
        }
        lastTime = timeline.time;
    }

    public void EnterKey()
    {
        if (chatBox.text != "")
        {
            SendMessagetoChat(username + ": " + chatBox.text, Message.MessageType.playerMessage, null);
            if ((chatBox.text.Contains("I") && chatBox.text.Contains("love")) || (chatBox.text.Contains("I") && chatBox.text.Contains("miss")) || (chatBox.text.Contains("I") && chatBox.text.Contains("LOVE")) || (chatBox.text.Contains("I") && chatBox.text.Contains("MISS")) || chatBox.text.Contains("HI") || chatBox.text.Contains("Hi")|| chatBox.text.Contains("hi")|| chatBox.text.Contains("Hello")|| chatBox.text.Contains("HELLO")|| chatBox.text.Contains("hello")|| chatBox.text.Contains("good")|| chatBox.text.Contains("Good")|| chatBox.text.Contains("GOOD")|| ((chatBox.text.Contains("i") && (chatBox.text.Contains("love")))))
            {
                StartCoroutine(Reply());
            }
            chatBox.text = "";            
        }
    }

    IEnumerator Reply() {
        
        yield return new WaitForSeconds(1);
        SendMessagetoChat(null, Message.MessageType.info, myPictures[Random.Range(0, myPictures.Count)]);
        SendMessagetoChat("paboritong anak: " +myMessages[Random.Range(0, myMessages.Count-1)], Message.MessageType.info, null);
        SendMessagetoChat("paboritong anak: " +"HE HE", Message.MessageType.info, null);
    }

    public void HideControls(bool isOn)
    {
        foreach (GameObject UI in UIs)
		{
            if (isOn)
            {
                UI.SetActive(true);
            }
            else
            {
			    UI.SetActive(false);                
            }
		}
    }
    // public void OnInputEvent()
    // {
    //     mobileKeys = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.default, false);
    // }

    public void action()
    {
        if (controller != null && timeline.time == 0f)
        {
            controller.Action();
        }    
    }
    
   public void Zoom()
    {
        zoomStart = true;
        timeline.Play();
    }

    public void resetZoom()
    {
        if (zoomStart == false)
        {
            timeline.Resume();
        }
        lastTime = 0;
    }
 
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(Player);
        PlayerData data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        PositionSaver.instance.savedPosition = position;
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
/*
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

      //  position = position;;
        PositionSaver.instance.savedPosition = position;*/ 

            Player.health = data.health;

        Player.hasInsecticide = data.hasPesticide;

        SceneManager.LoadScene(data.sceneName);
        
    
       //StartCoroutine(position(data));
    }

    public void SendMessagetoChat(string text, Message.MessageType messageType, Sprite sprite) 
    {
        if (messageList.Count >= maxMassages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage =  new Message();

        if(text != null)
        {
            newMessage.text=text;
            GameObject newText = Instantiate(textObject, chatPanel.transform);
            newMessage.textObject =  newText.GetComponent<Text>();
            newMessage.textObject.text =  newMessage.text;
            newMessage.textObject.color = MessageTypeColor(messageType);            
        }
        if(sprite != null)
        {
            GameObject newSpite = Instantiate(pictureObject, chatPanel.transform);
            newSpite.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
        }

        messageList.Add(newMessage);
    }

    IEnumerator position(PlayerData data) {
        
        yield return new WaitForSeconds(1);
         Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        Player.transform.position = position;
    }
    
    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;
        switch(messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
        }
        return color;    
    }

    public void Quit()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info
    }
}