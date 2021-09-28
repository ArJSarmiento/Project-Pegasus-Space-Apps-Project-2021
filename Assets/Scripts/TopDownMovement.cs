using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour {

	TopDownMaster master;
	
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private bool m_RightSideUp = true;  //  for determining if the arm should be inverted for left vs right direction

	private Transform playerGraphicsTop;   //REference to the player graphics so we can change direction ourself
    private Transform playerGraphicsBottom;

	public Rigidbody2D rb;
	public Transform playerArm;        //ref to the arm used for roataion determined facing
	public Animator animatorTop;
    public Animator animator;
	public float runspeed = 1f;
	public Vector2  movement;

    public bool isShooting = false;
	public bool hasEnemy = false;

	public PlayerController playercontroller;
	public bool hasConversation = false;
	



	// void Awake()
	// {
	// 	playerGraphicsTop = transform.Find("TopGraphics");
    //     playerGraphicsBottom = transform.Find("BottomGraphics");

	// 	if(playerGraphicsTop == null ||  playerGraphicsBottom==null) {
	// 		//couldn't find the graphics object
	// 		Debug.LogError("PlatformCharacter2D.cs - No player graphics detected as a child of the player. This is bad");
	// 	}
	// }
	
	// void Start()  
	// {
	// 	master = TopDownMaster.gm.GetComponent<TopDownMaster>();
	// 	//rotate firepoint transform if there is no enemy
	// }


	void Update()
	{
		if (!hasConversation)
		{
			//movement = master.MovementJoy.Direction;
			movement.x = SimpleInput.GetAxisRaw("Horizontal");
			movement.y = SimpleInput.GetAxisRaw("Vertical");

			if(SimpleInput.GetAxisRaw("Horizontal") == 0)
			{
				movement.x = Input.GetAxisRaw("Horizontal");
			}
			if(SimpleInput.GetAxisRaw("Vertical") == 0)
			{
				movement.y = SimpleInput.GetAxisRaw("Vertical");
			}
		}
		else
		{	
			movement = Vector2.zero;	
		}
			if (movement!= Vector2.zero)
			{
				animator.SetFloat("Horizontal",(movement.x));
				animator.SetFloat("Vertical", (movement.y));
				
			}

			animator.SetFloat("Speed", movement.sqrMagnitude);
        // animatorBottom.SetBool("Aim", isShooting);

        // animatorTop.SetFloat("MoveHorizontal", Mathf.Abs(movement.x));
        // animatorTop.SetFloat("MoveVertical", Mathf.Abs(movement.y));
        // animatorTop.SetFloat("MoveMagnitude", movement.magnitude);
        // animatorTop.SetBool("Aim", isShooting);


        // //Rotate the player based on direction pointing - its more natural

		// if (isShooting && hasEnemy)
		// {
		// 	if (playercontroller.nearestInteractable != null)
		// 	{
		// 		//face enemy		
		// 		Vector2 distance = this.transform.position - playercontroller.nearestInteractable.transform.position;
		
		// 		//Normalize the vector x + y + z = 1
		// 		distance.Normalize();// rotate based on distance  from player to enemy
				
		// 		//find the angle in degrees
		// 		float rotZ = Mathf.Abs(Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg);
				
		// 		if(((rotZ > 90f) && !m_FacingRight) || ((rotZ <= 90f) && m_FacingRight)) 
		// 		{	
		// 			FlipTop();
		// 			StartCoroutine ( RotateReset() );						
		// 		} 					
		// 	}
		// }
		// else
		// {
		// 	//Normalize the vector x + y + z = 1
		// 	movement.Normalize();
			
		// 	//find the angle in degrees
		// 	float rotZ = Mathf.Abs(Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg);
			
		// 	if(((rotZ <= 90f) && !m_FacingRight) || ((rotZ > 90f) && m_FacingRight)) 
		// 	{
		// 		if (movement != Vector2.zero)
		// 		{
		// 			//face right
		// 			Flip();
		// 			if (!m_RightSideUp || m_RightSideUp) 
		// 			{
		// 				//invertArm();
		// 			}
		// 		}
		// 	} 
		// }        
	}

	private void FixedUpdate() 
	{
		rb.MovePosition(rb.position + movement * runspeed* Time.fixedDeltaTime);
	}

	public void stopMovement()
	{
		animator.enabled = false;
		movement= Vector2.zero;
		this.enabled=false;
	}
	
	IEnumerator RotateReset() 
	{
		yield return new WaitForSeconds (playercontroller.timeShoot);
		FlipTop();
	}


	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
        playerGraphicsTop.transform.Rotate(0f, 180f, 0f);		
        playerGraphicsBottom.transform.Rotate(0f, 180f, 0f);

        // Multiply the player's x local scale by -1.
        /* Vector3 theScale = playerGraphics.localScale;
		theScale.x *= -1;
		playerGraphics.localScale = theScale;*/		
	}

	public void FlipTop()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
        playerGraphicsTop.transform.Rotate(0f, 180f, 0f);
	}

	public void FlipBot()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
        playerGraphicsBottom.transform.Rotate(0f, 180f, 0f);		
	}

	private void invertArm() {
		//switch the way the arm is labeled as facing
		m_RightSideUp = !m_RightSideUp;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = playerArm.localScale;
		theScale.y *= -1;
		playerArm.localScale = theScale;
	}
}