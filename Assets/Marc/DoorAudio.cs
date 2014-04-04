using UnityEngine;
using System.Collections;
using sv.Triggers;

public class DoorAudio : MonoBehaviour
{
	public AudioClip m_doorAudioClip;
	public float m_sourceSpread;
	public float m_sourceMaxDistance;
	public AudioRolloffMode m_rollOffMode;

	private AudioSource m_doorAudioSource;

	private GameObject m_doorObject;
	
	private DoorOpen m_doorOpen;
	private DoorClose m_doorClose;

	// Use this for initialization
	void Start ()
	{
		this.gameObject.AddComponent<AudioSource> ();
		
		m_doorAudioSource = (AudioSource)gameObject.AddComponent("AudioSource");
		m_doorAudioSource.clip = m_doorAudioClip;
		m_doorAudioSource.rolloffMode = m_rollOffMode;
		m_doorAudioSource.maxDistance = m_sourceMaxDistance;
		m_doorAudioSource.spread = m_sourceSpread;

		m_doorOpen = FindObjectOfType<DoorOpen> ();
		m_doorClose = FindObjectOfType<DoorClose> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		bool doorOpening = m_doorOpen.getDoorOpen () && m_doorOpen != null;
		bool doorClosing = m_doorClose.getDoorClose () && m_doorClose != null;

		if(doorOpening || doorClosing)
		{
			if (!m_doorAudioSource.isPlaying)
			{
				m_doorAudioSource.volume = 10.0f;
				m_doorAudioSource.Play();
			}
		}

	}
}
