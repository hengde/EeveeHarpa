using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
  GameObject juice;
  GameObject rainbowJuice;
  Vector2 lastMove; // last x, y moves
  public Vector2 timeBetweenMoves = new Vector2(.8f, .7f); //default for big bubbles
  float baseTimeBetweenMovesY;
  bool isClaimed = false; // has this bubble been claimed?

  public bool p1tagged;
  public bool p2tagged;

  public float radius;

  public Transform player1;
  public Transform player2;

  bool floatingRight; // control x movement left or right
  int xMovesbeforeSwitch; // number of moves in one x direction before flipping floatingRight
  int xMoveCounter = 2;
  Color ownerColor = new Color(0, 0, 0);
  float difficultyRamp = .95f;

  static int difficultyIncreasesCount = 0;
  void Awake()
  {
    EventManager.instance.AddListener<IncreaseDifficultyEvent>(increaseSpeed);
    EventManager.instance.AddListener<EndGameEvent>(endGame);
  }

  void OnDestroy()
  {
    EventManager.instance.RemoveListener<IncreaseDifficultyEvent>(increaseSpeed);
    EventManager.instance.RemoveListener<EndGameEvent>(endGame);
  }

  void endGame(EndGameEvent e)
  {
    timeBetweenMoves.y = .2f;
    difficultyIncreasesCount = 0;
  }

  void Start()
  {
    baseTimeBetweenMovesY = timeBetweenMoves.y;
    juice = Resources.Load("Prefabs/BubbleJuice") as GameObject;
    rainbowJuice = Resources.Load("Prefabs/RainbowJuice") as GameObject;
    timeBetweenMoves.x = Random.Range(.8f, 1.5f);
    for (int i = 0; i < difficultyIncreasesCount; i++)
    {
      timeBetweenMoves.y *= difficultyRamp;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time - timeBetweenMoves.y > lastMove.y)
    {
      transform.SetY(transform.position.y - 1);
      lastMove.y = Time.time;
    }
    if (transform.localPosition.y < 90)
    {
      Destroy(gameObject);
    }
    if (Time.time - timeBetweenMoves.x > lastMove.x)
    {
      if (floatingRight)
      {
        transform.SetX(transform.position.x + 1);
      }
      else
      {
        transform.SetX(transform.position.x - 1);
      }
      xMoveCounter++;
      lastMove.x = Time.time;
      if (xMoveCounter > xMovesbeforeSwitch)
      {
        floatingRight = !floatingRight;
      }
    }

    Vector2 flatPosition = new Vector2( transform.position.x, transform.position.y );
    float distance;
    float collisionDistance = 1.25f + radius;
    
    if( player1 != null )
    {
      distance = Vector2.Distance( flatPosition, new Vector2( player1.position.x, player1.position.y ) );
      if( distance < collisionDistance )
      {
        p1tagged = true;
        if (p2tagged)
        {
          getCaptured();
        }
        else
        {
          // Debug.Log(other.gameObject.GetComponent<BubblePlayer>().bubbleColor);
          getClaimed(player1.gameObject.GetComponent<BubblePlayer>().bubbleColor);
        }
      }
    }

    if( player2 != null )
    {
      distance = Vector2.Distance( flatPosition, new Vector2( player2.position.x, player2.position.y ) );
      if( distance < collisionDistance )
      {
        p2tagged = true;
        if (p1tagged)
        {
          getCaptured();
        }
        else
        {
          getClaimed(player2.gameObject.GetComponent<BubblePlayer>().bubbleColor);
        }
      }
    }
  }

  void increaseSpeed(IncreaseDifficultyEvent e)
  {
    difficultyIncreasesCount++;
    for (int i = 0; i < difficultyIncreasesCount; i++)
    {
      timeBetweenMoves.y *= difficultyRamp;
    }
  }

  void getClaimed(Color newColor)
  {
    if (isClaimed)
    {
      // Services.Audio.PlaySoundEffect(Services.Clips.BubblePops[Random.Range(0, Services.Clips.BubblePops.Length)], 0.5f);
      if (newColor != ownerColor)
      {
        Destroy(gameObject); // TODO replace with cool animation and sfx
      }
    }
    else
    {
      Services.Audio.PlaySoundEffect(Services.Clips.BubbleClaimed[Random.Range(0, Services.Clips.BubbleClaimed.Length)], 0.3f);
      isClaimed = true;
      ownerColor = newColor;
      gameObject.GetComponent<SpriteRenderer>().color = ownerColor;//
      // runJuice(false);
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
      //  Debug.Log("hit " + other.name + " when I'm at " + transform.position + " and other is at " + other.transform.position );
    if (!p1tagged && other.gameObject.name.Contains("Player1"))
    {
      p1tagged = true;
      if (p2tagged)
      {
        getCaptured();
      }
      else
      {
        // Debug.Log(other.gameObject.GetComponent<BubblePlayer>().bubbleColor);
        getClaimed(other.gameObject.GetComponent<BubblePlayer>().bubbleColor);
      }
    }
    if (!p2tagged && other.gameObject.name.Contains("Player2"))
    {
      p2tagged = true;
      if (p1tagged)
      {
        getCaptured();
      }
      else
      {
        getClaimed(other.gameObject.GetComponent<BubblePlayer>().bubbleColor);
      }
    }
  }

  private void getCaptured()
  {
    EventManager.instance.Raise(new CapturedBubbleEvent());
    Services.Audio.PlaySoundEffect(Services.Clips.BubbleCaptured[Random.Range(0, Services.Clips.BubbleCaptured.Length)], 0.35f);
    Services.Audio.PlaySoundEffect(Services.Clips.BubblePops[Random.Range(0, Services.Clips.BubblePops.Length)], 0.5f, Random.Range(0.8f, 1.2f));
    // do cool visual effects
    ownerColor = Color.white;
    gameObject.GetComponent<SpriteRenderer>().color = ownerColor;
    runJuice(true);
  }

  void runJuice(bool combined = false)
  {
    GameObject myJuice;
    if (combined)
    {
      Vector3 newPos = new Vector3(
        Mathf.Floor(transform.position.x) + .5f,
        Mathf.Floor(transform.position.x) + .5f,
        transform.position.z
      );
      // Instantiate(Resources.Load("Prefabs/RainbowBubble"), newPos, Quaternion.identity);
      myJuice = Instantiate(rainbowJuice, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
    else
    {
      // myJuice = Instantiate(juice, transform.position, Quaternion.identity);
      // myJuice.GetComponent<SpriteRenderer>().color = ownerColor;
    }

  }
}
