using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] prefabs = new Invader[5];
    public AnimationCurve speed;
    
    private Vector3 direction = Vector3.right;
    private Vector3 initialPosition;

    [Header("Grid")]
    public int rows = 5;
    public int columns = 10;

    [Header("Missiles")]
    public Projectile missilePrefab;
    public float missileSpawnRate = 1f;

    private void Awake()
    {
        initialPosition = transform.position;
        CreateInvaderGrid();
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    private void CreateInvaderGrid()
    {
        float width = 2f * (columns - 1);
        float height = 2f * (rows - 1);
        Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);

        for (int i = 0; i < rows; i++)
        {
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {
                Invader invader = Instantiate(prefabs[i], transform);
                invader.transform.localPosition = rowPosition + new Vector3(2f * j, 0f, 0f);
            }
        }
    }

    private void Update()
    {
        float percentKilled = 1f - (GetAliveCount() / (float)(rows * columns));
        transform.position += speed.Evaluate(percentKilled) * Time.deltaTime * direction;

        CheckBounds();
    }

    private void CheckBounds()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) continue;

            if ((direction == Vector3.right && invader.position.x >= rightEdge.x - 1f) ||
                (direction == Vector3.left && invader.position.x <= leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        direction.x = -direction.x;
        transform.position += Vector3.down;
    }

    private void MissileAttack()
    {
        int amountAlive = GetAliveCount();
        if (amountAlive == 0) return;

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy) continue;
            if (Random.value < (1f / amountAlive))
            {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;
        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf) count++;
        }
        return count;
    }
}
