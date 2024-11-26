using UnityEngine;
public class PCToggle : MonoBehaviour
{
    [SerializeField] GameObject pcOn;
    SpriteRenderer sprite;
    BoxCollider2D col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Player")
        {
            sprite.enabled = false;
            pcOn.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Player")
        {
            sprite.enabled = true;
            pcOn.SetActive(false);
        }
    }




}
