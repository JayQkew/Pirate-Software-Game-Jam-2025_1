using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using FMODUnity;
using UnityEngine.EventSystems;


public class MovementScript : MonoBehaviour
{
    [Header("Movement Features")]
    [SerializeField] private bool DashOn = true;
    [SerializeField] private bool JumpOn = true;
    [SerializeField] private bool LadderOn = true;
    [SerializeField] private bool LadderSlideOn = true;
    
   
    [Header("Player Movement")]
    [SerializeField] private float movementSpeed = 220f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Sprite playerSprite; //Please drag in the player sprite
    public InputMaster controls;
    private Vector2 InputVector;
    [SerializeField] private bool lookingLeft = true;
    [SerializeField] private ParticlesController PC_Script;
    
    [Header("Ladder Movement")]
    public bool closeToLadder = false;
    [SerializeField] private float climbingSpeed = 120f;
    [SerializeField] private float downSpeed;
    [SerializeField] private float slideSpeed = 350f; //Speed for when sliding down ladder
    [SerializeField] private float timeTillSlide = 0.5f;
    [SerializeField] private float timerSlide;
    [SerializeField] private bool slideStarted = false;
    
    [Header("Jump Movement")]
    [SerializeField] private float jumpHeight = 17f;
    [SerializeField] private bool canJump = true, isJumping = false, isGrounded = false;
    [SerializeField] private float rayDist = 0.35f;
    [SerializeField] private int theLayer = 3;
    private int targetLayerJump;
    [SerializeField] private float FallGravity = 7f;
    [SerializeField] private RaycastHit2D hitLeft, hitRight;
    
    [Header("Dash Movement")]
    //Dash can be shortened or lengthened by changing the dashSpeed or the dashDuration
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private bool canDash = true, isDashing = false;
    [SerializeField] private float dashCooldown = 1f, timerDashC;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float timerdashD;
    private Vector2 SaveVelocity;
    [SerializeField] private TrailRenderer dashTrail;
    
    [Header("Sound Effects")]
    [SerializeField] protected FMODUnity.EventReference WalkingSound;

    [SerializeField] private bool canPlaySound = false;
    
    
    void Awake()
    {
       controls = new InputMaster();
       rb = gameObject.GetComponent<Rigidbody2D>();
       //playerSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
       
       controls.Player.Movement.performed += contextMove => MovePlayer(contextMove.ReadValue<Vector2>());
       controls.Player.Jump.performed += cntJump => JumpPlayer();
       controls.Player.Dash.performed += cntDash => DashPlayer();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        targetLayerJump = 1 << theLayer;
        rb.gravityScale = FallGravity; //Makes Jump less floaty
        dashTrail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Switches Gravity off if close to ladder
        if (LadderOn &&
            closeToLadder && 
            !isDashing)
        {
            rb.gravityScale = 0;
        }

        else if (LadderOn &&
            !closeToLadder)
        {
            rb.gravityScale = FallGravity; //Makes Jump less floaty
        }
        //------------------------------------------------------------------------------

        
        
       // Ground check
       Vector2 BottomLeft = new Vector2(transform.position.x - playerSprite.bounds.extents.x, transform.position.y);
       Vector2 BottomRight = new Vector2(transform.position.x + playerSprite.bounds.extents.x, transform.position.y);
       
       Debug.DrawRay(BottomLeft, Vector2.down * rayDist, Color.magenta);
       Debug.DrawRay(BottomRight, Vector2.down * rayDist, Color.magenta);
       hitLeft = Physics2D.Raycast(BottomLeft, Vector2.down, rayDist, targetLayerJump);
       hitRight = Physics2D.Raycast(BottomRight, Vector2.down, rayDist, targetLayerJump);
        if (hitLeft || hitRight)
        {
            canJump = true;
            isGrounded = true;
        }
        else if ((!hitLeft || !hitRight) &&
                 !closeToLadder)
        {
            canJump = false;
            isGrounded = false;
        }
        //-------------------------------------------------------------------------
        
        //Checks if the Dash is done
        if (isDashing)
        {
            CalculateDashEnd(timerdashD);
        }

        if (dashTrail.time <= 0)
        {
            dashTrail.enabled = false;
        }
        //----------------------------------------
        //Resets dash
        if (!canDash)
        {
            ResetDashCooldown(timerdashD);
        }
        //----------------------------------------------
    }

    void FixedUpdate()
    {
        //Checks if any input is given
        Vector2 InputVector = controls.Player.Movement.ReadValue<Vector2>();
        if (InputVector != Vector2.zero && 
            !isDashing)
        {
            MovePlayer(controls.Player.Movement.ReadValue<Vector2>());
            
        }
        
        else if (closeToLadder &&
                 InputVector == Vector2.zero) // Switches drag off when climbing ladders
        {
            rb.velocity = Vector2.zero;
            PC_Script.isRunning = false;
        }
        
        else if (InputVector == Vector2.zero)
        {
            PC_Script.isRunning = false;  
        }

        if (InputVector != Vector2.zero && !isGrounded)
        {
            PC_Script.isRunning = false;
        }
        //----------------------------------------------------------------------
        
        // Checks if the player moves down on the ladder
        if (slideStarted == true)
        {
            TimeTillSlide(timerSlide);
        }
        
        if (!closeToLadder ||
            InputVector.y >= 0)
        {
            downSpeed = climbingSpeed;
            slideStarted = false;
        }
        //----------------------------------------------------------------------
        
        //Flips Character
        if (InputVector.x > 0)
        {
           // transform.eulerAngles = new Vector3(0, 180, 0);
            lookingLeft = false;
        }
        
        else if (InputVector.x < 0)
        {
           // transform.eulerAngles = new Vector3(0, 0, 0);
            lookingLeft = true;
        }
    }
    
    
    // Moves the Player
    private void MovePlayer(Vector2 Direction)
    {
        if (Direction.x != 0 && (!closeToLadder || !LadderOn)) //Left to right movement
        {
            rb.velocity = new Vector2(Direction.x * movementSpeed * Time.fixedDeltaTime, rb.velocity.y);
            PlayWalkingSound();
            downSpeed = climbingSpeed;
            slideStarted = false;
            if (isGrounded)
            {
                PC_Script.isRunning = true;
            }
            else
            {
                PC_Script.isRunning = false;
            }
            
        }
        
        else if (LadderOn &&
                 closeToLadder) //Movement on ladder
        {
            PC_Script.isRunning = false;
            if (Direction.y >= 0 ||
                !LadderSlideOn) // For going up the ladder
            {
                downSpeed = climbingSpeed;
                slideStarted = false;
                rb.velocity = new Vector2(Direction.x * movementSpeed * Time.fixedDeltaTime, Direction.y * climbingSpeed * Time.fixedDeltaTime); 
            }

            else if (LadderSlideOn &&
                     Direction.y < 0)  //Allows the player to slide down the ladder-faster movement when going down ladder
            {
                    if (slideStarted == false)
                    {
                        timerSlide = Time.time;
                        slideStarted = true;
                    }
                    rb.velocity = new Vector2(Direction.x * movementSpeed * Time.fixedDeltaTime, Direction.y * downSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void PlayWalkingSound()
    {
        if (canPlaySound == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot(WalkingSound);
        }
       
    }

   // Delay before the slide starts
    private void TimeTillSlide(float timer)
    {
        if (Time.time - timer > timeTillSlide)
        {
            downSpeed = slideSpeed;
        }
    }
    // Makes the Player Jump
    private void JumpPlayer()
    {
        if (JumpOn &&
            canJump)
        {
            PC_Script.isRunning = false;
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            isJumping = true;
        }
    }
    
    //Makes the player Dash
    private void DashPlayer()
    {
        if (DashOn)
        {
            PC_Script.isRunning = false;
            isDashing = true;
            if (canDash)
            {
                isDashing = true;
                SaveVelocity = new Vector2(rb.velocity.x, -FallGravity);
                if (lookingLeft) //Player dashes to the left
                {
                    if (isGrounded)
                    {
                        PC_Script.DashLeft();
                    }
                    rb.AddForce(Vector2.left * dashSpeed, ForceMode2D.Impulse);
                    dashTrail.enabled = true;
                }
                else //Player dashes to the right
                {
                    if (isGrounded)
                    {
                        PC_Script.DashRight();
                    }
                    rb.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);
                    dashTrail.enabled = true;
                }
            
                canDash = false;
                timerdashD = Time.time;
                timerDashC = timerdashD;
            }
            else
            {
                isDashing = false;
            }
        }
    }

    //Determines how far the player can dash
    private void CalculateDashEnd(float timer)
    {
        if (Time.time - timer > dashDuration)
        {
            isDashing = false;
            rb.velocity = SaveVelocity;
            dashTrail.enabled = false;
        }
    }
    
    private void ResetDashCooldown(float timer)
    {
        if (Time.time - timer > dashCooldown)
        {
            canDash = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == theLayer)
        {
                isGrounded = true;
                PC_Script.Landed();
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
