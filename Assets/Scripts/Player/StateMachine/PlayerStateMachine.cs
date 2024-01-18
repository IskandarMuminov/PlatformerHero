using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speed;
    [SerializeField]
    private CapsuleCollider2D capsuleCollider;
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


    PlayerBaseState currentSate;
    PlayerStateFactory states;
    


    //Getters and Setters
    public PlayerBaseState CurrentSate { get { return currentSate; } set { currentSate = value; } }

    public Rigidbody2D Rb { get { return rb; } }
    public GameObject Player { get { return player; } }
    public Animator Animator { get { return animator; } }
    public float Speed { get { return speed; } }
    public CapsuleCollider2D CapsuleCollider { get { return capsuleCollider; } }
    public float GravityScaleAtStart { get { return gravityScaleAtStart; } }
    public float FallGravityMultipluer { get { return fallGravityMultiplyer; } }

    public float JumpForce { get { return jumpForce; } }
    public float CutJumpForce { get { return cutJumpForce; } }
    public float JumpPressedRememberTime { get { return jumpPressedRememberTime; } set { jumpPressedRememberTime = value; } }
    public float JumpPressedRemember { get { return jumpPressedRemember; } set { jumpPressedRemember = value; } }
    public int StartJumpsAmount { get { return startJumpsAmount; } set { startJumpsAmount = value; } }
    public int BonusJumpsLeft { get { return bonusJumpsLeft; } set { bonusJumpsLeft = value; } }

    public float WasOnTheGroundRememberTime { get { return wasOnTheGroundRememberTime; } set { wasOnTheGroundRememberTime = value; } }
    public float WasOnTheGroundRemember { get {return wasOnTheGroundRemember; } set { wasOnTheGroundRemember = value; } }   



    private void Awake()
    {
        states = new PlayerStateFactory(this);
        currentSate = states.Grounded();
        currentSate.EnterState();
    }

    void Start()
    {
        gravityScaleAtStart = rb.gravityScale;
    }

    
    void Update()
    {
        UpdateTimers();
        Move();
        currentSate.UpdateState();
        ClimbLadder();
        FlipSprite();
        //AnimateJump();
    }

    public void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        bool playerHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        animator.SetBool("isRunning", playerHorizontalSpeed);
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

    public void FlipSprite()
    {

        bool playerHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (playerHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }

    }

    /// <summary>
    /// Work on the animation state in code 
    /// </summary>

  /*  //Very simple checker, needs to be enhanced
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
    }*/

    public void UpdateTimers() {
        //Start timers
        jumpPressedRemember -= Time.deltaTime;
        wasOnTheGroundRemember -= Time.deltaTime;
    }
    
}