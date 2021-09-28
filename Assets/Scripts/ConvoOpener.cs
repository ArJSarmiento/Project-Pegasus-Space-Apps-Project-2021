using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoOpener : Interactable
{
   public override void Interact()
	{ 
        Debug.Log("open convo");
		TopDownMaster.gm.uiManager.SweetWordsToggle();
	}
}