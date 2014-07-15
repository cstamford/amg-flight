//
// Filename : RelicController.cs
// Author : Louis Dimmock
// Date : 10th Feburary 2014
//
// Version : 1.0
// Version Info : 
// 		Simple script that provides visual functionality to the relic.
//		Rotates the relic around its centre pivot by a set speed.
//		Moves the relic up and down using a Sine wave based.
//

using Louis.Common;
using UnityEngine;
using System.Collections;

namespace Louis.Relics
{
	public class RelicController : MonoBehaviour
	{
		// Start position for the relic
		private Vector3 m_startPosition;

		// The offset for moving up and down
		private Vector3 m_offsetPosition;

		// Speed that the relic rotates at
		public float m_rotationSpeed = 15.0f;

		// Current rotation
		private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);

		// Frames counter
		private float m_frameCount = 0.0f;

		// Use this for initialization
		void Start ()
		{
			// Retrieve the objects positions
			m_startPosition = transform.position;
			m_rotation = transform.eulerAngles;
		}
		
		// Update is called once per frame
		void Update ()
		{
			// Apply rotation
			m_rotation.y += m_rotationSpeed * Time.deltaTime;

			// Make sure rotation remains between 0 and 360
			Helper.Wrap(ref m_rotation.y, 0.0f, 360.0f);

			m_offsetPosition.y = Mathf.Sin( m_frameCount++ / 30 ) / 3;

			// Apply transformation to object
			transform.position = m_startPosition + m_offsetPosition;
			transform.eulerAngles = m_rotation;
		}
	}
}