using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

  // Use this for initialization
  int bubblesCaptured;
  int difficultyLevel;

  void Awake()
  {
    bubblesCaptured = 0;
    difficultyLevel = 0;
    EventManager.instance.AddListener<CapturedBubbleEvent>(captureBubble);
  }

  void OnDestroy()
  {
    EventManager.instance.RemoveListener<CapturedBubbleEvent>(captureBubble);
  }
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  void captureBubble(CapturedBubbleEvent e)
  {
    bubblesCaptured++;
    if (bubblesCaptured % 5 == 0)
    {
      Debug.Log("inc");
      increasedifficultyLevel();
    }
  }

  void increasedifficultyLevel()
  {
    EventManager.instance.Raise(new IncreaseDifficultyEvent(difficultyLevel));
  }
}
