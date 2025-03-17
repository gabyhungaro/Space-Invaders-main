using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MysteryShip : MonoBehaviour
{
    public float speed = 5f;
    public float cycleTime = 30f;
    public int score = 300;

    private Vector2 leftDestination, rightDestination;
    private int direction = -1;
    private bool spawned;

    private void Start()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        leftDestination = new Vector2(leftEdge.x - 1f, transform.position.y);
        rightDestination = new Vector2(rightEdge.x + 1f, transform.position.y);

        Despawn();
    }

    private void Update()
    {
        if (!spawned) return;

        transform.position += speed * Time.deltaTime * (direction == 1 ? Vector3.right : Vector3.left);

        if (direction == 1 && transform.position.x >= rightDestination.x ||
            direction == -1 && transform.position.x <= leftDestination.x)
        {
            Despawn();
        }
    }

    private void Spawn()
    {
        direction *= -1;
        transform.position = direction == 1 ? leftDestination : rightDestination;
        spawned = true;
    }

    private void Despawn()
    {
        spawned = false;
        transform.position = direction == 1 ? rightDestination : leftDestination;
        Invoke(nameof(Spawn), cycleTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn();
            GameManager.Instance.OnMysteryShipKilled(this);
        }
    }
}
