using System.Collections;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 8f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 2f;
    public float crouchSpeed = 2f;
    public LayerMask groundLayer;
    public Animator animator;

    private bool isGrounded;
    private bool isCrouching;
    private float originalHeight;
    private float movementSpeed;
    private Vector3 moveDirection;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
        movementSpeed = walkSpeed;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleCrouch();
        HandleKick();
        HandleGrabAndSwing();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * moveZ;
        Vector3 right = transform.right * moveX;
        moveDirection = (forward + right).normalized;
        moveDirection *= movementSpeed;

        // Apply movement to the CharacterController
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            moveDirection.y = jumpForce;
        }

        // Apply gravity
        moveDirection.y += Physics.gravity.y * Time.deltaTime;

        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, characterController.height / 2 + 0.1f, groundLayer);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isCrouching)
            {
                // Stand up
                characterController.height = standingHeight;
                movementSpeed = walkSpeed;
                isCrouching = false;
            }
            else
            {
                // Crouch
                characterController.height = crouchHeight;
                movementSpeed = crouchSpeed;
                isCrouching = true;
            }
        }
    }

    void HandleKick()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Kick");
        }
    }

    void HandleGrabAndSwing()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(transform.forward * 500f); // Adjust the force as needed for swinging effect
                    animator.SetTrigger("GrabAndSwing");
                }
            }
        }
    }

    // Add more methods to handle other actions and gameplay mechanics as needed

    // Event called by animation to enable/disable collider for attack hit detection
    void EnableAttackCollider()
    {
        // Implement this method to enable the hit detection collider during the kick animation
        // You can use a trigger collider on the character's foot and check for collisions in another script
    }

    // Event called by animation to disable collider for attack hit detection
    void DisableAttackCollider()
    {
        // Implement this method to disable the hit detection collider when the kick animation ends
    }
}
