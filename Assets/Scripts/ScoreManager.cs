using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Image scoreBar;

    public int maxScore = 20;

    int bubblesCaptured;
    int difficultyLevel;

    void Awake()
    {
        bubblesCaptured = 0;
        difficultyLevel = 0;
        EventManager.instance.AddListener<CapturedBubbleEvent>( captureBubble );

        if( scoreBar )
        {
            scoreBar.fillAmount = 0f;
        }
    }

    void OnDestroy()
    {
        EventManager.instance.RemoveListener<CapturedBubbleEvent>( captureBubble );
    }

    void captureBubble( CapturedBubbleEvent e )
    {
        Debug.Log( "inc" );
        bubblesCaptured++;
        if( bubblesCaptured % 5 == 0 )
        {
            increasedifficultyLevel();
        }

        if( scoreBar != null )
        {
            scoreBar.fillAmount = Mathf.Clamp01( (float)bubblesCaptured / (float)maxScore );
        }
    }

    void increasedifficultyLevel()
    {
        EventManager.instance.Raise( new IncreaseDifficultyEvent( difficultyLevel ) );
    }
}
