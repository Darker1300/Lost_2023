using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TremorSense : MonoBehaviour
{
    public GameObject rippleEffectPrefab; // Prefab of the particle system for the ripple effect

    private Dictionary<Collider, Coroutine> detectedEntities = new Dictionary<Collider, Coroutine>();

    void Start()
    {
        // Ensure the attached GameObject has a SphereCollider
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            Debug.LogError("TremorSense requires a SphereCollider.");
            return;
        }

        // Ensure the SphereCollider is set as a trigger
        if (!sphereCollider.isTrigger)
        {
            Debug.LogWarning("Setting SphereCollider as trigger for TremorSense.");
            sphereCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Ensure the detected object is a moving entity
        if (other.CompareTag("MovingEntity") && !detectedEntities.ContainsKey(other))
        {
            // Start repeating the ripple effect every 1 second
            Coroutine coroutine = StartCoroutine(CreateRippleRepeatedly(other));
            detectedEntities.Add(other, coroutine);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the detected entity leaves the trigger, stop repeating the ripple effect
        if (detectedEntities.ContainsKey(other))
        {
            StopCoroutine(detectedEntities[other]);
            detectedEntities.Remove(other);
        }
    }

    IEnumerator CreateRippleRepeatedly(Collider entity)
    {
        while (true)
        {   

            // Instantiate the ripple effect at the position of the detected entity
            GameObject ripple = Instantiate(rippleEffectPrefab, entity.transform.position, Quaternion.identity);
            
            EnemyType type = entity.GetComponent<EnemyType>();
            if (type != null)
            {
                ripple.transform.localScale = type.GetScale();
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
