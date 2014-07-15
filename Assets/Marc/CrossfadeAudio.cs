//
// Filename : CrossfadeAudio.cs
// Author : Louis Dimmock & Marc Dunsmore
// Date : 8th April 2014
// 
// Version : 1.0
// Version Info : Simple script to crossfade two audio clips when the Seraph leaves a trigger area
//

using System;
using UnityEngine;


public class CrossfadeAudio : MonoBehaviour
{
	// Define our audio clips to play
	public AudioClip m_firstAudio;
	public AudioClip m_secondAudio;

	// Define whether to loop audio
	public bool m_loopFirstAudio = false;
	public bool m_loopSecondAudio = false;

	// Define how fast to crossfade speeds
	public float m_crossfadeSpeed = 0.1f;
	
	// Our audio player
	private AudioSource m_firstAudioSource;
	private AudioSource m_secondAudioSource;

	// Flag for whether we are cross fading or not
	private bool m_crossFade = false;

	// Flag for whether we have already triggered the crossfade
	private bool m_isTriggered = false;
	
	void Start()
	{
		// Attach the audio sources to the game object
		m_firstAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
		m_secondAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");

		// Apply our audio clips to our audio sources
		m_firstAudioSource.clip = m_firstAudio;
		m_secondAudioSource.clip = m_secondAudio;

		// Check whether we should loop the audioclips
		m_firstAudioSource.loop = m_loopFirstAudio;
		m_secondAudioSource.loop = m_loopSecondAudio;

		// Define the starting volumes
		m_firstAudioSource.volume = 1.0f;
		m_secondAudioSource.volume = 0.0f;
	}
	
	void Update()
	{
		// If we are wanting to crossfade
		if( m_crossFade )
		{
            if( m_firstAudio != null)
			{
				// Decrease the first audio source volume
				m_firstAudioSource.volume -= m_crossfadeSpeed;

				// Check if the first audio source can no longer be heard
				if( m_firstAudioSource.volume <= 0.0f )
				{
					// Stop playing
                    m_firstAudioSource.Stop();
				}

			}

			if( m_secondAudio != null)
			{
				// Increase the second audio source volume
				m_secondAudioSource.volume += m_crossfadeSpeed;
				
				// Check if the second audio source has reached its maximum
				if( m_secondAudioSource.volume >= 1.0f )
				{
					// Make sure we are at the maximum volume for the source
					m_secondAudioSource.volume = 1.0f;
					
					// Unflag that we want to crossfade
					m_crossFade = false;
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		// If we arent playing the first sound
		if( !m_firstAudioSource.isPlaying)
		{
			// Play it
			m_firstAudioSource.Play();
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if( !m_isTriggered )
		{
			// When we leave the area
			// Tell the object to crossfade our first audio with the second
			m_crossFade = true;
			
			// Set that we have triggered the trigger
			m_isTriggered = true;

	        // Start playing the second
			m_secondAudioSource.Play();
		}
	}
}