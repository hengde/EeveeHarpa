using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Clip Library")]
public class ClipLibrary : ScriptableObject {

	[Header("Music Tracks")]
	
	[SerializeField] private AudioClip _audioBed;
	public AudioClip AudioBed { get { return _audioBed; } }
	
	[SerializeField] private AudioClip _burbles;
	public AudioClip Burbles { get { return _burbles; } }
	
	[Header("Sound Effects")]
	
	[SerializeField] private AudioClip[] _bubblePops;
	public AudioClip[] BubblePops { get { return _bubblePops; } }
	
	[SerializeField] private AudioClip[] _bubbleClaimed;
	public AudioClip[] BubbleClaimed { get { return _bubbleClaimed; } }

	[SerializeField] private AudioClip[] _bubbleCaptured;
	public AudioClip[] BubbleCaptured { get { return _bubbleCaptured; }}
	
	[SerializeField] private AudioClip[] _endGamePop;
	public AudioClip[] EndGamePop { get { return _endGamePop; }}
	
	[SerializeField] private AudioClip _gameWin;
	public AudioClip GameWin { get { return _gameWin; }}
	
	[Header("Placeholders")]
	
	[SerializeField] private AudioClip _tick;
	public AudioClip Tick { get { return _tick; } }
	
	[SerializeField] private AudioClip _click;
	public AudioClip Click { get { return _click; } }
	
	[SerializeField] private AudioClip _donk;
	public AudioClip Donk { get { return _donk; } }
}
  