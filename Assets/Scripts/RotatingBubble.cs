using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBubble : MonoBehaviour
{

  public Sprite rot1;
  public Sprite rot2;
  public SpriteRenderer renderer1;
  public SpriteRenderer renderer2;
  // Use this for initialization
  void Start()
  {

  }

  public float timeToRotate;
  float lastRotationTime;

  // Update is called once per frame
  void Update()
  {
    if (Time.time - lastRotationTime > timeToRotate)
    {
      rotate();
    }
  }

  void rotate()
  {
    Sprite tempSprite = renderer1.sprite;
    renderer1.sprite = renderer2.sprite;
    renderer2.sprite = tempSprite;
    lastRotationTime = Time.time;
  }
}
