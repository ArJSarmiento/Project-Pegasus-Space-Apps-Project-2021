using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
     [SerializeField] private string toScene;
    private SceneController sceneController; 
    private LevelLoader levelLoader;
    public bool canEnterScene = false;
    public Dialogue otherDialogue;

    void Start() 
    {
        sceneController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>();
        levelLoader = TopDownMaster.gm.GetComponent<LevelLoader>();
        this.canEnterScene = GetComponent<Collider2D>().isTrigger = canEnterScene;
    }  

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player") && (canEnterScene) )
        {      
            TopDownMaster.gm.activePlayer.GetComponent<TopDownMovement>().stopMovement();
            levelLoader.LoadLevel(toScene);
            sceneController.LoadScene(toScene);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && (!canEnterScene)) 
        {
            FindObjectOfType<DialogueManager>().StartDialogue(otherDialogue);
        }
    }
}
