using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{

  public GameObject bubblePrefab;
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

  // Use this for initialization
  void Start()
  {
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
    int myType = bubbleTypesBag.Next();
    float spawnXpos = Random.Range(spawnRegions[myRegion].x, spawnRegions[myRegion].y);
    GameObject newBub = Instantiate(
      bubblePrefab,
      new Vector3(
        Mathf.Floor(spawnXpos) + .5f,
        transform.position.y,
        transform.position.z
      ),
      Quaternion.identity
    );
    if (myType == 1)
    {
      newBub.GetComponent<SpriteRenderer>().sprite = medBubbleSprite;
      newBub.transform.ShiftX(-.5f);
      newBub.transform.ShiftY(-.5f);
      // small bubbles have a spawn rate between .4 and .6 times that of big bubbles
      newBub.GetComponent<Bubble>().timeBetweenMoves *= Mathf.Floor(Random.Range(4f, 6f)) / 10;
    }
    // spawnedBubbles.Add(newBub);
    Debug.Log("Spawn at " + spawnXpos + "timeUntilNextSpawn");
    SetNewSpawnTime();
  }

  void SetNewSpawnTime()
  {
    timeUntilNextSpawn = timeBetweenSpawnsBase + Random.Range(
      -1 * timeBetweenSpawnsVariance / 2,
      timeBetweenSpawnsVariance / 2
    );
    Debug.Log(timeUntilNextSpawn);
  }

}
