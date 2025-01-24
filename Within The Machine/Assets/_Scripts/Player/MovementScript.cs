using System.Collections;
using System.Collections.Generic;
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
    
    [Header("Ladder Movement")]
    public bool closeToLadder = false;
    [SerializeField] private float climbingSpeed = 4f;
    
    void Awake()
    {
       controls = new InputMaster(); 
       
       controls.Player.Movement.performed += context => MovePlayer(context.ReadValue<Vector2>());
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       //Checks if any input is given
        Vector2 InputVector = controls.Player.Movement.ReadValue<Vector2>();
        if (InputVector != Vector2.zero)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (closeToLadder)
            {
                closeToLadder = false;
            }
            else
            {
                closeToLadder = true;
            }
        }
    }

    void FixedUpdate()
    {
        
    }
    
    // Moves the Player
    private void MovePlayer(Vector2 Direction)
    {
       // rb.velocity = new Vector2(Direction.x * movementSpeed, Direction.y * movementSpeed);

        if (Direction.x != 0 && !closeToLadder) //Left to right movement
        {
            rb.velocity = new Vector2(Direction.x * movementSpeed, -1);
        }
        
        else if (closeToLadder) //Movement on ladder
        {
            rb.velocity = new Vector2(Direction.x * movementSpeed, Direction.y * climbingSpeed); 
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
