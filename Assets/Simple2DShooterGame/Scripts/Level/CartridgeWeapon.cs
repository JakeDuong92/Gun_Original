// Create the cartridge when firing.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class CartridgeWeapon : MonoBehaviour 
	{
		private bool flipStatusPlayer = false; // Looking to the right.
		private SpriteRenderer objCartridgeSpriteRenderer = null;
		private float destroyTime = 3.0f; // Timeout for the cartridge to be destroyed.

		void Awake () {
			gameObject.name = GlobalEnvironment.idUniqueCartridgeWeapon;
			objCartridgeSpriteRenderer = GetComponent <SpriteRenderer>();
		}

		public void SetFlipPlayer (bool bValue) {
			flipStatusPlayer = bValue;
			if (objCartridgeSpriteRenderer) {
				objCartridgeSpriteRenderer.flipX = bValue;
			}
		}

		public void StartCartridge (float xForce, float yForce) {
			float directionForce = 1.0f;
			if (flipStatusPlayer) {
				directionForce = 1.0f;
			} else {
				directionForce = -1.0f;
			}
			Rigidbody2D rigidbodyBullet = GetComponent<Rigidbody2D> ();
			if (rigidbodyBullet) {
				rigidbodyBullet.AddRelativeForce (new Vector2 (xForce * directionForce, yForce)); 
			}
			Destroy (gameObject, destroyTime);
		}

	}
}
