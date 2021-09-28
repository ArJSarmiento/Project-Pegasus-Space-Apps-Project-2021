using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class World : SceneController {
 
    public string RoomJ= "House";
    public string Menu;
    public string Room2;
    public Transform player;
    public Transform RoomJTransform;
    public Transform Room2Transform;
    public Transform MenuTransform;

   /*  private void Awake() {

        if (instance!= null)
        {
            GameObject.Destroy(instance);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
 */
    // Use this for initialization
    public override void Start () 
    {
        base.Start();

        //player.position = PositionSaver.instance.savedPosition;
 
        if (prevScene == RoomJ)
        {
            player.position = RoomJTransform.position;
        }   
        else if(prevScene == Menu)  
        {
            player.position = MenuTransform.position;
        }  
        else if(prevScene == Room2)  
        {
            player.position = Room2Transform.position;
        }  
    }
}
