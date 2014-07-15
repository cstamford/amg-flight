//
// Filename: Helper.cs
// Author: Louis Dimmock
// Date : 16th January 2014
//
// Current Version: 1.0
// Version information : 
//		Allows common functionality to be shared amongst classes.
//

using UnityEngine;
using System.Collections;

namespace Louis.Common
{
	public static class Helper
	{
		// Wrap a value between a minimum and maximum
		public static void Wrap(ref float angle, float minimum, float maximum)
		{
			// Wrap back to 360
			if (angle < minimum)
			{
				angle += maximum;
			}
			else if (angle > maximum) // Wrap back to 0
			{
				angle -= maximum;
			}
		}

		// Limits a value to stay between two values
		public static void Limit(ref float value, float minimum, float maximum)
		{
			// If the value is too low
			if( value < minimum )
			{
				// Set to the minimum
				value = minimum;
			}
			else if( value > maximum ) // If the value is too high
			{
				// Set to the maximum
				value = maximum;
			}
		}
	}
}