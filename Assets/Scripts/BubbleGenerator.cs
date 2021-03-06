﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{

  GameObject bubblePrefab;
  GameObject smallBubblePrefab;
  GameObject dropPrefab;
  public int maxBubbles;
  public float timeBetweenSpawnsBase;
  public float timeBetweenSpawnsVariance; // amount around base that time between spawns can vary
  float lastSpawnTime;
  float timeUntilNextSpawn;
  ShuffleBag<int> spawnRegionsBag; // to control amount of spawns in each region
  ShuffleBag<int> bubbleTypesBag; // to control the spawn rates of different bubble types

  public Vector2[] spawnRegions; // x is left edge, y is right edge

  public List<GameObject> spawnedBubbles;

  public Sprite bigBubbleSprite;
  public Sprite medBubbleSprite;
  public bool isDropSpawner;
  // Use this for initialization
  public Color p1Color;
  public Color p2Color;

  public Transform player1;
  public Transform player2;

  float lastAlreadyTaggedSpawnTime;
  int alreadyTaggedSpawnCounter = 0;
  static int difficultyIncreases = 0;

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

  void increaseSpeed(IncreaseDifficultyEvent e)
  {
    difficultyIncreases++;
    if ((isDropSpawner && difficultyIncreases <= 5) || !isDropSpawner)
    {
      if ((timeBetweenSpawnsBase > .3f && !isDropSpawner) || isDropSpawner)
      {
        timeBetweenSpawnsBase -= .1f;
        timeBetweenSpawnsVariance -= .1f;
      }
    }
  }

  void endGame(EndGameEvent e)
  {
    Services.Audio.PlaySoundEffect(Services.Clips.GameWin, 0.6f);

    if (!isDropSpawner)
    {
      Destroy(gameObject);
    }
    else
    {
      timeBetweenSpawnsBase = .01f;
      //Debug.Log(e.gameLength+ " "+getNumEndJuiceBubbles(e.gameLength));
      StartCoroutine(resetGame(getNumEndJuiceBubbles(e.gameLength)));
    }
  }

  int getNumEndJuiceBubbles (float gameLength) {
    return Mathf.RoundToInt( Mathf.Lerp( 0f, 7f, Mathf.InverseLerp( 150f, 30f, gameLength ) ) );
  }

  IEnumerator resetGame(float numEndJuiceBubbles)
  {
    ShuffleBag<int> juiceAreas = new ShuffleBag<int>();
    juiceAreas.Add(0, 1);
    juiceAreas.Add(1, 1);
    juiceAreas.Add(2, 1);
    juiceAreas.Add(3, 1);
    yield return new WaitForSeconds(2f);
    for (int i = 0; i < numEndJuiceBubbles; i++)
    {
      addRainbow(juiceAreas.Next());
      Services.Audio.PlaySoundEffect(Services.Clips.EndGamePop[Random.Range(0, Services.Clips.EndGamePop.Length)], 0.7f);
      yield return new WaitForSeconds(.33f);
    }
    yield return new WaitForSeconds(1f);
    addRainbow(4);
    Services.Audio.PlaySoundEffect(Services.Clips.EndGamePop[Random.Range(0, Services.Clips.EndGamePop.Length)], 0.7f);
    yield return new WaitForSeconds(3f);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    yield return null;
  }

  void addRainbow(int area)
  {
    float x = 5;
    float y = 5;
    switch (area)
    {
      case 0:
        x = Random.Range(3.5f, 15f);
        y = Random.Range(5f, 11f);
        break;
      case 1:
        x = Random.Range(15f, 33f);
        y = Random.Range(5f, 11f);
        break;
      case 2:
        x = Random.Range(15f, 33f);
        y = Random.Range(0f, 5f);
        break;
      case 3:
        x = Random.Range(3.5f, 15f);
        y = Random.Range(0f, 5f);
        break;
      case 4:
        x = Random.Range(8f, 25f);
        y = Random.Range(5f, 11f);
        break;
      default:
        x = Random.Range(3.5f, 15f);
        y = Random.Range(5f, 11f);
        break;
    }
    y += 100;
    Instantiate(
      Resources.Load("Prefabs/RainbowJuice"),
      new Vector3(
        x,
        y,
        transform.position.z
      ),
      Quaternion.identity
    );
  }

  void Start()
  {
    bubblePrefab = Resources.Load("Prefabs/Bubble") as GameObject;
    smallBubblePrefab = Resources.Load("Prefabs/SmallBubble") as GameObject;
    dropPrefab = Resources.Load("Prefabs/Waterdrop") as GameObject;
    spawnRegionsBag = new ShuffleBag<int>();
    spawnRegionsBag.Add(0, 1);
    spawnRegionsBag.Add(1, 1);
    spawnRegionsBag.Add(2, 1);
    spawnRegionsBag.Add(3, 1);
    spawnRegionsBag.Add(4, 1);

    bubbleTypesBag = new ShuffleBag<int>();
    bubbleTypesBag.Add(0, 2);
    bubbleTypesBag.Add(1, 4);
  }

  // Update is called once per frame
  void Update()
  {
    if (lastSpawnTime + timeUntilNextSpawn <= Time.time)
    {
      Spawn();
      lastSpawnTime = Time.time;
    }
  }

  void Spawn()
  {
    int myRegion = spawnRegionsBag.Next();
    int myType = isDropSpawner ? 0 : bubbleTypesBag.Next();
    float spawnXpos = Random.Range(spawnRegions[myRegion].x, spawnRegions[myRegion].y);
    GameObject thingToInstantiate = isDropSpawner ? dropPrefab : bubblePrefab;
    if (myType == 1 && !isDropSpawner)
    {
      thingToInstantiate = smallBubblePrefab;
    }
    GameObject newBub = Instantiate(
      thingToInstantiate,
      new Vector3(
        Mathf.Floor(spawnXpos) + .5f,
        transform.position.y,
        transform.position.z
      ),
      Quaternion.identity
    );

    if( !isDropSpawner)
    {
      Bubble bubble = newBub.GetComponent<Bubble>();

      if(bubble)
      {
        bubble.player1 = player1;
        bubble.player2 = player2;
      }

      if (myType == 1)
      {
        newBub.transform.ShiftX(-.5f);
        newBub.transform.ShiftY(-.5f);
        //newBub.GetComponent<Bubble>().timeBetweenMoves *= Mathf.Floor(Random.Range(8f, 9f)) / 10;
      }
    }

    // if (myType == 1 && !isDropSpawner)
    // {
    //   newBub.transform.ShiftX(-.5f);
    //   newBub.transform.ShiftY(-.5f);
    //   //newBub.GetComponent<Bubble>().timeBetweenMoves *= Mathf.Floor(Random.Range(8f, 9f)) / 10;
    // }
    // spawnedBubbles.Add(newBub);
    // if ((myRegion != 2) && !isDropSpawner)
    // {
    //   Debug.Log(alreadyTaggedSpawnCounter);
    //   if (alreadyTaggedSpawnCounter >= 3)
    //   {
    //     int playerColor = Random.Range(0, 2);
    //     newBub.GetComponent<SpriteRenderer>().color =
    //       playerColor == 0
    //         ? p1Color
    //         : p2Color;
    //     if (playerColor == 0)
    //     {
    //       newBub.GetComponent<Bubble>().p1tagged = true;
    //     }
    //     else
    //     {
    //       newBub.GetComponent<Bubble>().p2tagged = true;
    //     }
    //     alreadyTaggedSpawnCounter = 0;
    //   }
    //   else
    //   {
    //     alreadyTaggedSpawnCounter++;
    //   }
    // }
    SetNewSpawnTime();
  }

  void SetNewSpawnTime()
  {
    timeUntilNextSpawn = timeBetweenSpawnsBase + Random.Range(
      -1 * timeBetweenSpawnsVariance / 2,
      timeBetweenSpawnsVariance / 2
    );
  }

}
