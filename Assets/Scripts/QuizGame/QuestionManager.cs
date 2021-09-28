using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    public Question[] questions_Light;
    public Question[] questions_Medium;
    public Question[] questions_Hard;

    public Question[] questions_Current;

    public GameObject questionUI;
    public GameObject optionUI;
    public GameObject playUI;
    public GameObject playAgainUI;
    public GameObject areyousureUI;
    public GameObject backbuttonMain;

    public GameObject[] withimageUI;
    public GameObject[] withoutimageUI;

    public Text questionText;
    public Text question2Text;
    public Text button0Text;
    public Text button1Text;
    public Text scoreText;
    public Text difficultyText;
    public Text[] highscoreText;

    private Queue<Question> questionsQue;
    private Queue<string> questions;
    private Queue<string> option1;
    private Queue<string> option0;

    public Button b1;
    public Button b2;
    public Button medium;
    public Button hard;

    Queue<int> answer;
    Queue<bool> isToF;
    Queue<bool> ToFa;

    public Animator animator1;
    public Animator animator2;
    public Animator animatorQ;
    public Animator animatorPlay;

    public Image image;

    public bool isAnswered =false;
    public int score = 0;
    int difficult = 0;

    private void Start() {
        questionsQue= new Queue<Question>();

        if (PlayerPrefs.GetInt("scoreL",0)>= questions_Light.Length)
        {
            medium.interactable = true;
        }

        if (PlayerPrefs.GetInt("scoreM",0) >= questions_Medium.Length)
        {
            hard.interactable = true;
        }
    }

    public void StartGame()
    {
        score = 0;
        questionsQue.Clear();

        System.Random rnd=new System.Random();         
        questions_Current=questions_Current.OrderBy(x => rnd.Next()).ToArray();    
        
        foreach (Question q in questions_Current)
		{
            questionsQue.Enqueue(q);
		}
        openGame();
        playAgainUI.SetActive(false);
        NextQ();
    }

    Question q;
    
    public void AnswerQ0()
    {
        b1.enabled = false; 
        b2.enabled = false; 
        animator2.SetTrigger("Uninteractable");


        if (q.isToF)
        {
            if(q.ToFa==true)
            {
                AudioManager.instance.playSound("Correct");
                animator1.SetTrigger("Correct");
                score++;
            }
            else
            {
                AudioManager.instance.playSound("Wrong");
                animator1.SetTrigger("Wrong");
            }
        }
        else
        {
            if (q.answer == 0)
            {
                AudioManager.instance.playSound("Correct");
                animator1.SetTrigger("Correct");
                score++;
            }
            else
            {
                AudioManager.instance.playSound("Wrong");
                animator1.SetTrigger("Wrong");
            }
        }
        animatorQ.SetTrigger("Exit");
        NextQ();
    }

    
    public void AnswerQ1()
    {
        b1.enabled = false; 
        b2.enabled = false; 
        animator1.SetTrigger("Uninteractable");
        if (q.isToF)
        {
            if(q.ToFa==false)
            {
                AudioManager.instance.playSound("Correct");
                 animator2.SetTrigger("Correct");
                score++;
            }
            else
            {
                AudioManager.instance.playSound("Wrong");
                animator2.SetTrigger("Wrong");
            }
        }
        else
        {
            if (q.answer == 1)
            {
                AudioManager.instance.playSound("Correct");
                animator2.SetTrigger("Correct");
                score++;
            }
            else{
                AudioManager.instance.playSound("Wrong");
                animator2.SetTrigger("Wrong");
            }
        }
        animatorQ.SetTrigger("Exit");
        NextQ();
    }


    public void NextQ()
    {
        if (questionsQue.Count == 0)
        {
            EndGame();
            return;
        }
        animatorQ.SetTrigger("Enter");
        q = questionsQue.Dequeue();
        animator1.SetTrigger("Normal");
        animator2.SetTrigger("Normal");
        StartCoroutine("enableButtons"); 
    }

    IEnumerator enableButtons() {
        yield return new WaitForSecondsRealtime(0.75f); 

        questionText.text = q.question;
        question2Text.text = q.question;
        
        if (q.isToF)
        {
            button0Text.text = "True";
            button1Text.text = "False";
        }
        else
        {
            button0Text.text = q.option0;
            button1Text.text = q.option1;
        }
        if (q.picture != null)
        {
            image.sprite = q.picture;
            foreach(GameObject ob in withoutimageUI)
            {
                ob.SetActive(false);
            }

            foreach(GameObject ob in withimageUI)
            {
                ob.SetActive(true);
            }
        }
        else
        {
            foreach(GameObject ob in withoutimageUI)
            {
                ob.SetActive(true);
            }

            foreach(GameObject ob in withimageUI)
            {
                ob.SetActive(false);
            }
        }
        b1.enabled = true;
        b2.enabled = true;
    }


    public void EndGame()
    {
        SetScore();
        openPlayAgain();

        if (PlayerPrefs.GetInt("scoreL",0)>= questions_Light.Length)
        {
            medium.interactable = true;
        }

        if (PlayerPrefs.GetInt("scoreM",0) >= questions_Medium.Length)
        {
            hard.interactable = true;
        }

        score = 0;
    }

    public void SetScore()
    {
        int scs = 0;

        if (difficult == 0)
        {
            scs = PlayerPrefs.GetInt("scoreL",0);
            if (scs < score)
            {
               PlayerPrefs.SetInt("scoreL",score); 
               scs = score;
            }
        }
        if (difficult == 1 )
        { 
            scs = PlayerPrefs.GetInt("scoreM",0);
            if (scs < score)
            {
               PlayerPrefs.SetInt("scoreM",score);
               scs = score;
            }
        } 
        if (difficult == 2 )
        {
            scs = PlayerPrefs.GetInt("scoreH",0) ;

            if (scs < score)
            {
                PlayerPrefs.SetInt("scoreH",score);
                scs = score;
            }
            if (PlayerPrefs.GetInt("scoreH",score)==questions_Hard.Length)
            {
                if (PlayerPrefs.GetInt("quizDone",0) == 0)
                {
                    PlayerPrefs.SetInt("quizDone",1);
                    TopDownMaster.gm.GetComponent<LevelLoader>().LoadToMainMenu();
                }

            }
        }

        foreach (Text s in highscoreText)
        {
            s.text = scs.ToString();
        }

        scoreText.text = score.ToString();
    }

    public void openLight()
    {
        backbuttonToggle();
        difficult = 0;
        difficultyText.text = "Light Mode";
        SetScore();
        openPlay();
      
        questions_Current=questions_Light;
    }

    public void openMedium()
    {
        backbuttonToggle();
        difficult = 1;
        difficultyText.text = "Medium Mode";
        SetScore();
        openPlay();
       
        questions_Current=questions_Medium;    
    }

    public void openHard()
    {
        backbuttonToggle();
        difficult = 2; 
        difficultyText.text = "Hard Mode";
        SetScore();
        openPlay();   
        
        questions_Current=questions_Hard;    
    }

    public void openPlay()
    {
        if (optionUI.activeSelf)
        {
            optionUI.SetActive(false);
            playUI.SetActive(true);          
        }
        else
        {
            optionUI.SetActive(true);
           playUI.SetActive(false);  
           playAgainUI.SetActive(false);
        }
    }

    public void openGame()
    {
        if (playUI.activeSelf)
        {
            playUI.SetActive(false);
            questionUI.SetActive(true);          
        }
        else
        {
            playUI.SetActive(true);
            questionUI.SetActive(false);   
        }
    }

    public void openPlayAgain()
    {
        if (playAgainUI.activeSelf)
        {
            playAgainUI.SetActive(false);
            questionUI.SetActive(true);          
        }
        else
        {
            playAgainUI.SetActive(true);
            questionUI.SetActive(false);   
        }
    }

    public void aysToggle()
    {
        if(areyousureUI.activeSelf)
        {
            areyousureUI.SetActive(false);
        }
        else
        {   
            areyousureUI.SetActive(true);
        }
    }

    public void backbuttonToggle()
    {
        if (backbuttonMain.activeSelf)
        {
            backbuttonMain.SetActive(false);
        }
        else
        {
            backbuttonMain.SetActive(true);
        }
        
    }
}
