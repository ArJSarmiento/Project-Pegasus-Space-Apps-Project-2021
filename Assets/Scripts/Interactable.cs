using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour {

	public Transform interactionTransform;
	public float radius = 3f;
	public bool isNpc = false;
	private bool isFocus = false;
	private bool hasInteracted = false;
	private Transform player;
	bool m_FacingRight = true; 
	

	public virtual void Interact()
	{
		Debug.Log ("INTERACT");
	}


	void Update()
	{
		if (isFocus && !hasInteracted)
		{
			float distance = Vector3.Distance(player.position, interactionTransform.localPosition);
			if (distance <= radius)
			{
				Interact();
				hasInteracted = true;
				TopDownMaster.gm.signRange = false;
			}
			else
			{
				return;
			}
		}

		if (isNpc)
			IsBAboveA(transform, TopDownMaster.gm.activePlayer.transform);
	}


	public void OnFocused (Transform playerTransform)
	{
		isFocus = true;
		player = playerTransform;
		hasInteracted = false;
	}

	public void OnDefocused()
	{
		isFocus = false;
		player = null;
		hasInteracted = false;
	}
		
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player") )
		{
			TopDownMaster.gm.signRange = true;
		}
	}  

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player") )
		{
			TopDownMaster.gm.signRange = false;
		}
	}

	public void IsBAboveA(Transform A, Transform B) 
	{
		if (A.position.y < B.position.y)
		{
			A.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1; 
			B.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;
		}
		else
		{
			A.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0; 
			B.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
		}	
	}	
}
