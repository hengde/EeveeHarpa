using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
  GameObject juice;
  GameObject rainbowJuice;
  Vector2 lastMove; // last x, y moves
  public Vector2 timeBetweenMoves = new Vector2(.6f, .5f); //default for big bubbles
  bool isClaimed = false; // has this bubble been claimed?

  bool p1tagged;
  bool p2tagged;

  bool floatingRight; // control x movement left or right
  int xMovesbeforeSwitch; // number of moves in one x direction before flipping floatingRight
  int xMoveCounter = 2;
  Color ownerColor = new Color(0, 0, 0);
  void Start()
  {
    juice = Resources.Load("Prefabs/BubbleJuice") as GameObject;
    rainbowJuice = Resources.Load("Prefabs/RainbowJuice") as GameObject;
    Debug.Log(juice);
    timeBetweenMoves.x = Random.Range(.8f, 1.5f);
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time - timeBetweenMoves.y > lastMove.y)
    {
      transform.SetY(transform.position.y + 1);
      lastMove.y = Time.time;
    }
    if (transform.localPosition.y > 120)
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
  }

  void getClaimed(Color newColor)
  {
    if (isClaimed)
    {
      Services.Audio.PlaySoundEffect(Services.Clips.BubblePops[Random.Range(0, Services.Clips.BubblePops.Length)], 0.5f);
      if (newColor != ownerColor)
      {
        Destroy(gameObject); // TODO replace with cool animation and sfx
      }
    }
    else
    {
      Services.Audio.PlaySoundEffect(Services.Clips.BubbleClaimed[Random.Range(0, Services.Clips.BubbleClaimed.Length)], 0.5f);
      isClaimed = true;
      ownerColor = newColor;
      gameObject.GetComponent<SpriteRenderer>().color = ownerColor;//
      // runJuice(false);
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log("hit " + other.name);
    if (!p1tagged && other.gameObject.name.Contains("Player1"))
    {
      p1tagged = true;
      if (p2tagged)
      {
        getCaptured();
      }
      else
      {
        Debug.Log(other.gameObject.GetComponent<BubblePlayer>().bubbleColor);
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
    Services.Audio.PlaySoundEffect(Services.Clips.BubbleCaptured, 1.0f);
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
