using System.Collections;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 3f;   
    public float sprintSpeed = 8f;
    public float originalSpeed = 5f;
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    [SerializeField] private float interactRange = 3f;

    Transform  playerCamera;
    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;
    bool isCrouching = false;

    bool wasGroundedLastFrame = true;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = transform.Find("HeadCamera");
        originalSpeed = speed;
        Cursor.lockState = CursorLockMode.Locked;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        wasGroundedLastFrame = isGrounded;
        isGrounded = controller.isGrounded;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching){
            speed = sprintSpeed;
        }
        else if (!isCrouching)
            speed = originalSpeed;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            speed = isCrouching ? crouchSpeed : originalSpeed;
            StopAllCoroutines();
            StartCoroutine(SmoothCrouch(isCrouching));
        }


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //Debug.Log("JUMP INPUT ✅");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (!isGrounded && wasGroundedLastFrame)
        {
            //Debug.Log("JUMP LIFTED OFF ✅");
        }

        if (isGrounded && !wasGroundedLastFrame)
        {
            //Debug.Log("LANDED ✅");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void TryInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactRange))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
    IEnumerator SmoothCrouch(bool isCrouching)
    {
        float startHeight = controller.height;
        float startCenter = controller.center.y;
        float startCamera = playerCamera.localPosition.y;
        float t = 0f;
        float targetHeight = isCrouching? standHeight : crouchHeight;
        while (Mathf.Abs(controller.height - targetHeight) > 0.01f)
        {
            t += Time.deltaTime * 5f;
            controller.height = Mathf.Lerp(startHeight, targetHeight, t);
            controller.center = new Vector3(0, Mathf.Lerp(startCenter, targetHeight/2, t), 0);
            playerCamera.localPosition = new Vector3(
                                            playerCamera.localPosition.x,
                                            Mathf.Lerp(startCamera, isCrouching ? 1.8f : 0.8f, t),
                                            playerCamera.localPosition.z);
            yield return null;
        }
        controller.height = targetHeight;
    }

}
