using UnityEngine; // This imports Unity’s built-in classes like: MonoBehaviour, Transform, Input, Quaternion, Mathf
using UnityEngine.InputSystem; //Imports the NEW Input System classes.

public class mouseMovement : MonoBehaviour
{
    // Controls how fast the camera rotates when moving the mouse.
    public float mouseSensitivity = 0.05f;

    // This stores the generated Input System class.
    private PlayerControls controls;

    // Stores mouse movement data.
    private Vector2 lookInput;

    float xRotation = 0f;   // Stores vertical camera rotation
    float yRotation = 0f;   // Stores horizontal camera rotation
    
    public float topClamp = -90f;
    public float bottomClamp = 90f;

    private void Awake()
    {
        // Creates an object from the generated input class.
        controls = new PlayerControls();
    }

    // Unity automatically calls this:
    //      when GameObject becomes active
    //      when script becomes enabled
    private void OnEnable()
    {
        controls.Enable();
    }

    // Called automatically when:
    //      GameObject disabled
    //      script disabled
    //      object destroyed
    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        // Lock the mouse cursor to the center of the screen and hide it.
        // Even though the cursor does not move visually, Unity still measures mouse movement.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        lookInput = controls.Player.Look.ReadValue<Vector2>();

        // Input.GetAxis("Mouse X")
        // Returns: Positive → mouse moved right
        //          Negative → mouse moved left
        float mouseX = lookInput.x * mouseSensitivity;  //Time.deltaTime Makes movement frame-rate independent. Without this: Faster FPS = faster turning
        float mouseY = lookInput.y * mouseSensitivity;

        // This changes up/down viewing.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp); // Limits vertical rotation.

        // Stores left/right rotation.
        yRotation += mouseX;

        // It is the line that actually makes the camera/player look around
        transform.localRotation =
            Quaternion.Euler(xRotation, yRotation, 0f);
            // localRotation Rotation relative to parent object.
            // Quaternion.Euler(x, y, z)
            // X = up/down tilt
            // Y = left/right turn
            // Z = sideways tilt
    }
}