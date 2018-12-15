using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
  Vector2 lastMove; // last x, y moves
  public Vector2 timeBetweenMoves = new Vector2(.1f, .3f); //default for big bubbles

  // Use this for initialization
  void Start()
  {
    timeBetweenMoves.y = Random.Range(.1f, .3f);
    transform.ShiftY(.5f);
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time - timeBetweenMoves.y > lastMove.y)
    {
      transform.SetY(transform.position.y - 1);
      lastMove.y = Time.time;
    }
    if (transform.localPosition.y < -5)
    {
      Destroy(gameObject);
    }
    if (Time.time - timeBetweenMoves.x > lastMove.x)
    {

    }
  }
}
