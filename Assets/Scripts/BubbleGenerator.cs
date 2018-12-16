using System.Collections;
using System.Collections.Generic;
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

  float lastAlreadyTaggedSpawnTime;
  int alreadyTaggedSpawnCounter = 0;
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
    if (myType == 1 && !isDropSpawner)
    {
      newBub.transform.ShiftX(-.5f);
      newBub.transform.ShiftY(-.5f);
      //newBub.GetComponent<Bubble>().timeBetweenMoves *= Mathf.Floor(Random.Range(8f, 9f)) / 10;
    }
    // spawnedBubbles.Add(newBub);
    if ((myRegion != 2) && !isDropSpawner)
    {
      Debug.Log(alreadyTaggedSpawnCounter);
      if (alreadyTaggedSpawnCounter >= 3)
      {
        int playerColor = Random.Range(0, 2);
        newBub.GetComponent<SpriteRenderer>().color =
          playerColor == 0
            ? p1Color
            : p2Color;
        if (playerColor == 0)
        {
          newBub.GetComponent<Bubble>().p1tagged = true;
        }
        else
        {
          newBub.GetComponent<Bubble>().p2tagged = true;
        }
        alreadyTaggedSpawnCounter = 0;
      }
      else
      {
        alreadyTaggedSpawnCounter++;
      }
    }
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
