// Animation, player explosion.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class PlayerExplosion : MonoBehaviour 
	{
		private Animator objAnimPlayer = null;

		void Awake () {
			objAnimPlayer = GetComponent <Animator>();
		}

		void LateUpdate() {
			if (!objAnimPlayer) {
				return;
			}
			if (objAnimPlayer.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
				Destroy (gameObject);
			}
		}

	}
}
