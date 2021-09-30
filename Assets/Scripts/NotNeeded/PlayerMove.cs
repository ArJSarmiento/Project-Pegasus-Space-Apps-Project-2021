using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Net.Http;
using System.Threading.Tasks;

// 현재 선택된 플레이어(예를 들어서 baba is you 라면 baba가 플레이어임)를 이동시켜줌.
public class PlayerMove : MonoBehaviour
{
    private static PlayerMove instance;
    public List<GameObject> currentPlayer = new List<GameObject>();
    public GameObject[] allObjects;
    public Animator an;
    public int moveDistance = 1;
    public Vector2  movement;
    public bool canMove = true;
    float buttonTimer = 1f;
    public float prevSpeed;
    public float CooldownTime;
    public GameObject Baba;
    List<GameObject[]> movedObj = new List<GameObject[]>();
    List<int> intObj =  new List<int>();
    
    bool canReset = false;
    public int nextSceneLoad;
    bool canSetSceneInt = true;
    public SpriteRenderer playerSprite;
    public bool isSinking = false;

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(this.gameObject);
        allObjects = GameObject.FindGameObjectsWithTag("object");
    }
    public static PlayerMove Instance
    {
        get
        {
            if(instance == null) return null;
            return instance;
        }
    }
    private void Start() {
        prevSpeed = an.speed;
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void Update()
    {        
        if (canMove)
        {
            if(!isSinking)
            {
                movement.x = SimpleInput.GetAxisRaw("Horizontal");
                movement.y = SimpleInput.GetAxisRaw("Vertical"); 

                if(Input.GetKeyDown("right") || movement.x==1)
                {
                    List<GameObject> movingObjects = new List<GameObject>();
                    movingObjects = moveObjects(movingObjects, 1, 0);
                    // 중복을 제거해준다. 벽이 플레이어일 경우 겹치는 오브젝트들이 있을 수 있으므로.
                    movingObjects = movingObjects.Distinct().ToList();
                    an.speed = prevSpeed;

                    if (movingObjects!=null&& movingObjects.Count!=0)
                    {
                    movedObj.Add(movingObjects.ToArray()); 
                    intObj.Add(3);
                    }

                    foreach(GameObject ob in movingObjects)
                    {
                        ob.transform.Translate(new Vector2(moveDistance, 0));
                        if(ob.name == "Baba") an.Play("babaright");
                    }
                    if(RuleManager.Instance.isWin()) 
                    {
                       
                        
                        TopDownMaster.gm.uiManager.WinUiToggle ();
                        
                    }
                    Sink(movingObjects);
                    canMove = false;
                }
                if(Input.GetKeyDown("left")|| movement.x==-1)
                {
                    List<GameObject> movingObjects = new List<GameObject>();
                    movingObjects = moveObjects(movingObjects, -1, 0);
                    movingObjects = movingObjects.Distinct().ToList();
                    an.speed = prevSpeed;
                    
                    if (movingObjects!=null&& movingObjects.Count!=0)
                    {
                        movedObj.Add(movingObjects.ToArray()); 
                        intObj.Add(4);         
                    } 

                    foreach(GameObject ob in movingObjects)
                    {
                        ob.transform.Translate(new Vector2(-moveDistance, 0));
                        if(ob.name == "Baba") an.Play("babaleft");
                    }
                    if(RuleManager.Instance.isWin()) 
                    {
                      
                        TopDownMaster.gm.uiManager.WinUiToggle ();
                         
                    }
                    Sink(movingObjects);                   
                    canMove = false;
                }
                if(Input.GetKeyDown("up")|| movement.y==1)
                {
                    List<GameObject> movingObjects = new List<GameObject>();
                    movingObjects = moveObjects(movingObjects, 0, 1);
                    movingObjects = movingObjects.Distinct().ToList();
                    an.speed = prevSpeed;
        

                    if (movingObjects!=null && movingObjects.Count!=0)
                    {
                        movedObj.Add(movingObjects.ToArray());
                        intObj.Add(1);                  
                    }

                    foreach(GameObject ob in movingObjects)
                    {
                        ob.transform.Translate(new Vector2(0, moveDistance));
                        if(ob.name == "Baba") an.Play("babaup");
                    }
                    if(RuleManager.Instance.isWin()) 
                    {
                       
                        
                        TopDownMaster.gm.uiManager.WinUiToggle ();
                         
                    }
                    Sink(movingObjects);                    
                    canMove = false;
                }
                if(Input.GetKeyDown("down")|| movement.y==-1)
                {
                    List<GameObject> movingObjects = new List<GameObject>();
                    movingObjects = moveObjects(movingObjects, 0, -1);
                    movingObjects = movingObjects.Distinct().ToList();
                    an.speed = prevSpeed;
        
                    if (movingObjects!=null&& movingObjects.Count!=0)
                    {
                        movedObj.Add(movingObjects.ToArray());  
                        intObj.Add(2);                   
                    }

                    foreach(GameObject ob in movingObjects)
                    {
                        ob.transform.Translate(new Vector2(0, -moveDistance));
                        if(ob.name == "Baba") an.Play("babadown");
                    }

                    if(RuleManager.Instance.isWin()) 
                    {
                      
                        TopDownMaster.gm.uiManager.WinUiToggle ();
                         
                    }

                    Sink(movingObjects); 

                    canMove = false;
                }
            }
        }
        else
        {
            an.speed = 0;

            if(buttonTimer <= 0)
            {  
                buttonTimer = CooldownTime;
                canMove = true;
            }
            else
            {
                buttonTimer -= Time.deltaTime;
            }
        }
        canReset = true;
    }

    public void Win()
    {
        WhenClicked(EventSystem.current.currentSelectedGameObject.GetComponent<Button>(), 1f);
        if(canSetSceneInt)
        {
            canSetSceneInt = false;
            if(SceneManager.GetActiveScene().buildIndex == 12) /* < Change this int value to whatever your
                                                                    last level build index is on your                                                          build settings */
            {
                TopDownMaster.gm.levelLoader.LoadToMainMenu();
            }
            else
            {
                //Move to next level
                TopDownMaster.gm.levelLoader.idxLoadTo(nextSceneLoad);

                //Setting Int for Index
                if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
                {
                    PlayerPrefs.SetInt("levelAt", nextSceneLoad);
                }
            }            
        }
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

    List<GameObject> waters =  new List<GameObject>();
    List<GameObject> otherPlayer =  new List<GameObject>();

    public GameObject empty;
       
    public void Sink(List<GameObject> sensed)
    {
        bool cansink = false;
        bool isSame = false;
        List<GameObject> sensedObjects =  new List<GameObject>();

        foreach (GameObject s in sensed)
        {
            if(s.transform.childCount > 0)
            {
                GameObject moved = s.transform.GetChild(0).gameObject;
                
                if (moved.tag =="notString")
                {
                    sensedObjects.Add(s);  
                }                                                 
            }
        }

        foreach(GameObject ob in sensedObjects)
        {
            foreach(GameObject ao in PlayerMove.Instance.allObjects)
            {
                if(ob == ao) continue;
                if(ob.transform.position == ao.transform.position)
                {
                    if(ao.GetComponent<Rule>().isSink && ao.activeSelf)
                    {
                        waters.Add(ao);
                        cansink = true;
                        isSame  =true;
                        ao.SetActive(false);

                       List<GameObject> duplicate = currentPlayer;
                        otherPlayer.Add(ob); 
                        for(int i=currentPlayer.Count-1;i >= 0; i--)
                        {
                            if(ob==currentPlayer[i])
                            {
                                if(currentPlayer.Count <= 1)
                                {
                                    currentPlayer[i].GetComponent<SpriteRenderer>().enabled = false;
                                    isSinking = true;
                                    break;
                                }
                            }
                        }
                                
                        if(!isSinking && ob!= null)
                        {
                            ob.SetActive(false);
                        }
                    }
                }
            }
        }
        if (!cansink)
        {
            waters.Add(empty);
        }
        if(!isSame)
        {
            otherPlayer.Add(empty);
        }
    }

    public void ResetMove()
    {
        if (movedObj != null && movedObj.Count != 0 && canReset)
        {
            foreach(GameObject ob in movedObj[movedObj.Count-1])
            {
                ob.SetActive(true);
                int num = intObj[intObj.Count-1];
                if (num ==1)
                {
                    ob.transform.Translate(new Vector2(0, -moveDistance));
                }
                else if(num ==2)
                {
                    ob.transform.Translate(new Vector2(0, moveDistance));
                }
                else if (num ==3)
                {
                    ob.transform.Translate(new Vector2(-moveDistance, 0));
                }
                else
                {
                    ob.transform.Translate(new Vector2(moveDistance, 0));
                }
            }
            movedObj.RemoveAt(movedObj.Count - 1);
            intObj.RemoveAt(intObj.Count - 1);

        
            if (waters != null && waters.Count != 0)
            {
                waters[waters.Count - 1].SetActive(true);
                waters.RemoveAt(waters.Count - 1);               
            }

            if (isSinking)
            {
                isSinking = false;
                otherPlayer[otherPlayer.Count - 1].GetComponent<SpriteRenderer>().enabled = true;                
            }
            else
            {
                if (otherPlayer != null && otherPlayer.Count != 0)
                {
                    GameObject op = otherPlayer[otherPlayer.Count - 1];

                    if(op.tag != "empty")
                    {
                        op.SetActive(true);                      
                    }
                    otherPlayer.RemoveAt(otherPlayer.Count - 1);    
                }
            }

            Debug.Log("reset");      
        }
    }

    public List<GameObject> moveObjects(List<GameObject> movingObjects, float dx, float dy)
    {
        List<GameObject> actualPlayer = new List<GameObject>();
        // 현재 선택된 플레이어를 기준으로 움직여야할 오브젝트들을 구해준다.
        foreach (GameObject ap in currentPlayer)
        {
            if(ap.activeSelf)
            {
                actualPlayer.Add(ap);
            }
        }

        foreach(GameObject cp in actualPlayer)
        {
            float currentX = cp.transform.position.x + dx;
            float currentY = cp.transform.position.y + dy;
            movingObjects.Add(cp);
            for(int i = 0; i < 30; i++)
            {
                bool check = false;
                foreach(GameObject ao in allObjects)
                {
                    if(ao.transform.position.x == currentX && ao.transform.position.y == currentY)
                    {
                        if(ao.GetComponent<Rule>().isStop)
                        {
                            if(!ao.GetComponent<Rule>().isPush) return new List<GameObject>();
                        }
                        if(!ao.GetComponent<Rule>().isPush || !ao.activeSelf) continue;
                        movingObjects.Add(ao);
                        check = true;
                    }
                }
                currentX += dx;
                currentY += dy;
                if(!check) break;
            }
        }
        return movingObjects;
    }
}
