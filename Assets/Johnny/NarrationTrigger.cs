using UnityEngine;
using System.Collections;

public class NarrationTrigger : MonoBehaviour {

	public AudioClip narration;
	public GameObject barrier;

	public AudioSource audioSource;
	private bool narrativeTriggered;

	// Use this for initialization
	void Start () {
		narrativeTriggered = false;
		audioSource.clip = (AudioClip)Resources.Load("footsteps-1");
	}

	void OnTriggerEnter (Collider other) {
		print(other);
		print(other.gameObject.tag);
		if(narrativeTriggered == false)
		{
			if(other.gameObject.tag == "Player")
			{
				//if(audioSource.isPlaying == false)
				//{
					//if(narration != null) {
					//	audioSource.Play();
					//}
					print("Destroying barrier");
					if(barrier != null) {
						Destroy(barrier);
						print("Destroyed barrier");
					}
					narrativeTriggered = true;
				//}
			}
		}
	}
}
