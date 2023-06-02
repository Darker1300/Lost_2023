using UnityEngine;

public class ObjectTransparency : MonoBehaviour
{
    public Transform player;
    public float transparency = 0.5f;

    private Renderer lastHitRenderer;
    private Color lastOriginalColor;

    void Update()
    {
        RaycastHit hit;

        // Reset the last hit object's transparency
        if (lastHitRenderer != null)
        {
            lastHitRenderer.material.color = lastOriginalColor;
            lastHitRenderer = null;
        }

        // Cast a ray from the camera to the player
        if (Physics.Linecast(transform.position, player.position, out hit))
        {
            // If an object is hit, make it transparent
            Renderer hitRenderer = hit.transform.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                Debug.Log("Trans");
                lastOriginalColor = hitRenderer.material.color;
                Color newColor = new Color(lastOriginalColor.r, lastOriginalColor.g, lastOriginalColor.b, transparency);
                hitRenderer.material.color = newColor;
                lastHitRenderer = hitRenderer;
            }
        }
    }
}
