using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
  float speed = .3f;
  float lastMove;
  float timeBetweenMoves = .8f;
  // Use this for initialization
  void Start()
  {
    transform.SetX(Mathf.Floor(transform.position.x) + .5f);
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
}
