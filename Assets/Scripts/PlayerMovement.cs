using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;

    [SerializeField]
    private Transform body;

    [SerializeField]
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        body.localRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f).normalized;
        // rb.velocity = movement * speed;
        rb.velocity = movement * speed;

        if (movement.magnitude > .1f)
        {
            float targetAngle = (Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg);
            Quaternion targetRotation = Quaternion.Euler(0f, -targetAngle, 0f);

            // Rotate the body
            body.localRotation = Quaternion.Lerp(body.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            // Flip the sprite's Y axis if there is change in horizontal movement
            spriteRenderer.flipY = moveHorizontal < 0f;
        
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
    }

}
