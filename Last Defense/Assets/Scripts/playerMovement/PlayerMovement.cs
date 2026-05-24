using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Stores reference to the Character Controller component.
    private CharacterController _characterController;

    // Stores your generated Input System class.
    private PlayerControls _controls;

    [Header("Movement")] public float speed = 12f;
    public float gravity = -9.81f * 2; // -ve = downward
    public float jumpHeight = 3f;

    [Header("Ground Check")] public Transform groundCheck; // Reference point used to detect the ground
    public float groundDistance = 0.4f; // Radius of the ground-check sphere.
    public LayerMask groundMask; // Specifies what layers count as ground.

    // Stores movement velocity.
    private Vector3 _velocity;

    private bool _isGrounded; // Is player touching ground?
    private bool _isMoving; // Is player currently moving?

    // Stores previous frame position.
    private Vector3 _lastPosition = Vector3.zero;

    // Stores WASD input.
    private Vector2 _moveInput;

    private void Awake()
    {
        // Create input system object
        _controls = new PlayerControls();
    }

    // Called when object becomes enabled
    private void OnEnable()
    {
        // Enable all input actions
        _controls.Enable();
    }

    // Called when object becomes disabled
    private void OnDisable()
    {
        // Disable all input actions
        _controls.Disable();
    }

    // Start is called once before the first frame update
    void Start()
    {
        // Get CharacterController component
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Is this sphere touching any object in the Ground layer?
        _isGrounded = Physics.CheckSphere( // Creates an invisible sphere in the world.
            groundCheck.position, // parameter - Sphere Position, Sphere Radius, Layer Mask (Check if the sphere is touched by this layer)
            groundDistance,
            groundMask
        );

        // If the player is on the ground and currently falling downward, stop the falling speed
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        // READ MOVEMENT INPUT
        _moveInput = _controls.Player.Move.ReadValue<Vector2>();

        float x = _moveInput.x;
        float z = _moveInput.y;

        // Convert local movement into world direction
        Vector3 move =
            transform.right * x +   // transform.right - Player's local RIGHT direction
            transform.forward * z;  // transform.forward - Player's local FORWARD direction

        // Move player horizontally
        _characterController.Move(
            move * speed * Time.deltaTime
        );

        // If the jump button was pressed and the player is on the ground, give the player upward velocity.
        if (_controls.Player.Jump.triggered && _isGrounded)
        {
            _velocity.y =
                Mathf.Sqrt(jumpHeight * -2f * gravity); // v = √(h × -2 × g) This is a real physics equation used to calculate jump velocity
        }

        // GRAVITY
        _velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement
        _characterController.Move(
            _velocity * Time.deltaTime
        );

        // MOVEMENT CHECK
        if (_lastPosition != transform.position && _isGrounded)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }

        // Save current position
        _lastPosition = transform.position;
    }
}