using UnityEngine;

public class PlayerController : GameInput
{
    [Header( "Game" )]
    public GameObject player1;
    public GameObject player2;
    public float moveSpeed;

    Vector3 p1Start;
    Vector3 p2Start;

    Vector3 p1Pos;
    Vector3 p2Pos;

    void Start()
    {
        p1Start = player1.transform.localPosition;
        p2Start = player2.transform.localPosition;

        p1Pos = p1Start;
        p2Pos = p2Start;
    }

    void Update()
    {
        Vector2 p1Move = Vector2.ClampMagnitude( joy1, 1 );
        p1Pos += ( Vector3.right * p1Move.x + Vector3.up * p1Move.y ) * moveSpeed * Time.deltaTime;
        p1Pos = new Vector3( Mathf.Clamp( p1Pos.x, 2f, 35f ), Mathf.Clamp( p1Pos.y, 2f, 10f ), p1Pos.z );

        player1.transform.localPosition = new Vector3( Mathf.Floor( p1Pos.x ) + 0.5f, Mathf.Floor( p1Pos.y ) + 0.5f, p1Pos.z );

        Vector2 p2Move = Vector2.ClampMagnitude( joy2, 1 );
        p2Pos += ( Vector3.right * p2Move.x + Vector3.up * p2Move.y ) * moveSpeed * Time.deltaTime;
        p2Pos = new Vector3( Mathf.Clamp( p2Pos.x, 2f, 35f ), Mathf.Clamp( p2Pos.y, 2f, 10f ), p2Pos.z );

        player2.transform.localPosition = new Vector3( Mathf.Floor( p2Pos.x ) + 0.5f, Mathf.Floor( p2Pos.y ) + 0.5f, p2Pos.z );
    }

    public override void Button1Down()
    {
         
    }

    public override void Button2Down()
    {
        
    }

    public override void Button3Down()
    {

    }

    public override void Button4Down()
    {
        
    }

    public override void Reset()
    {
        p1Pos = p1Start;
        p2Pos = p2Start;

        player1.transform.localPosition = new Vector3( Mathf.Floor( p1Pos.x ) + 0.5f, Mathf.Floor( p1Pos.y ) + 0.5f, p1Pos.z );
        player2.transform.localPosition = new Vector3( Mathf.Floor( p2Pos.x ) + 0.5f, Mathf.Floor( p2Pos.y ) + 0.5f, p2Pos.z );
    }
}
