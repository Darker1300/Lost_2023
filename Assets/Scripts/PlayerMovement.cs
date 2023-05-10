using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f).normalized;
        rb.velocity = movement * speed;

        // if (movement.magnitude > 0.1f)
        // {
        //     float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
        //     Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        //     transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
    }
}
