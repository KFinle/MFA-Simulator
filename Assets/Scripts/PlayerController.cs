using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float x_movement;
    public float y_movement;
    private Vector2 movementVector = new Vector2();
    public Vector3 target_position = new Vector3();
    [HideInInspector] public bool isInteracting = false; // Track if the player is interacting
    public float interactionDistance = 3f; // Distance for interaction
    public IInteractable interactableObject; // Reference to the IInteractable object
    
    public Transform spawnPoint;
    LevelManager levelManager;
    PlayerDirection playerDirection = PlayerDirection.Down;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        levelManager = FindObjectOfType<LevelManager>(); // Find the LevelManager in the scene
    }
    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        gameObject.transform.position = spawnPoint.position;

        isInteracting = false;
        interactableObject = null; // No interactable object initially
    }
    private void FixedUpdate()
    {
        if (!isInteracting && !IsInDialogue()) Move();
    }

    void Update()
    {
        GetInputs();
        Flip();
        Animate();
        CheckForInteractable();
        HandleInteraction();
    }

    void Move()
    {
        target_position.x = transform.position.x + x_movement * walkSpeed * Time.deltaTime;
        target_position.y = transform.position.y + y_movement * walkSpeed * Time.deltaTime;
        transform.position = target_position;
    }

    void Animate()
    {
            animator.SetFloat("xVec", Math.Abs(x_movement));
            animator.SetFloat("yVec", y_movement);
    }

    void Flip()
    {
        if (movementVector.x > 0 )
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, 1);
        }
        if (movementVector.x < 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, 1);
        }
    }
    void GetInputs()
    {
        x_movement = Input.GetAxisRaw("Horizontal");
        y_movement = Input.GetAxisRaw("Vertical");
        movementVector = new Vector2(x_movement, y_movement);
    }

    void CheckForInteractable()
    {
        // Check for interactable objects within range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionDistance);

        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactableObject = interactable; // Set the interactable object
                break; // Only pick the first interactable object found
            }
            else
            {
                interactableObject = null; // No interactable object found
            }
        }
    }

    void HandleInteraction()
    {
        
        if (interactableObject != null && Input.GetKeyDown(KeyCode.E)) // Press 'E' to interact
        {
            // Trigger the interaction and pass the LevelManager to control canvas switching
            if (levelManager != null)
            {
                isInteracting = true;
                interactableObject.Interact(levelManager);
            }
        }
        // Check for interaction cancellation if the player is currently interacting
        if (isInteracting && Input.GetKeyDown(KeyCode.Q))
        {
            isInteracting = false;
            interactableObject.CancelInteraction(levelManager); // Cancel the interaction when Escape is pressed
        }
    }

    // Gizmos method to draw the interaction range as a 2D circle in the Scene view
    private void OnDrawGizmosSelected()
    {
        // Set Gizmo color (optional, for clarity)
        Gizmos.color = Color.green;

        // Draw the interaction range as a 2D circle around the player
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
    
    bool IsInDialogue()
    {
        return levelManager.dialogueController.inDialogue;
    }
}