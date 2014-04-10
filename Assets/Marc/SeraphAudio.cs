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
using sv;
using sv.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeraphAudio : MonoBehaviour
{
    private const int WALK_VARIANTS = 15;
	private const int WATER_VARIANTS = 8;
	private const int WOOD_VARIANTS = 3;
	private const int COLLECT_VARIANTS = 8;

    private AudioSource m_ambientAudioSource;
	private AudioSource m_fallingAudioSource;
	private AudioSource m_glidingAudioSource;
    private AudioSource m_walkingAudioSource;
	private AudioSource m_landingAudioSource;
	private AudioSource m_warpingAudioSource;
	private AudioSource m_collectAudioSource;
	private AudioSource m_placementAudioSource;

	private AudioClip m_ambientAudioClip;
	private AudioClip m_fallingAudioClip;
    private AudioClip m_glidingAudioClip;
	private AudioClip m_warpingAudioClip;
	private AudioClip m_placementAudioClip;
	private AudioClip[] m_walkWoodAudioClips = new AudioClip[WOOD_VARIANTS];
	private AudioClip[] m_walkWaterAudioClips = new AudioClip[WATER_VARIANTS];
    private AudioClip[] m_walkGrassAudioClips = new AudioClip[WALK_VARIANTS];
	private AudioClip[] m_walkStoneAudioClips = new AudioClip[WALK_VARIANTS];
	private AudioClip[] m_collectAudioClips = new AudioClip[COLLECT_VARIANTS];

    private SeraphController m_controller;
	private ObjectSelection m_objectSelection;

	public void Start()
	{
	    m_controller = FindObjectOfType<SeraphController>();
		m_objectSelection = FindObjectOfType<ObjectSelection> ();

        if (m_controller == null)
            throw new Exception("No Seraph Controller found on this GameObject.");

		m_ambientAudioClip = (AudioClip)Resources.Load("windy ambience");
		m_fallingAudioClip = (AudioClip)Resources.Load("falling");
        m_glidingAudioClip = (AudioClip)Resources.Load("flight");
		m_warpingAudioClip = (AudioClip)Resources.Load ("warping");
		m_placementAudioClip = (AudioClip)Resources.Load ("placement");
		
		for (int i = 0; i != WALK_VARIANTS; ++i)
		{
			m_walkGrassAudioClips[i] = (AudioClip)Resources.Load(String.Format("grass walk ({0})", i + 1));
			m_walkStoneAudioClips[i] = (AudioClip)Resources.Load(String.Format("stone footstep {0}", i + 1));
		}

		for (int i = 0; i < WATER_VARIANTS - 1; ++i)
		{
			m_walkWaterAudioClips[i] = (AudioClip)Resources.Load(String.Format("water footstep {0}", i + 1));
        }

		for (int i = 0; i < WOOD_VARIANTS - 1; ++i)
		{
			m_walkWoodAudioClips[i] = (AudioClip)Resources.Load(String.Format("wood footstep {0}", i + 1));
		}

		for (int i = 0; i < COLLECT_VARIANTS - 1; ++i)
		{
			m_collectAudioClips[i] = (AudioClip)Resources.Load(String.Format("relic ({0})", i + 1));
		}
		
		m_collectAudioClips[7] = (AudioClip)Resources.Load ("relic collection noise two");









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

		m_collectAudioSource = (AudioSource)gameObject.AddComponent ("AudioSource");

		m_warpingAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
		m_warpingAudioSource.clip = m_warpingAudioClip;

		m_placementAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
		m_placementAudioSource.clip = m_placementAudioClip;
		
	}
	
	public void Update()
	{
        handleAmbientAudio();
		handleFallingAudio();
        handleGlidingAudio();
        handleWalkingAudio();
		handleWarpingAudio();
		handleLandingAudio();
		handleCollectAudio();
		handlePlacementAudio();
	}

	//Plays when collecting the Relics
	private void handleCollectAudio()
	{
		if(m_objectSelection.GetCollection())
		{
			int relicCount = m_objectSelection.GetCollectionCount();

			switch( m_objectSelection.GetCollectionName() )
			{
				case "Wings":
					
					m_collectAudioSource.volume = 0.01f;
					m_collectAudioSource.clip = m_collectAudioClips[7];					
					break;

				default:
					m_collectAudioSource.volume = 1.0f;
					m_collectAudioSource.clip = m_collectAudioClips[relicCount-1];
					break;
			}

			m_collectAudioSource.loop = false;
			m_collectAudioSource.playOnAwake = false;
			m_collectAudioSource.Play ();
		}
	}

	//Plays while placing the relics
	private void handlePlacementAudio()
	{
		if(m_objectSelection.RelicPlaced())
		{
			m_placementAudioSource.loop = false;
			m_placementAudioSource.playOnAwake = false;
			m_placementAudioSource.Play ();
		}
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

	//Only plays when warping
	private void handleWarpingAudio()
	{
		if (m_controller.state == SeraphState.WARPING)
		{
			if (!m_warpingAudioSource.isPlaying)
			{
				m_warpingAudioSource.loop = true;
				m_warpingAudioSource.Play();
			}
		}
	}
	// Only plays when landing
	private void handleLandingAudio()
	{
		if (m_controller.state == SeraphState.LANDING)
		{
		    m_landingAudioSource.pitch = m_controller.activeController.transitionData().velocity * 1.5f;
			m_landingAudioSource.volume = 0.3f;

			if (!m_landingAudioSource.isPlaying)
            {
				RaycastHit hit = new RaycastHit();

				if (Physics.Raycast(transform.position, Vector3.down, out hit, 32.0f))
				{
					if(hit.collider.gameObject.tag == "RockObject")
					{
						// Set audio to rock steps
						m_landingAudioSource.clip = m_walkStoneAudioClips[Random.Range(1, WALK_VARIANTS)];
					}
					else if(hit.collider.gameObject.tag == "WaterObject")
					{
						// Set audio to water steps
						m_landingAudioSource.clip = m_walkWaterAudioClips[Random.Range (1, WATER_VARIANTS)];
					}
					else
					{
						// Set audio to grass steps
						m_landingAudioSource.clip = m_walkGrassAudioClips[Random.Range(1, WALK_VARIANTS)];
	                }
				}

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
			m_warpingAudioSource.Stop();
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
						m_walkingAudioSource.clip = m_walkStoneAudioClips[Random.Range(1, WALK_VARIANTS)];
						m_walkingAudioSource.pitch = 1.6f;
						m_walkingAudioSource.volume = 0.3f;
					}
					else if(hit.collider.gameObject.tag == "WaterObject")
					{
						// Set audio to water steps
						m_walkingAudioSource.clip = m_walkWaterAudioClips[Random.Range (1, WATER_VARIANTS)];
						m_walkingAudioSource.pitch = 0.5f;
						m_walkingAudioSource.volume = 0.3f;
					}
					else if(hit.collider.gameObject.tag == "WoodObject")
					{
						// Set audio to wood steps
						m_walkingAudioSource.clip = m_walkWoodAudioClips[Random.Range (1, WOOD_VARIANTS)];
						m_walkingAudioSource.pitch = 2.3f;
						m_walkingAudioSource.volume = 0.3f;
					}
					else
					{
						// Set audio to grass steps
						m_walkingAudioSource.clip = m_walkGrassAudioClips[Random.Range(1, WALK_VARIANTS)];
						m_walkingAudioSource.pitch = 2.3f;
						m_walkingAudioSource.volume = 0.2f;
					}
				}
                

                m_walkingAudioSource.Play();
            }
        }
        else
        {
            m_walkingAudioSource.Stop();
		}
    }
}
