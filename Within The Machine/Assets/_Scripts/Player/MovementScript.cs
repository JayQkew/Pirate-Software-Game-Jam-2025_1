using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;


public class MovementScript : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    public InputMaster controls;
    private Vector2 InputVector;
    
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
        Vector2 InputVector = controls.Player.Movement.ReadValue<Vector2>();
        if (InputVector != Vector2.zero)
        {
            MovePlayer(controls.Player.Movement.ReadValue<Vector2>());
        }
        

        if (controls.Player.Movement.IsPressed())
        {
            Debug.Log("Pressed");
        }
    }

    void FixedUpdate()
    {
        
    }
    
    // Moves the Player
    private void MovePlayer(Vector2 Direction)
    {
        rb.velocity = new Vector2(Direction.x * movementSpeed, Direction.y * movementSpeed);
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
