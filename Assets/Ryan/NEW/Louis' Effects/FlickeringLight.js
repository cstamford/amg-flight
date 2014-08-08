#pragma strict
	
// How quickly the light flickers
public var minFlickerSpeed : float = 0.01;
public var maxFlickerSpeed : float = 0.2;

// How much to change the light intensity by when flickering
public var minLightRange : float = -0.2;
public var maxLightRange : float = 0.2;

// Min and max light intensity
public var minLightIntensity : float = 0.5;
public var maxLightIntensity : float = 1;

function Start ()
{
	while (true)
	{
		// Apply a random value to the intensity
		light.intensity += Random.Range(minLightRange, maxLightRange);

		// Cap the intensity between two values
		if(light.intensity < minLightIntensity)
		light.intensity = minLightIntensity;
		else if(light.intensity > maxLightIntensity)
		light.intensity = maxLightIntensity;

		// Pause for a random time
		yield WaitForSeconds (Random.Range(minFlickerSpeed, maxFlickerSpeed ));
	}
}
 
function Update()
{
	// Nothing to do here
}