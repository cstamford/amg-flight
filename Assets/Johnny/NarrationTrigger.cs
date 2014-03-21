using UnityEngine;
using System.Collections;

public class NarrationTrigger : MonoBehaviour {

	public AudioClip narration;
	public GameObject barrier;

	private AudioSource audioSource;
	private bool narrativeTriggered;

	// Use this for initialization
	void Start () {
		narrativeTriggered = false;
		audioSource = GameObject.Find("Seraph").transform.gameObject.AddComponent<AudioSource>();
		//audioSource = (AudioSource)Camera.main.transform.gameObject.GetComponent(typeof(AudioSource));
		print(audioSource);
		audioSource.clip = Resources.Load("Gliding") as AudioClip;
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
