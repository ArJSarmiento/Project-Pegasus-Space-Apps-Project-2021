using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;


public class NPC : Interactable {
	public bool isStartDialog = false;

	public Dialogue dialogue;
	public Animator animator;
	public Dialogue otherDialogue;
	public Dialogue congratsDialogue;
	public bool isFirst = true;
	public string[] textSplit;
	public string PickUpLine;


	private void Start() {
		if (isStartDialog || PlayerPrefs.GetInt("quizDone",0) == 1)
		{
			
			TriggerDialogue();
		}
		if (PickUpLine!="" && PickUpLine!= null)
		{
			textSplit = PickUpLine.Split('/');
            PickUpLine = textSplit[Random.Range(0, textSplit.Length-1)];
		}
	}

	public override void Interact()
	{ 
		Debug.Log ("INTERACT");
		TopDownMaster.gm.NPCAnimator = animator;
		animator.SetBool("Interact", true);
		animator.SetFloat("Vertical",(TopDownMaster.gm.PlayerAnimator.GetFloat("Vertical")*(-1)));
		animator.SetFloat("Horizontal",(TopDownMaster.gm.PlayerAnimator.GetFloat("Horizontal")*(-1)));

		// FlipManage();		
		TriggerDialogue();
	}

	public void TriggerDialogue ()
	{ 
		if (PlayerPrefs.GetInt("quizDone",0) == 1)
		{
			FindObjectOfType<DialogueManager>().StartDialogue(congratsDialogue);
			AudioManager.instance.stopSound("Making Love Out Of Nothing At All");
			AudioManager.instance.playSound("Even");
			PlayerPrefs.SetInt("quizDone",2);
		}

		if (PlayerPrefs.GetInt("quizDone",0) == 0)
		{
			if (isFirst)
			{
				FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
				isFirst = false;
			}
			else
			{
				string[] _Sentences = new string[textSplit.Length + TopDownMaster.gm.myMessages.ToArray().Length];
				Array.Copy(textSplit, _Sentences, textSplit.Length);
				Array.Copy(TopDownMaster.gm.myMessages.ToArray(), 0, _Sentences, textSplit.Length, TopDownMaster.gm.myMessages.ToArray().Length);
				otherDialogue.sentences = new string[1];
				otherDialogue.names = new string[1];

				otherDialogue.sentences[0] = _Sentences[Random.Range(0, _Sentences.Length-1)];
				otherDialogue.names[0]= "Arnel";
				FindObjectOfType<DialogueManager>().StartDialogue(otherDialogue);
			}		
		}
		if  (!isStartDialog)
			TopDownMaster.gm.Zoom();
	}
}