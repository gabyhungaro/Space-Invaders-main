using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public Projectile laserPrefab;
    private Projectile laser;

    private float leftBound, rightBound;

    private void Start()
    {
        leftBound = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        rightBound = Camera.main.ViewportToWorldPoint(Vector3.right).x;
    }

    private void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        float direction = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) direction = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) direction = 1f;

        if (direction != 0f)
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x + speed * direction * Time.deltaTime, leftBound, rightBound);
            transform.position = position;
        }
    }

    private void Shoot()
    {
        if (laser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            GameManager.Instance.OnPlayerKilled(this);
        }
    }
}
