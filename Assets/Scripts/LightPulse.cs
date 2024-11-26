using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPulse : MonoBehaviour
{
    [SerializeField] Light2D light;
    [SerializeField] float pulseIntensity;
    [SerializeField] float pulseSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            light.falloffIntensity = 1 - Mathf.PingPong(pulseSpeed * Time.time, pulseIntensity);
        }
    }


}
