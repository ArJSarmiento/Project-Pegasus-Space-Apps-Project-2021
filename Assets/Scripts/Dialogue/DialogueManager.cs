using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
	public GameObject dialogbox;
	public Animator animator;
	public GameObject[]  UIs;

	private Queue<string> names;
	private Queue<string> sentences;
	float typeDuration;
	bool  isTyping = false;
	string sentence_;

	// Use this for initialization
	void Awake() 
	{
		sentences = new Queue<string>();
		names = new Queue<string>();
		dialogbox.SetActive(false);
	}
	
	public void StartDialogue (Dialogue dialogue)
	{
		dialogbox.SetActive(true);
		TopDownMaster.gm.activePlayer.GetComponent<TopDownMovement>().enabled = false;
		if (UIs.Length > 0)
		{
			foreach (GameObject UI in UIs)
			{
				UI.SetActive(false);
			}
		}

		animator.SetBool("IsOpen", true);

		
		foreach (string name in dialogue.names)
		{
			names.Enqueue(name);
		}

		//nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
	
			if(isTyping)
			{
				StopAllCoroutines();
				rushDialog(sentence_);
			}
			else
			{
				if (sentences.Count == 0)
				{
					EndDialogue();
					isTyping = false;
					return;
				}		
				else{
					string sentence = sentences.Dequeue();
					string name = names.Dequeue();
					nameText.text = name;
					
					StartCoroutine(TypeSentence(sentence));
					sentence_ = sentence;	
				}
			}
		
	}

	void rushDialog(string _sentence)
	{
		isTyping = false;
		dialogueText.text = _sentence;
	}

	// IEnumerator TypeSentence (string sentence)
	// {
	// 	dialogueText.text = "";
		
	// 	foreach (char letter in sentence.ToCharArray())
	// 	{
	// 		dialogueText.text += letter;
	// 		yield return null;
	// 	}
	// }

	IEnumerator TypeSentence (string sentence)
	{
		isTyping = true;
		dialogueText.text = "";

		foreach (char letter in sentence.ToCharArray())
		{		
			typeDuration = Random.Range(0, 0.1f);
			yield return new WaitForSeconds(typeDuration);
			dialogueText.text += letter;
		}
		isTyping = false;
	}

	void EndDialogue()
	{
		TopDownMaster TPM = TopDownMaster.gm;
		if (TPM.NPCAnimator != null)
		{
			TPM.NPCAnimator.SetFloat("Vertical",0f);
			TPM.NPCAnimator.SetFloat("Horizontal",0f);
			TPM.NPCAnimator.SetBool("Interact", false);			
		}
		if (UIs.Length > 0)
		{
			foreach (GameObject UI in UIs)
			{
				UI.SetActive(true);
			}
		}
		TPM.resetZoom();
		animator.SetBool("IsOpen", false);
		TopDownMaster.gm.activePlayer.GetComponent<TopDownMovement>().enabled = true;
		dialogbox.SetActive(false);
	}
}
