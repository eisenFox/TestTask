using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{

    [Header("Referencess")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;

    [Header("Move Parameters")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float gravity = -20f;

    [Header("Interactions")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private float holdDistance = 1f;
    [SerializeField] private float moveSpeedHold = 5f;

    private Pickable currentObject;
    private Rigidbody currentRb;

    private float cameraPitch;
    private Vector3 velocity;

    bool IsVRActive()
    {
        return XRSettings.isDeviceActive;
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        if (!IsVRActive())
        {
            Look();
        }
        Move();
        Interact();
    }

    void Look()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void Move()
    {
        float x, z;

        if (IsVRActive())
        {
            x = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickHorizontal");
            z = Input.GetAxis("Oculus_CrossPlatform_PrimaryThumbstickVertical");
        }
        else
        {
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
        }

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void Interact()
    {
        Ray ray;
        if (IsVRActive())
        {
            ray = new Ray(playerCamera.position, playerCamera.forward);
        }
        else
        {
            ray = playerCamera.GetComponent<Camera>()
                .ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }

        RaycastHit hit;
        
        bool interactPressed = false;
        bool interactReleased = false;

        if (IsVRActive())
        {
            float trigger = Input.GetAxis("Oculus_CrossPlatform_Trigger");

            interactPressed = trigger > 0.8f;
            interactReleased = trigger <= 0.2f;
        }
        else
        {
            interactPressed = Input.GetMouseButtonDown(0);
            interactReleased = Input.GetMouseButtonUp(0);
        }
        
        
        // Picking up object and Interacting with them
        
        
        
        
        if (interactPressed)
        {
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                if (hit.collider.TryGetComponent(out Door door))
                {
                    door.ToggleDoor();
                    return;
                }
                if (hit.collider.TryGetComponent(out KnobAndStove knob))
                {
                    knob.ToggleKnob();
                    return;
                }
            }

            if (currentObject == null)
            {
                if (Physics.Raycast(ray, out hit, interactDistance))
                {
                    if (hit.collider.TryGetComponent(out Pickable pickable))
                    {
                        currentObject = pickable;
                        currentRb = pickable.rb;

                        currentRb.useGravity = false;
                        currentRb.linearVelocity = Vector3.zero;
                        currentRb.angularVelocity = Vector3.zero;
                    }
                }
            }
        }
        // Throwing object
        if (interactReleased)
        {
            if (currentRb != null)
            {
                currentRb.useGravity = true;
            }

            currentObject = null;
            currentRb = null;
        }
        // Holding object
        if (currentObject != null && currentRb != null)
        {
            Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;

            Vector3 direction = targetPos - currentRb.position;

            currentRb.linearVelocity = direction * moveSpeedHold;
            
            Quaternion targetRot = Quaternion.Euler(270f, playerCamera.eulerAngles.y, 180f);
            currentRb.MoveRotation(targetRot);

        }
    }
}