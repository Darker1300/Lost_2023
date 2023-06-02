using UnityEngine;

public class FishAI : MonoBehaviour
{
    public Transform[] waypoints; // Waypoints for patrol
    public float speed = 2.0f; // Patrol speed
    public float rotationSpeed = 2.0f; // Speed at which the fish rotates
    public float chaseSpeed = 5.0f; // Chase speed
    public float detectionDistance = 10.0f; // Distance at which fish starts to chase the player

    private int currentWaypointIndex = 0; // Index of current waypoint
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private float lostSightTime;
    private Vector3 lastSeenPosition;
    private bool isChasing;
    private bool isInvestigating;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
    // Cast a ray in front of the fish
        RaycastHit hit;

        bool playerDetected = Physics.Raycast(transform.position, -transform.right, out hit, detectionDistance) 
            && hit.transform == playerTransform;
        
        Debug.DrawRay(transform.position, -transform.right * detectionDistance, Color.red);

        if (playerDetected)
        {
            ChasePlayer();
            lastSeenPosition = playerTransform.position;
            lostSightTime = 0f;
            isInvestigating = false;
            isChasing = true;
        }
        else if (isChasing)
        {
            Debug.Log("Investigating...");
            // If the player was being chased but is now out of sight
            if (!isInvestigating)
            {
                // Start the investigation timer
                lostSightTime += Time.deltaTime;
                if (lostSightTime >= 3.0f)
                {
                    // Start investigating after a delay
                    isInvestigating = true;
                }
            }
            else
            {
                // Continue investigating
                Investigate();
            }
        }
        else
        {
            // If the player is not detected, patrol
            Patrol();
        }

    }

    void Patrol()
    {
        // If fish reached a waypoint, move to the next one
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Rotate and move towards the current waypoint
        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
        direction = direction.normalized;
        float targetAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        Quaternion toRotation = Quaternion.Euler(0f, 0f, targetAngle - 180f);  
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        spriteRenderer.flipY = direction.x > 0f;
        
        transform.position += direction * speed * Time.deltaTime;
    }

    void ChasePlayer()
    {
        // Rotate and move towards the player
        Vector3 direction = playerTransform.position - transform.position;
        direction = direction.normalized;
        float targetAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        Quaternion toRotation = Quaternion.Euler(0f, 0f, targetAngle - 180f); 
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        spriteRenderer.flipY = direction.x > 0f;
        transform.position += direction * chaseSpeed * Time.deltaTime;
    }

    void Investigate()
    {
        // Move towards the last seen position of the player
        Vector3 direction = lastSeenPosition - transform.position;
        direction = direction.normalized;

        float targetAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Quaternion toRotation = Quaternion.Euler(0f, 0f, targetAngle - 180f);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        transform.position += direction * speed * Time.deltaTime;

        // If the fish reached the last seen position, stop investigating and start patrolling
        if (Vector3.Distance(transform.position, lastSeenPosition) < 0.1f)
        {
            isInvestigating = false;
            isChasing = false;
        }
    }
}
