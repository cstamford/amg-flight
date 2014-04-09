using UnityEngine;

public class Credits : MonoBehaviour
{
	private float offset;
	public float speed = 29.0f;
	public GUIStyle style;
	public Rect viewArea;
	
	private void Start()
	{
		this.offset = this.viewArea.height;
	}
	
	private void Update()
	{
		this.offset -= Time.deltaTime * this.speed;
	}
	
	private void OnGUI()
	{
		GUI.BeginGroup(this.viewArea);
		
		var position = new Rect(0, this.offset, this.viewArea.width, this.viewArea.height);
		var text = @"




















	Flight
     
     
     
    Producer:
	Roy Stevens
     
     
     
    Designer: 
	Hattle
     
     
     
    Lead Programmer:
	Christopher Stamford
     
     
     
    Audio Programmer:
	Marc Dunsmore
	 
	 
	 
	Programmer:
	Louis Dimmock
	Jhonny Foroster
    Sean Viera
     
     
	 
	Audio Engeneer:
	Alex James
	 
	 
	 
	3D Artist:
	Peter Danskin
	Ryan Johnston
	Max Vizard



	Texture Artist:
	Ryan Johnston
	Max Vizard
	 
	
	
	Level Designer:
	Hattle
	 
	 
	 
	Enviromental Artist:
	Ryan Johnston
	 
	 
	 
	Voice Actor:
	Brett Murray - Grandmaster Harune	
	 
	 
	 

	 
	 
	 
	Scrip Writer:
	Hattle
	 
 	 






































	 
   	End";
		
		GUI.Label(position, text, this.style);
		
		GUI.EndGroup();
	}
}