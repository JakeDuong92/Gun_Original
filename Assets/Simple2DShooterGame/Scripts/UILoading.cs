// Rotation of the level loading sprite.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{	
	public class UILoading : MonoBehaviour 
	{
		// Update is called once per frame
		void Update () {
			transform.Rotate (new Vector3 (0.0f, 0.0f, -200.0f * Time.deltaTime));
		}
	}
}
