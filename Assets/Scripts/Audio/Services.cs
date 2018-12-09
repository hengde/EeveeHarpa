using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Services
{
	private static AudioManager _audio;
	public static AudioManager Audio
	{
		get
		{
			Debug.Assert(_audio != null);
			return _audio;
		}
		set { _audio = value; }
	}

	private static ClipLibrary _clips;
	public static ClipLibrary Clips
	{
		get
		{
			Debug.Assert(_clips != null);
			return _clips;
		}
		set { _clips = value; }
	}
}
