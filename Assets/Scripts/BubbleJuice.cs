﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleJuice : MonoBehaviour
{

  float spawnTime;
  // Use this for initialization
  void Start()
  {
    spawnTime = Time.time;
  }

  // Update is called once per frame
  void Update()
  {
    if (Time.time - spawnTime > 1.9f)
    {
      Destroy(gameObject);
    }
  }
}
