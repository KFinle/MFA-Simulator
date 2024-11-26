using UnityEngine;

public class CountdownBar : MonoBehaviour
{
    [SerializeField] GameObject countdownBarVisual;
    public bool countdown = false;
    public float time_remaining;
    public float max_time = 20;

    Vector3 initial_scale;

    private void Start()
    {
        time_remaining = max_time;
        initial_scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (time_remaining > 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0,transform.localScale.y, transform.localScale.z), Time.deltaTime/time_remaining);
            time_remaining -= Time.deltaTime;
        }
        if (time_remaining < 0)
        {
            time_remaining = max_time;
            countdown = false;
            transform.localScale = initial_scale;
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        countdown = true;
    }

}
