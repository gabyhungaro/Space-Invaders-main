using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites = new Sprite[0];
    public float animationTime = 1f;
    public int score = 10;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animationSprites.Length > 0)
        {
            spriteRenderer.sprite = animationSprites[0];
        }
    }

    private void Start()
    {
        if (animationSprites.Length > 1)
        {
            InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
        }
    }

    private void AnimateSprite()
    {
        animationFrame = (animationFrame + 1) % animationSprites.Length;
        spriteRenderer.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Laser"))
        {
            GameManager.Instance.OnInvaderKilled(this);
        }
        else if (layer == LayerMask.NameToLayer("Boundary"))
        {
            GameManager.Instance.OnBoundaryReached();
        }
    }
}
