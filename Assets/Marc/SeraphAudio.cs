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

    private AudioClip m_ambientAudioClip;
    private AudioClip m_glidingAudioClip;
    private readonly AudioClip[] m_walkGrassAudioClips = new AudioClip[WALK_VARIANTS];

    private SeraphController m_controller;

	public void Start()
	{
	    m_controller = FindObjectOfType<SeraphController>();

        if (m_controller == null)
            throw new Exception("No Seraph Controller found on this GameObject.");

        m_ambientAudioClip = (AudioClip)Resources.Load("windy ambience");
        m_glidingAudioClip = (AudioClip)Resources.Load("temporary flying noise");

        for (int i = 0; i != WALK_VARIANTS; ++i)
        {
            m_walkGrassAudioClips[i] = (AudioClip)Resources.Load(String.Format("grass walk ({0})", i + 1));
        }

        m_ambientAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
	    m_ambientAudioSource.clip = m_ambientAudioClip;

        m_glidingAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
        m_glidingAudioSource.clip = m_glidingAudioClip;

        m_walkingAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
	    m_walkingAudioSource.clip = m_walkGrassAudioClips[0];
	}
	
	public void Update()
	{
        handleAmbientAudio();
        handleGlidingAudio();
        handleWalkingAudio();
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

    // Only plays when gliding
    private void handleGlidingAudio()
    {
        if (m_controller.state == SeraphState.GLIDING)
        {
            GlideController activeController = m_controller.activeController as GlideController;

            if (activeController == null)
                return;

            m_glidingAudioSource.volume = (activeController.forwardSpeed / GlideController.MAX_VELOCITY) * 0.5f;

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
                m_walkingAudioSource.clip = m_walkGrassAudioClips[Random.Range(1, WALK_VARIANTS)];
                Debug.Log(m_walkingAudioSource.clip);
                m_walkingAudioSource.Play();
            }
        }
        else
        {
            m_walkingAudioSource.Stop();
        }
    }
}
