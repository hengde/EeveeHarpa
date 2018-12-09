﻿using System.Collections;
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
	
	[Header("Placeholders")]
	
	[SerializeField] private AudioClip _tick;
	public AudioClip Tick { get { return _tick; } }
	
	[SerializeField] private AudioClip _click;
	public AudioClip Click { get { return _click; } }
	
	[SerializeField] private AudioClip _donk;
	public AudioClip Donk { get { return _donk; } }
}
  