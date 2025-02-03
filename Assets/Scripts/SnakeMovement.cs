using Unity.VisualScripting;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public float speed = 5f;
    public SteeringKnob steeringKnob;

    private Rigidbody2D rb;
    private Vector2 direction;
    private float baseRotationAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.up;
        baseRotationAngle = 0f;
    }

    void FixedUpdate()
    {
        if (!GameManager.Isgridinitialized)
            return;

        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (steeringKnob != null && steeringKnob.IsKnobHeld)
        {
            float steeringOutput = -steeringKnob.SteeringValue;
            float offsetAngle = steeringOutput * steeringKnob.MaxTurnAngle;
            float newAngle = baseRotationAngle + offsetAngle;

            transform.rotation = Quaternion.Euler(0, 0, newAngle);

            float radians = newAngle * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
        }

        rb.velocity = direction * speed;

        if (steeringKnob != null && !steeringKnob.IsKnobHeld)
        {
            baseRotationAngle = transform.eulerAngles.z;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            direction = Vector2.Reflect(direction, normal);
            direction.Normalize();

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            baseRotationAngle = angle;
        }
    }
}
