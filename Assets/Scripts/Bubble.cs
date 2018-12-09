using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
  public GameObject juice;
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
      if (newColor != ownerColor)
      {
        Destroy(gameObject); // TODO replace with cool animation and sfx
      }
    }
    else
    {
      isClaimed = true;
      ownerColor = newColor;
      gameObject.GetComponent<SpriteRenderer>().color = ownerColor;
      runJuice();
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
        getClaimed(other.gameObject.GetComponent<SpriteRenderer>().color);
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
        getClaimed(other.gameObject.GetComponent<SpriteRenderer>().color);
      }
    }
  }

  private void getCaptured()
  {
    // do cool visual effects
    ownerColor = Color.white;
    gameObject.GetComponent<SpriteRenderer>().color = ownerColor;
    runJuice();
  }

  void runJuice()
  {
    GameObject myJuice = Instantiate(juice, transform.position, Quaternion.identity);
    myJuice.GetComponent<SpriteRenderer>().color = ownerColor;
  }
}
