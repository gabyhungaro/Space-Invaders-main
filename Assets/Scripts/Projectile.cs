using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public Vector3 direction = Vector3.up;
    public float speed = 20f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (HandleBunkerCollision(other) || other.gameObject.layer != LayerMask.NameToLayer("Bunker"))
        {
            Destroy(gameObject);
        }
    }

    private bool HandleBunkerCollision(Collider2D other)
    {
        Bunker bunker = other.GetComponent<Bunker>();
        return bunker != null && bunker.CheckCollision(boxCollider, transform.position);
    }
}
