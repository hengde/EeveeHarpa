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

  public Vector2[] spawnRegions; // x is left edge, y is right edge

  public List<GameObject> spawnedBubbles;

  // Use this for initialization
  void Start()
  {
    spawnRegionsBag = new ShuffleBag<int>();
    spawnRegionsBag.Add(0, 1);
    spawnRegionsBag.Add(1, 1);
    spawnRegionsBag.Add(2, 1);
    spawnRegionsBag.Add(3, 1);
    spawnRegionsBag.Add(4, 1);
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
    float spawnXpos = Random.Range(spawnRegions[myRegion].x, spawnRegions[myRegion].y);
    GameObject newBub = Instantiate(bubblePrefab, new Vector3(spawnXpos, transform.position.y, transform.position.z), Quaternion.identity);
    spawnedBubbles.Add(newBub);
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
