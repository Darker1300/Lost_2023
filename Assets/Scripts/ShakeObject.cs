using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public AnimationCurve shakeCurve
        = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    public float maxShakeIntensity = 0.333f;  // Maximum shake intensity
    private Vector2 originalPosition;
    private bool isShaking = false;
    private bool isEasingOut = false;
    private bool hasResetPosition = true;  // New variable to check if the position has been reset
    private float shakeTime = 0f;
    private float easeOutTime = 0f;
    public float easeOutDuration = 0.25f;  // Duration of the ease-out effect

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (isShaking || isEasingOut)
        {
            hasResetPosition = false;  // Object has moved, so it needs to be reset later
            Shake();
        }
        else if (!hasResetPosition)
        {
            // Reset the position only once when shaking and easing out are both finished
            transform.localPosition = originalPosition;
            hasResetPosition = true;  // Position has been reset
        }
    }

    public void StartShake()
    {
        isShaking = true;
        isEasingOut = false;
        shakeTime = 0f;  // Reset shake time when starting to shake

        //Debug.Log("START SHAKING");
    }

    public void StopShake()
    {
        isShaking = false;
        isEasingOut = true;  // Begin the ease out phase
        easeOutTime = 0f;  // Reset ease out time

        //Debug.Log("STOP SHAKING");
    }

    private void Shake()
    {
        float shakeIntensity;

        if (isShaking)
        {
            shakeIntensity = shakeCurve.Evaluate(shakeTime);
            shakeTime += Time.deltaTime;
        }
        else if (isEasingOut)
        {
            // Calculate a new shake intensity that decreases over time
            shakeIntensity = shakeCurve.Evaluate(shakeTime) * (1 - easeOutTime / easeOutDuration);
            easeOutTime += Time.deltaTime;

            // If the ease-out phase is complete, reset to the original position
            if (easeOutTime >= easeOutDuration)
            {
                isEasingOut = false;
                return;
            }
        }
        else
        {
            return;  // Do nothing if not shaking and not easing out
        }
        
        // Limit the shake intensity
        shakeIntensity = Mathf.Min(shakeIntensity, maxShakeIntensity);

        Vector2 shakePosition = originalPosition + Random.insideUnitCircle * shakeIntensity;
        transform.localPosition = shakePosition;
    }
}
