using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float x_movement;
    public float y_movement;
    public Vector3 target_position = new Vector3();



    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        Move();

    }
    // Update is called once per frame
    void Update()
    {
        GetInputs();

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
}
