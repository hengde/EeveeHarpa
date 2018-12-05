using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
  float lastMove;
  public float timeBetweenMoves = .6f; //default for big bubbles
  bool isClaimed = false; // has this bubble been claimed?
  Color ownerColor = new Color(0, 0, 0);
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time - timeBetweenMoves > lastMove)
    {
      transform.SetY(transform.position.y + 1);
      lastMove = Time.time;
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
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    Debug.Log("hit " + other.name);
    if (other.gameObject.name.Contains("layer"))
    {
      getClaimed(other.gameObject.GetComponent<SpriteRenderer>().color);
    }
  }
}
