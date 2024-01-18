using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private CapsuleCollider2D capsuleCollider;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float gravityScaleAtStart;
    [SerializeField]
    private float fallGravityMultiplyer;

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float cutJumpForce;
    [SerializeField]
    private float jumpPressedRememberTime = 0.2f;
    [SerializeField]
    private float jumpPressedRemember;
    [SerializeField]
    private int startJumpsAmount;
    [SerializeField]
    private int bonusJumpsLeft;


    [SerializeField]
    private float wasOnTheGroundRememberTime = 0.2f;
    [SerializeField]
    private float wasOnTheGroundRemember;

    [SerializeField]
    private float climbSpeed;
    [SerializeField]
    private Animator animator;

    

    void Start()
    {
        gravityScaleAtStart = rb.gravityScale;
    }


    void Update()
    {
        Move();
        Jump();
        ClimbLadder();
        FlipSprite();
        AnimateJump();

    }

    //Work on the movement
    public void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        bool playerHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        animator.SetBool("isRunning", playerHorizontalSpeed);
    }

    public void Jump()
    {
        //Set the timer
        jumpPressedRemember -= Time.deltaTime;
        wasOnTheGroundRemember -= Time.deltaTime;

        //Assign timer to the remember time
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            wasOnTheGroundRemember = wasOnTheGroundRememberTime;
            bonusJumpsLeft = startJumpsAmount;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }

        //Cut Jump height when Jump wans't pressed completely
        if (Input.GetButtonUp("Jump")) {
            if (rb.velocity.y > 0) {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * cutJumpForce);
            }
            
        }

        //If time since Jump button was pressed is bigger then 0 still do the jump

        //This creates Double Jump NEED TO FIX!!
        if ((jumpPressedRemember > 0) && (wasOnTheGroundRemember > 0))
        {
            jumpPressedRemember = 0;
            wasOnTheGroundRemember = 0;
            rb.velocity += new Vector2(rb.velocity.x, jumpForce);
           
            
        }
        //Double Jump
        //LOOKS LIKE OVERRIDING MB FIX LATER ?
        else if (Input.GetButtonDown("Jump") && (bonusJumpsLeft > 0)) 
        {
            bonusJumpsLeft--;
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", true);
            rb.velocity += new Vector2(rb.velocity.x, jumpForce);
        }
        

        else { return; }
    }

    //Create Walking state in Animation 

    public void ClimbLadder()
    {
        float verticalInput = Input.GetAxis("Vertical");
        bool playerVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;

        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
            rb.gravityScale = 0f;
            //Play animation only if player has pressed up or down
            //Doesn't work when player jump on to the ladder
            if (verticalInput != 0 || !capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                animator.SetBool("isClimbing", playerVerticalSpeed);
            }
            
        }

        else
        {
            rb.gravityScale = gravityScaleAtStart;
            animator.SetBool("isClimbing", false);
            
        }
    }

    public void FlipSprite() {

        bool playerHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }

    }

    /// <summary>
    /// Work on the animation state in code 
    /// </summary>
    
    //Very simple checker, needs to be enhanced
    public void AnimateJump() 
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && 
            !capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", true);
           

        }
        else 
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
        }
    }
}
//Fix when can't jump under the ladder
//Fix when getting off the ladder on higher ground switches animation to jumping
//Redisign code to make it more readable and functional