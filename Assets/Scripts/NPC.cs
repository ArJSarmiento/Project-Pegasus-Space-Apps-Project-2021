using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;


public class NPC : Interactable {
	public bool isStartDialog = false;

	public Dialogue dialogue;
	public Animator animator;
	public Dialogue congratsDialogue;
	public bool isFirst = true;

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

	
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		


		TopDownMaster.gm.Zoom();
	}
}