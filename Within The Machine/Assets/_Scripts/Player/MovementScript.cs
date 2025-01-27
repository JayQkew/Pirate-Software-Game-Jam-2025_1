using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;


public class MovementScript : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    public InputMaster controls;
    private Vector2 InputVector;
    [SerializeField] private bool lookingLeft = true;
    
    [Header("Ladder Movement")]
    public bool closeToLadder = false;
    [SerializeField] private float climbingSpeed = 4f;
    
    [Header("Jump Movement")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private bool canJump = true;
    [SerializeField] private float rayDist = 0.02f;
    [SerializeField] private int theLayer = 3;
    private int targetLayerJump;
    [SerializeField] private float FallGravity = 10f;
    [SerializeField] private RaycastHit2D hit;
    
    [Header("Dash Movement")]
    //Dash can be shortened or lengthened by changing the dashSpeed or the dashDuration
    [SerializeField] private float dashSpeed = 5f;
    [SerializeField] private bool canDash = true, isDashing = false;
    [SerializeField] private float dashCooldown = 1f, timerDashC;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float timerdashD;
    private Vector2 SaveVelocity;
    
    void Awake()
    {
       controls = new InputMaster(); 
       
       controls.Player.Movement.performed += contextMove => MovePlayer(contextMove.ReadValue<Vector2>());
       controls.Player.Jump.performed += cntJump => JumpPlayer();
       controls.Player.Dash.performed += cntDash => DashPlayer();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        targetLayerJump = 1 << theLayer;
    }

    // Update is called once per frame
    void Update()
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
        }
        //----------------------------------------------------------------------
        
        //Switches Gravity off if close to ladder
        if (closeToLadder == true)
        {
            rb.gravityScale = 0;
        }

        else
        {
            rb.gravityScale = 1;
        }
        //------------------------------------------------------------------------------
        
       // Ground check + changes gravity for less floaty jump
        Debug.DrawRay(transform.position, Vector2.down * rayDist, Color.magenta);
        hit = Physics2D.Raycast(transform.position, Vector2.down, rayDist, targetLayerJump);
        if (hit)
        {
            canJump = true;
            rb.gravityScale = 1;
        }
        else
        {
            canJump = false;
            rb.gravityScale = FallGravity;
        }
        //-------------------------------------------------------------------------
        
        //Checks if the Dash is done
        if (isDashing)
        {
            CalculateDashEnd(timerdashD);
        }
        
        //Resets dash
        if (!canDash)
        {
            ResetDashCooldown(timerdashD);
        }
        
        //Flips Character
        if (InputVector.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            lookingLeft = false;
        }
        
        else if (InputVector.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            lookingLeft = true;
        }
    }

    void FixedUpdate()
    {
        
    }
    
    // Moves the Player
    private void MovePlayer(Vector2 Direction)
    {
        if (Direction.x != 0 && !closeToLadder) //Left to right movement
        {
            rb.velocity = new Vector2(Direction.x * movementSpeed, rb.velocity.y);
        }
        
        else if (closeToLadder) //Movement on ladder
        {
            rb.velocity = new Vector2(Direction.x * movementSpeed, Direction.y * climbingSpeed); 
        }
    }
    
    // Makes the Player Jump
    private void JumpPlayer()
    {
       
        if (canJump)
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
    }
    
    //Makes the player Dash
    private void DashPlayer()
    {
        if (canDash)
        {
            SaveVelocity = rb.velocity;
            if (lookingLeft)
            {
                rb.AddForce(Vector2.left * dashSpeed, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);
            }
            
            canDash = false;
            timerdashD = Time.time;
            timerDashC = timerdashD;
            isDashing = true;
        }
    }

    //Determines how far the player can dash
    private void CalculateDashEnd(float timer)
    {
        if (Time.time - timer > dashDuration)
        {
            isDashing = false;
            rb.velocity = SaveVelocity;
        }
    }
    
    private void ResetDashCooldown(float timer)
    {
        if (Time.time - timer > dashCooldown)
        {
            canDash = true;
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
