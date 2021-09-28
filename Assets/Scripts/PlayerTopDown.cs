using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTopDown : MonoBehaviour {

	public enum GameState { first, second, third, fourth};

	public GameState state = GameState.first;
    public GameState State {
        get { return state; }
    }

	public float startHealth = 100;
	public float health ;
	public int worth = 50;
	public GameObject deatheffect;
	
	[Header("Unity Stuff")]
	public Image healthBar;
	private bool isDead= false;
	public float sec = 5f;
//	public StatusIndicator statusIndicator;
	public Transform popupPos;
	public bool hasInsecticide = false;


	void Start ()
	{
		health = startHealth ;
//		statusIndicator.setHealth(health, startHealth);
		
	}
	

	public void TakeDamage(float amount)
	{ 
		
		//amount -= armor;
		health -= amount;
		amount = Mathf.Clamp(amount, 0, int.MaxValue);
		/*h ealthBar.fillAmount = health / startHealth;*/		
	//	statusIndicator.setHealth(health, startHealth);
		Debug.Log(transform.name + " takes " + amount + " damage.");

		if  (health <=0 && !isDead)
		{	
			Die();							
		}

	//	DamagePopup.Create(popupPos.position, amount, true);	
	}
	/* public void Slow (float pct)
	{
		speed = startSpeed * (1f-pct);
	}*/

	public void Heal (int healamount)
	{
		health += healamount;
		health = Mathf.Clamp(health, 0, startHealth);
	}

	public void Die()
	{
		/* ragdoll.transform.parent = null;
		ragdoll.Setup ();*/

		isDead = true;
		//PlayerStats.Money += worth;

		GameObject effect = (GameObject) Instantiate (deatheffect, transform.position, Quaternion.identity);
		Destroy (effect,5f);

		Destroy(gameObject);
//		TopDownMaster.gm.youDiedUI.SetActive(true);
	}
}
