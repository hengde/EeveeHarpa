using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
  Vector2 lastMove; // last x, y moves
  public Vector2 timeBetweenMoves = new Vector2(.1f, .3f); //default for big bubbles
  float difficultyRamp = .95f;

  static int difficultyIncreasesCount = 0;

  void Awake()
  {
    EventManager.instance.AddListener<IncreaseDifficultyEvent>(increaseSpeed);
  }

  void OnDestroy()
  {
    EventManager.instance.RemoveListener<IncreaseDifficultyEvent>(increaseSpeed);
  }

  void increaseSpeed(IncreaseDifficultyEvent e)
  {
    if (difficultyIncreasesCount <= 5)
    {
      difficultyIncreasesCount++;
      for (int i = 0; i < difficultyIncreasesCount; i++)
      {
        timeBetweenMoves.y *= difficultyRamp;
      }
    }
  }
  // Use this for initialization
  void Start()
  {
    timeBetweenMoves.y = Random.Range(.1f, .3f);
    transform.ShiftY(.5f);
    int increases = difficultyIncreasesCount <= 5 ? difficultyIncreasesCount : 5;
    for (int i = 0; i < increases; i++)
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

    }
  }
}
