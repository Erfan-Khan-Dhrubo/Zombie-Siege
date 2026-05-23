using UnityEngine; // This imports Unity’s built-in classes like: MonoBehaviour, Transform, Input, Quaternion, Mathf
using UnityEngine.InputSystem; //Imports the NEW Input System classes.

public class mouseMovement : MonoBehaviour
{
    // Controls how fast the camera rotates when moving the mouse.
    public float mouseSensitivity = 0.05f;

    // This stores the generated Input System class.
    private PlayerControls _playerControls;

    // Stores mouse movement data.
    private Vector2 _lookInput;

    private float _xRotation = 0f; // Stores vertical camera rotation
    private float _yRotation = 0f; // Stores horizontal camera rotation

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    private void Awake()
    {
        // Creates an object from the generated input class.
        _playerControls = new PlayerControls();
    }

    // Unity automatically calls this:
    //      when GameObject becomes active
    //      when script becomes enabled
    private void OnEnable()
    {
        _playerControls.Enable();
    }

    // Called automatically when:
    //      GameObject disabled
    //      script disabled
    //      object destroyed
    private void OnDisable()
    {
        _playerControls.Disable();
    }

    void Start()
    {
        // Lock the mouse cursor to the center of the screen and hide it.
        // Even though the cursor does not move visually, Unity still measures mouse movement.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        _lookInput = _playerControls.Player.Look.ReadValue<Vector2>();

        float mouseX = _lookInput.x * mouseSensitivity;
        //Time.deltaTime Makes movement frame-rate independent. Without this: Faster FPS = faster turning
        float mouseY = _lookInput.y * mouseSensitivity;

        // This changes up/down viewing.
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, topClamp, bottomClamp); // Limits vertical rotation.

        // Stores left/right rotation.
        _yRotation += mouseX;

        // It is the line that actually makes the camera/player look around
        transform.localRotation =
            Quaternion.Euler(_xRotation, _yRotation, 0f);
        // localRotation Rotation relative to parent object.
        // Quaternion.Euler(x, y, z)
        // X = up/down tilt
        // Y = left/right turn
        // Z = sideways tilt
    }
}