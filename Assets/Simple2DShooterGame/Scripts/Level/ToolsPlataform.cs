// Animation of the impact of bullet on platform.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class ToolsPlataform : MonoBehaviour 
	{

		public bool stopBullet = true; // true = stops bullet, false = doesn't stop bullet.
		public GameObject bulletImpactPrefab;
		public float heightGun = 20.0f;
		public float heightMachineGun = 40.0f;
		public float heightBazooka = 80.0f;
		private ToolsGame objToolsGame;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
		}

		void OnTriggerEnter2D(Collider2D otherobjs) {
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idBulletTag)) {
				BulletWeapon (otherobjs.gameObject);
				if (stopBullet) {
					Destroy (otherobjs.gameObject);
				}
			}
		}

		void OnTriggerStay2D(Collider2D otherobjs) {
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idBulletTag)) {
				if (!stopBullet) {
					BulletWeapon (otherobjs.gameObject);
				}
			}
		}

		void BulletWeapon (GameObject objCollider) {
			if (!bulletImpactPrefab) {
				return;
			}
			float heightBulletImpact = heightGun;
			var getBulletWeapon = objCollider.GetComponent<BulletWeapon> ();
			int getWeapon = 0;
			if (getBulletWeapon) {
				getWeapon = getBulletWeapon.GetModelWeapon ();
			}
			switch (getWeapon) {
			case GlobalEnvironment.idGunWeapon:
				heightBulletImpact = heightGun;
				break;
			case GlobalEnvironment.idMachineGunWeapon:
				heightBulletImpact = heightMachineGun;
				break;
			case GlobalEnvironment.idBazookaWeapon:
				heightBulletImpact = heightBazooka;
				break;
			default :
				heightBulletImpact = heightGun;
				break;
			}
			GameObject getObj = Instantiate (bulletImpactPrefab, objCollider.transform.position, objCollider.transform.rotation) as GameObject;
			if (getObj) {
				SpriteRenderer objBulletSpriteRenderer = getObj.GetComponent <SpriteRenderer>();
				float widthSprite = objBulletSpriteRenderer.sprite.rect.width;
				float heightSprite = objBulletSpriteRenderer.sprite.rect.height;
				float heightNewBullet = heightBulletImpact;
				float widthNewBullet = heightNewBullet * (widthSprite / heightSprite);
				float scaleFactor = Random.Range (0.5f, 1.0f);
				objToolsGame.SetScaleSprite (getObj, widthNewBullet * scaleFactor, heightNewBullet * scaleFactor);
				float rotationDegrees = Random.Range (0.0f, 180.0f);
				objToolsGame.SetRotateSprite (getObj, rotationDegrees, false);
				objToolsGame.SetParentSprite (getObj);
			}
		}
	}
}
