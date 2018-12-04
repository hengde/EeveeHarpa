using UnityEngine;

public class TestGame : GameInput
{
    [Header("Game")]
    public GameObject player1;
    public GameObject player2;
    public float moveSpeed;
    public GameObject explosion;
    public float explosionLifetime;

    Vector3 p1Start;
    Vector3 p2Start;

    void Start()
    {
        p1Start = player1.transform.position;
        p2Start = player2.transform.position;
    }

    void Update()
    {
        Vector2 p1Move = Vector2.ClampMagnitude(joy1, 1);
        player1.transform.position += (Vector3.right * p1Move.x + Vector3.up * p1Move.y) * moveSpeed * Time.deltaTime;

        Vector2 p2Move = Vector2.ClampMagnitude(joy2, 1);
        player2.transform.position += (Vector3.right * p2Move.x + Vector3.up * p2Move.y) * moveSpeed * Time.deltaTime;
    }

    void ExplosionAt(Vector3 position)
    {
        GameObject newExplosion = Instantiate(explosion, position, Quaternion.identity);
        Destroy(newExplosion, explosionLifetime);
    }

    public override void Button1Down()
    {
        ExplosionAt(player1.transform.position);
    }

    public override void Button2Down()
    {
        ExplosionAt(player1.transform.position);
    }

    public override void Button3Down()
    {
        ExplosionAt(player2.transform.position);
    }

    public override void Button4Down()
    {
        ExplosionAt(player2.transform.position);
    }

    public override void Reset()
    {
        player1.transform.position = p1Start;
        player2.transform.position = p2Start;
    }
}
