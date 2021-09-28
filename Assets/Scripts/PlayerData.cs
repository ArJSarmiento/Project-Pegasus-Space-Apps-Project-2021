using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float[] position;
    public string sceneName; 
    public bool hasPesticide;   
    public PlayerData(PlayerTopDown player)
    {
        hasPesticide = player.hasInsecticide;
        sceneName = SceneManager.GetActiveScene().name;

         

        health = player.health;

        position = new float[3];
        position [0] =  player.transform.position.x;
        position [1] =  player.transform.position.y;
        position [2] =  player.transform.position.z;

        //World.instance.savedPosition = position;
    }
 

}
