using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxInitialAngle=0.67f;
    private Rigidbody2D rb;
    private Vector2 direction;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitialPush();
    }

    
    private void InitialPush()
    {
        Vector2 dir = Vector2.left;
        dir.y = Random.Range(-maxInitialAngle, maxInitialAngle);
        rb.linearVelocity = dir * movementSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.gameObject.name);
    }
}
