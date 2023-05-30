using UnityEngine;

public class SwitchMode : MonoBehaviour
{
    [SerializeField]
    public Light targetLight;
    public float lightReductionSpeed = 32f;
    public float lightIncreaseSpeed = 32f;    
    public float colliderReductionSpeed = 10f;
    public float colliderIncreaseSpeed = 10f;

    private float initialRange;
    private float initialRadius;
    private bool isLightOn = true;
    private float targetRange;

    [SerializeField]
    private SphereCollider tremorRange;

    private void Start()
    {
        // Store the initial range of the light
        initialRange = targetLight.range;
        initialRadius = 10f;
        tremorRange = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        // Check if the "E" key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Toggle the light on/off
            isLightOn = !isLightOn;

            // Adjust the target range based on the light state
            if (isLightOn == false)
            {
                Debug.Log("Light off");
                StartCoroutine(DecreaseLightRange());
            }
            else
            {
                Debug.Log("Light on");
                StartCoroutine(IncreaseLightRange());
            }
        }

    }

    public bool IsStealth()
    {
        return !isLightOn;
    }

    private System.Collections.IEnumerator DecreaseLightRange()
    {
        // Gradually decrease the light range
        while (targetLight.range > 0f)
        {
            targetLight.range -= lightReductionSpeed * Time.deltaTime;
            tremorRange.radius += colliderIncreaseSpeed * Time.deltaTime;
            yield return null;
        }

        // Ensure the light range is not negative
        targetLight.range = Mathf.Max(targetLight.range, 0f);
        tremorRange.radius = Mathf.Min(tremorRange.radius, initialRadius);        
    }

    private System.Collections.IEnumerator IncreaseLightRange()
    {
        // Gradually increase the light range
        while (targetLight.range < initialRange)
        {
            targetLight.range += lightReductionSpeed * Time.deltaTime;
            tremorRange.radius -= colliderReductionSpeed * Time.deltaTime;
            yield return null;
        }

        // Ensure the light range is not exceed max range
        targetLight.range = Mathf.Min(targetLight.range, initialRange);
        tremorRange.radius = Mathf.Max(tremorRange.radius, 0f);
    }
}

