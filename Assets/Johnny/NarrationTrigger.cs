/*
		File: NarrationTrigger.cs
		
		Version: 1.1
		
		Author: Johnathon Forster
		
		Description:
				Enables a trigger that:
					plays an audio clip attached in the Inspector (required)
					destroys an object once the audio clip has finished playing (optional)
*/

using UnityEngine;
using System;

public class NarrationTrigger : MonoBehaviour {

	public AudioClip narration;
	public GameObject barrier;

	private AudioSource audioSource;
	private bool narrativeTriggered;

	// Use this for initialization
	void Start () {
		narrativeTriggered = false;
		audioSource = GameObject.Find("Seraph").transform.gameObject.AddComponent<AudioSource>();
		print(audioSource);
		
		if (narration != null)
		{
			audioSource.clip = narration;
		} else {
			throw new Exception("No audio clip attached");
		}
		
		if (barrier == null)
		{
			Debug.LogWarning("No barrier attached");
		}
	}
	
	void Update () {
		//	This should only trigger if the audio has finished playing
		if(narrativeTriggered == true)
		{
			if(audioSource.isPlaying == false)
			{
				if(barrier != null)
				{
					Destroy(barrier);
					print("Destroyed barrier");
				}
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if(narrativeTriggered == false)
		{
			if(other.gameObject.tag == "Player")
			{
				if(audioSource.isPlaying == false)
				{
					if(narration != null) {
						audioSource.Play();
						print("Playing narration");
					}
					narrativeTriggered = true;
				}
			}
		}
	}
}
