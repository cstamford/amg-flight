using UnityEngine;
using System.Collections;
using cst.Flight;

public class Sound_Follow : MonoBehaviour 
{
	public AudioSource Glide_sound;
	//public Rigidbody Seraph;
	[SerializeField] private GameObject seraph;
	private float playerVelocity;

	// Use this for initialization
	void Start () 
	{
		//transform.position = Seraph.position;
	}
	
	// Update is called once per frame
	void Update () 
	{

		//transform.position = Seraph.position;
		//transform.localPosition += new Vector3 (0.0f,0.0f,20.0f);

		//transform.localPosition += Seraph.velocity;
		//seraph.GetRelativePointVelocity(seraph_speed);
		
		SeraphController seraphController = seraph.GetComponent<SeraphController>();
		//seraphController.state

		playerVelocity = seraphController.SeraphGlideVelocity;
		
		//seraph_speed.magnitude
		if (seraphController.state == SeraphState.GLIDING) 
		{
			Glide_sound.volume = (playerVelocity / 250.0f) * 0.5f;
		}
		else 
		{
			Glide_sound.volume = 0;
		}

	}
}
