using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    private void Update()
    {
        transform.position = playerTransform.position + offset;
    }
}
