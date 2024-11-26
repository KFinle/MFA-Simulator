using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float x_movement;
    public float y_movement;
    public Vector3 target_position = new Vector3();
    private bool isInteracting = false; // Track if the player is interacting
    public float interactionDistance = 3f; // Distance for interaction
    [SerializeField] IInteractable interactableObject; // Reference to the IInteractable object

    void Start()
    {
        interactableObject = null; // No interactable object initially
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        GetInputs();
        CheckForInteractable();
        HandleInteraction();
    }

    void Move()
    {
        target_position.x = transform.position.x + x_movement * walkSpeed * Time.deltaTime;
        target_position.y = transform.position.y + y_movement * walkSpeed * Time.deltaTime;
        transform.position = target_position;
    }

    void GetInputs()
    {
        x_movement = Input.GetAxisRaw("Horizontal");
        y_movement = Input.GetAxisRaw("Vertical");
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
        LevelManager levelManager = FindObjectOfType<LevelManager>(); // Find the LevelManager in the scene
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
}