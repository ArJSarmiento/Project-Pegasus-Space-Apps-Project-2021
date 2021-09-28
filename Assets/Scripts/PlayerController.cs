using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private float timeBtwShots;
	public float startTimeBtwShots;
	public GameObject questionmark;
	public GameObject projectile;
	public Transform firepoint;

	public Interactable focus;
	public delegate void OnFocusChanged(Interactable newFocus);
	public OnFocusChanged onFocusChangedCallback;

	public float range = 10f;
	GameObject[] foci;
	GameObject[] enemy;
	bool countingDown = false;

    public TopDownMovement movement;
	public GameObject nearestInteractable = null;
	public float timeShoot;
	public float timehasEnemy;
	public bool hasInteractable = false;

	float shortestDistance;

	void Start()
	{
		foci = GameObject.FindGameObjectsWithTag("Interactable");
	}
	void Update()
	{
		if(TopDownMaster.gm.signRange)
		{
			questionmark.SetActive(true);
		}
		else
		{
			questionmark.SetActive(false);
		}
		
		if (countingDown)
		{
			timeBtwShots-= Time.deltaTime;
		}
		// if (Input.GetButtonDown("Space"))
		// {
		// 	Action();
		// }
	}

	public void Action()
	{	
		float shortestDistance = Mathf.Infinity;
		GameObject nearestInteractable = null;
		
		foreach (GameObject _focus in foci)
		{
			float distance = Vector3.Distance(this.transform.position, _focus.transform.localPosition);
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				nearestInteractable = _focus;
			}
		}			
		
		if (nearestInteractable != null && shortestDistance <= range)
		{
			Interactable focus_ = nearestInteractable.GetComponent<Interactable>();
			float distanceEnemy = Vector3.Distance(this.transform.position, focus_.interactionTransform.localPosition);
			
			
			if(distanceEnemy <= focus_.radius)
			{
				SetFocus(focus_);
				hasInteractable = true;
			}
			else
			{
				SetFocus(null);	
				Attack();
				hasInteractable = false;					
			}  
		}
		else
		{
			SetFocus(null);
           	Attack();
			hasInteractable = false;
		}		
	}

	public void Attack()
	{
		// if(TopDownMaster.gm.Player.hasInsecticide)
		// {
		// 	GameObject[] enemy = GameObject.FindGameObjectsWithTag("BADDIE");
		// 	//attack
		// 	shortestDistance = Mathf.Infinity;
		// 	nearestInteractable = null;
		// 	if (enemy != null)
		// 	{
		// 		foreach (GameObject _focus in enemy)
		// 		{
		// 			float distance = Vector3.Distance(this.transform.position, _focus.transform.localPosition);
		// 			if (distance < shortestDistance)
		// 			{
		// 				shortestDistance = distance;
		// 				nearestInteractable = _focus;
		// 			}
		// 		}	
		// 		if (nearestInteractable != null && shortestDistance <= range)
		// 		{
		// 			movement.hasEnemy = true;
		// 			StartCoroutine ( rotatecontrol() );

		// 			if (timeBtwShots <= 0)
		// 			{
		// 				movement.isShooting = true; // base is shooting on button hold instead of co routine

		// 				StartCoroutine ( shoot() );
		// 				countingDown = false;
		// 				GameObject bulletGo = (GameObject) Instantiate (projectile, firepoint.position, Quaternion.identity);
		// 				PlayerProjectile bullet = bulletGo.GetComponent<PlayerProjectile>();
		// 				if (bullet != null)
		// 				{
		// 					bullet.Seek(nearestInteractable.transform, movement.hasEnemy, movement.movement, movement.m_FacingRight);
		// 				}
		// 				timeBtwShots = startTimeBtwShots;
		// 				Debug.Log ("Attacked");
		// 			}
		// 			else
		// 			{    
		// 				countingDown = true;
		// 			}
		// 		}
		// 		else
		// 		{
					
		// 			movement.hasEnemy = false;

		// 			if (timeBtwShots <= 0)
		// 			{
		// 				movement.isShooting = true;
		// 				StartCoroutine ( shoot() );
		// 				countingDown = false;
		// 				GameObject bulletGo = (GameObject) Instantiate (projectile, firepoint.position, Quaternion.identity);
		// 				PlayerProjectile bullet = bulletGo.GetComponent<PlayerProjectile>();
		// 				if (bullet != null)
		// 				{
		// 					bullet.Seek(firepoint, movement.hasEnemy, movement.movement, movement.m_FacingRight);
		// 				}
		// 				timeBtwShots = startTimeBtwShots;
		// 				Debug.Log ("Attacked");
		// 			}
		// 			else
		// 			{    
		// 				countingDown = true;
		// 			}
		// 		}
		// 	}
		// 	else
		// 	{
		// 		Debug.Log("NoEnemy");
		// 	}				
		// }
	}

	IEnumerator shoot() 
	{
		yield return new WaitForSeconds (timeShoot);
		movement.isShooting = false;
	}

	IEnumerator rotatecontrol() 
	{
		yield return new WaitForSeconds (timeShoot);
		movement.hasEnemy = false;
	}
	
	
	void SetFocus (Interactable newFocus)
	{
		if (onFocusChangedCallback != null)
			onFocusChangedCallback.Invoke(newFocus);

		// If our focus has changed
		if (focus != newFocus && focus != null)
		{
			// Let our previous focus know that it's no longer being focused
			focus.OnDefocused();
		}

		// Set our focus to what we hit
		// If it's not an interactable, simply set it to null
		focus = newFocus;

		if (focus != null)
		{
			// Let our focus know that it's being focused
			focus.OnFocused(transform);
			
		}
	}
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, range);
	}
}