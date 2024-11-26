using System.Collections.Generic;
using UnityEngine;

public class ForegroundShifter : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject objectToShift;
    [SerializeField] List<BoxCollider2D> aboveColliders;
    [SerializeField] List<BoxCollider2D> belowColliders;

    //[Header("Toggle Colliders")]
    //public bool collidersActiveWhenPlayerAbove = true;
    //public bool collidersActiveWhenPlayerBelow = true;







    public Vector3 direction = Vector3.zero;
    public Vector3 playerAbove = new Vector3();
    public Vector3 playerBelow = new Vector3();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDirection();
        SwapRender();
    }

    void CalculateDirection()
    {
        direction = player.transform.position - transform.position;
        Debug.DrawRay(transform.position, direction, Color.green);
    }

    void SwapRender()
    {

        if (direction.y < 0)
        {
        objectToShift.transform.position = playerBelow;
            foreach (var collider in belowColliders)
            {
                collider.enabled = true;
            }
            foreach (var collider in aboveColliders)
            {
                collider.enabled = false;
            }
        }
        if (direction.y > 0)
        {
        objectToShift.transform.position = playerAbove;
            foreach (var collider in belowColliders)
            {
                collider.enabled = false;
            }
            foreach (var collider in aboveColliders)
            {
                collider.enabled = true;
            }
        }
    }

}
