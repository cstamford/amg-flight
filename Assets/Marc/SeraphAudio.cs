// ==================================================================== \\
// File   : SeraphAudio.cs                                        		\\
// Author : Marc Dunsmore									    		\\
//                                                                      \\
// SeraphAudio.cs provides all audio for the project.   				\\
//                                                                      \\
// TO BE FILLED WITH INSIGHTFUL WORDS 									\\
//                                                                      \\
// This audio manager mainly deals with the different audio by 			\\
// switching between the following staes in the Seraph Controller:      \\
//   - GROUNDED                                                         \\
//   - GLIDING                                                          \\
//   - LANDING                                                          \\
//	 - FALLING															\\
// ==================================================================== \\

using System;
using cst.Flight;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeraphAudio : MonoBehaviour
{
    private const int WALK_VARIANTS = 15;

    private AudioSource m_ambientAudioSource;
    private AudioSource m_glidingAudioSource;
    private AudioSource m_walkingAudioSource;
	private AudioSource m_fallingAudioSource;
	private AudioSource m_landingAudioSource;

	private AudioClip m_ambientAudioClip;
	private AudioClip m_fallingAudioClip;
    private AudioClip m_glidingAudioClip;
    private readonly AudioClip[] m_walkGrassAudioClips = new AudioClip[WALK_VARIANTS];
	private AudioClip m_walkStoneAudioClip;

    private SeraphController m_controller;

	public void Start()
	{
	    m_controller = FindObjectOfType<SeraphController>();

        if (m_controller == null)
            throw new Exception("No Seraph Controller found on this GameObject.");

		m_ambientAudioClip = (AudioClip)Resources.Load("windy ambience");
		m_fallingAudioClip = (AudioClip)Resources.Load("falling");
        m_glidingAudioClip = (AudioClip)Resources.Load("temporary flying noise");

        for (int i = 0; i != WALK_VARIANTS; ++i)
        {
            m_walkGrassAudioClips[i] = (AudioClip)Resources.Load(String.Format("grass walk ({0})", i + 1));
        }

		m_walkStoneAudioClip = (AudioClip)Resources.Load ("StoneFootsteps");

        m_ambientAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
	    m_ambientAudioSource.clip = m_ambientAudioClip;

        m_glidingAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
		m_glidingAudioSource.clip = m_glidingAudioClip;

		m_fallingAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
		m_fallingAudioSource.clip = m_fallingAudioClip;

        m_walkingAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
	    m_walkingAudioSource.clip = m_walkGrassAudioClips[0];

		m_landingAudioSource = (AudioSource)gameObject.AddComponent ("AudioSource");
		m_landingAudioSource.clip = m_walkGrassAudioClips[0];
	}
	
	public void Update()
	{
        handleAmbientAudio();
		handleFallingAudio();
        handleGlidingAudio();
        handleWalkingAudio();
		handleLandingAudio();
	}

	// Plays while falling
	private void handleFallingAudio()
	{
		if (m_controller.state == SeraphState.FALLING)
		{
			FallingController activeController = m_controller.activeController as FallingController;
			
			if (activeController == null)
				return;

			if (!m_fallingAudioSource.isPlaying && activeController.transitionData().velocity > 0.0f)
			{
				m_fallingAudioSource.loop = true;
				m_fallingAudioSource.playOnAwake = false;
				m_fallingAudioSource.Play();
			}
		}
		else
		{
			m_fallingAudioSource.Stop();
		}
	}

    // Plays at all times
    private void handleAmbientAudio()
    {
        if (!m_ambientAudioSource.isPlaying)
        {
            m_ambientAudioSource.loop = true;
            m_ambientAudioSource.volume = 0.5f;
            m_ambientAudioSource.Play();
        }
    }

	// Only plays when landing
	private void handleLandingAudio()
	{
		if (m_controller.state == SeraphState.LANDING)
		{
		    m_landingAudioSource.pitch = m_controller.activeController.transitionData().velocity / 20.0f;
			
			if (!m_landingAudioSource.isPlaying)
			{
				m_landingAudioSource.clip = m_walkGrassAudioClips[Random.Range(1, WALK_VARIANTS)];
				m_landingAudioSource.playOnAwake = false;
				m_landingAudioSource.Play();
			}
		}
		else
		{
			m_landingAudioSource.Stop();
		}
	}

    // Only plays when gliding
    private void handleGlidingAudio()
    {
        if (m_controller.state == SeraphState.GLIDING)
        {
            GlideController activeController = m_controller.activeController as GlideController;

            if (activeController == null)
                return;

            m_glidingAudioSource.volume = (activeController.transitionData().velocity / GlideController.MAX_VELOCITY) * 0.5f;

            if (!m_glidingAudioSource.isPlaying)
            {
                m_glidingAudioSource.loop = true;
                m_glidingAudioSource.playOnAwake = false;
                m_glidingAudioSource.volume = 0.0f;
                m_glidingAudioSource.Play();
            }
        }
        else
        {
            m_glidingAudioSource.Stop();
        }
    }

    // Only plays when walking
    private void handleWalkingAudio()
    {
        if (m_controller.state == SeraphState.GROUNDED)
        {
            GroundController activeController = m_controller.activeController as GroundController;

            if (activeController == null)
                return;

            if (activeController.moved && !m_walkingAudioSource.isPlaying)
			{
				m_walkingAudioSource.loop = false;
                m_walkingAudioSource.playOnAwake = false;

				RaycastHit hit = new RaycastHit();

				if (Physics.Raycast(transform.position, Vector3.down, out hit, 32.0f))
				{
					if(hit.collider.gameObject.tag == "RockObject")
					{
						// Set audio to rock steps
						m_walkingAudioSource.clip = m_walkStoneAudioClip;
					}
					else
					{
						// Set audio to rock steps
						m_walkingAudioSource.clip = m_walkGrassAudioClips[Random.Range(1, WALK_VARIANTS)];
					}
				}
                
				m_walkingAudioSource.pitch = 1.6f;
                m_walkingAudioSource.Play();
            }
        }
        else
        {
            m_walkingAudioSource.Stop();
		}
    }
}
