using UnityEngine;

public class SwitchMode : MonoBehaviour
{
    [SerializeField]
    public Light targetLight;
    public float reductionSpeed = 32f;
    public float increaseSpeed = 32f;

    private float initialRange;
    private bool isLightOn = true;
    private float targetRange;

    private void Start()
    {
        // Store the initial range of the light
        initialRange = targetLight.range;
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
    private System.Collections.IEnumerator DecreaseLightRange()
    {
        // Gradually decrease the light range
        while (targetLight.range > 0f)
        {
            targetLight.range -= reductionSpeed * Time.deltaTime;
            yield return null;
        }

        // Ensure the light range is not negative
        targetLight.range = Mathf.Max(targetLight.range, 0f);
    }

    private System.Collections.IEnumerator IncreaseLightRange()
    {
        // Gradually increase the light range
        while (targetLight.range < initialRange)
        {
            targetLight.range += reductionSpeed * Time.deltaTime;
            yield return null;
        }

        // Ensure the light range is not exceed max range
        targetLight.range = Mathf.Min(targetLight.range, initialRange);
    }
}

