// Animation of the impact on the platform.

using System.Collections;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class BulletImpact : MonoBehaviour 
	{
		void OnEnable() {
			StartCoroutine ("CoroutineBulletImpactOnPlatform");
		}

		IEnumerator CoroutineBulletImpactOnPlatform () {
			yield return new WaitForSeconds (0.2f);
			Destroy (gameObject);
		}
					
	}
}
