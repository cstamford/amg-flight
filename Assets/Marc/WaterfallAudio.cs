using UnityEngine;
using System.Collections;
using sv.Triggers;

public class WaterfallAudio : MonoBehaviour
{
	public AudioClip m_waterfallAudioClip;
	public float m_sourceSpread;
	public float m_sourceMaxDistance;
	public AudioRolloffMode m_rollOffMode;

	private AudioSource m_waterfallAudioSource;

	private GameObject m_waterfallObject;
	private ParticleEmitter m_waterfallEmitter;
	
	private WaterfallHalt m_waterfallHalt;

	// Use this for initialization
	void Start ()
	{
		this.gameObject.AddComponent<AudioSource> ();
		
		m_waterfallAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
		m_waterfallAudioSource.clip = m_waterfallAudioClip;
		m_waterfallAudioSource.rolloffMode = m_rollOffMode;
		m_waterfallAudioSource.maxDistance = m_sourceMaxDistance;
		m_waterfallAudioSource.spread = m_sourceSpread;

		m_waterfallHalt = this.gameObject.GetComponent<WaterfallHalt> ();
		m_waterfallEmitter = this.gameObject.GetComponent<ParticleEmitter> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		bool waterfallHalting = m_waterfallHalt.GetWaterfallHalt () && m_waterfallHalt != null;

		if (!m_waterfallAudioSource.isPlaying)
		{
			m_waterfallAudioSource.Play();
		}

		m_waterfallAudioSource.volume = m_waterfallEmitter.particleCount / 35.0f;

	}
}
