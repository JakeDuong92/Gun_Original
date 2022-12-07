// Create weapons, control movement and shooting.

using System.Collections;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class WeaponCharacters : MonoBehaviour 
	{
		private bool isEnableGame = false;
		private float countUpDownWeapon = 0.0f;
		private bool isRunningUpDownWeapon = false;
		public GameObject bulletPrefab; 
		public GameObject rightBulletStand; 
		public GameObject leftBulletStand;
		private ToolsGame objToolsGame;
		public float heightBullet = 10.0f;
		public float speedBullet = 18.0f;
		public float heightFlame = 25.0f;
		private bool flipStatusPlayer = false; //  Looking to the right.
		public GameObject flamePrefab; // The flame of shot.
		public GameObject cartridgePrefab; // Bullet cartridge.
		public GameObject LeftCartridgeStand; // Left position of the cartridge.
		public GameObject RightCartridgeStand; // Right position of the cartridge.
		public float heightCartridge = 5.0f;
		public float xForceCartridge = 300.0f;
		public float yForceCartridge = 300.0f;
		public float recoilPlayer = 0.03f;
		private SpriteRenderer objWeaponSpriteRenderer = null;
		private int modelWeapon;
		private float shootingTime = 0.0f;
		private float shootingTimeTmp = 0.0f;
		[Header("Gun")]
		public bool enableGun = true;
		public float shootTimeGun = 1.0f;
		public float recoilGun = 16.0f;
		public int ammunitionGun = 54; // Gun ammo.
		[Header("MachineGun")]
		public bool enableMachineGun = false;
		public float shootTimeMaGun = 0.5f;
		public float recoilMaGun = 20.0f;
		public int ammunitionMaGun = 200; // MachineGun ammo.
		[Header("Bazooka")]
		public bool enableBazooka = false;
		public float shootTimeBazooka = 2.0f;
		public float recoilBazooka = 30.0f;
		public int ammunitionBazooka = 10; // Bazzoka ammo.
		private bool isEnableShooting = false;
		private AudioController objAudioController = null;
		private UIScoreController objUIScoreController = null;
		private int maxAmmunition = 0;
		private int descAmmunition = 0;
		private LayerMask maskLayerRaycast;
		private PlayerController objPlayerController;
		private BotController objBotController;
		private string tagPlayer;
		private CameraController objCameraController;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
			objUIScoreController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIScoreController> ();
			objPlayerController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<PlayerController> ();
			objCameraController = Camera.main.GetComponent<CameraController> ();
			if (enableGun) {
				modelWeapon = GlobalEnvironment.idGunWeapon;
			} else if (enableMachineGun) {
				modelWeapon = GlobalEnvironment.idMachineGunWeapon;
			} else if (enableBazooka) {
				modelWeapon = GlobalEnvironment.idBazookaWeapon;
			} else {
				modelWeapon = GlobalEnvironment.idGunWeapon;
			}
			SetModelWeapon (modelWeapon);
		}

		// Use this for initialization.
		void Start () {
			objWeaponSpriteRenderer = GetComponent <SpriteRenderer>();
			EnableCoroutineUpDownWeapon (true);
		}

		// Update is called once per frame.
		void Update () {
			if (!isEnableGame) {
				return;
			}
			if (isEnableShooting) {
				shootingTimeTmp -= Time.deltaTime;
				if (shootingTimeTmp < 0) {
					isEnableShooting = false;
				}
			} 

		}

		void FixedUpdate() {
			if (!isEnableGame) {
				return;
			}
		}

		void LateUpdate() {
		
		}

		public void SetPositionWeapon (Vector2 positionWeapon, bool idleStatusAnim, bool jumpStatusAnim, bool crouchStatusAnim, Vector2 positionCrouch) {
			Vector2 positionTmpWeapon = Vector2.zero;
			if (jumpStatusAnim) {
				EnableCoroutineUpDownWeapon (false);
				positionTmpWeapon = new Vector2 (positionWeapon.x, positionWeapon.y);
			} else {
				EnableCoroutineUpDownWeapon (true);
				positionTmpWeapon = new Vector2 (positionWeapon.x, positionWeapon.y + countUpDownWeapon);
			}
			if (crouchStatusAnim && !jumpStatusAnim) {
				positionTmpWeapon = new Vector2 (positionWeapon.x, positionWeapon.y + countUpDownWeapon - positionCrouch.y);
			}
			transform.position = positionTmpWeapon;
		}

		// Enable weapon motion animation.
		void EnableCoroutineUpDownWeapon (bool bValue) {
			if (bValue) {
				if (!isRunningUpDownWeapon) {
					StartCoroutine ("CoroutineUpDownWeapon");
					isRunningUpDownWeapon = true;
				}
			} else {
				if (isRunningUpDownWeapon) {
					StopCoroutine ("CoroutineUpDownWeapon");
					isRunningUpDownWeapon = false;
				}
			}
		}

		IEnumerator CoroutineUpDownWeapon () {
			float countTmp = 1.0f;
			float maxCount = (0.05f * Screen.height) / GlobalEnvironment.resolutionHeight;
			float speedCount = (0.02f * Screen.height) / GlobalEnvironment.resolutionHeight;
			while (true) {
				yield return new WaitForSeconds (0.2f);
				countUpDownWeapon += (speedCount * countTmp);
				if (countUpDownWeapon >= maxCount) {
					countTmp = -1.0f;
				} else if (countUpDownWeapon <= 0.0f) {
					countTmp = 1.0f;
				}
			}
		}

		public void shootWeapon (int modeShoot, float xPositionShoot, float yPositionShoot, float degreesShoot) {
			if (!bulletPrefab || !rightBulletStand || isEnableShooting) {
				return;
			}
			if (!IsRaycastEnemy (xPositionShoot, yPositionShoot)) {
				return;
			}
			if (descAmmunition == 0) {
				objAudioController.PlaySounds (GlobalEnvironment.idWithoutAmmunitionSound);
				return;
			}
			isEnableShooting = true;
			shootingTimeTmp = shootingTime;
			float directionBullet = 1.0f;
			Vector2 positionBulletPlayer = Vector2.zero;
			if (flipStatusPlayer) {
				positionBulletPlayer = new Vector2 (leftBulletStand.transform.position.x, leftBulletStand.transform.position.y);
				directionBullet = -1.0f;
			} else {
				positionBulletPlayer = new Vector2 (rightBulletStand.transform.position.x, rightBulletStand.transform.position.y);
				directionBullet = 1.0f;
			}
			GameObject getObj = Instantiate (bulletPrefab, positionBulletPlayer, bulletPrefab.transform.rotation) as GameObject;
			if (getObj) {
				SpriteRenderer objBulletSpriteRenderer = getObj.GetComponent <SpriteRenderer>();
				float widthSprite = objBulletSpriteRenderer.sprite.rect.width;
				float heightSprite = objBulletSpriteRenderer.sprite.rect.height;
				float heightNewBullet = heightBullet;
				float widthNewBullet = heightNewBullet * (widthSprite / heightSprite);
				objToolsGame.SetScaleSprite (getObj, widthNewBullet, heightNewBullet);
				objToolsGame.SetParentSprite (getObj);
				objToolsGame.SetRotateSprite (getObj, degreesShoot, false);
				var getBulletWeapon = getObj.GetComponent<BulletWeapon> ();
				getBulletWeapon.SetModelWeapon (modelWeapon);
				getBulletWeapon.SetTagPlayer (tagPlayer);
				Rigidbody2D rigidbodyBullet = getObj.GetComponent<Rigidbody2D> ();
				if (rigidbodyBullet) {
					rigidbodyBullet.AddRelativeForce (new Vector2 (xPositionShoot * speedBullet, yPositionShoot * speedBullet)); 
				}
				Destroy (getObj, 0.6f);
				CreateFlame (positionBulletPlayer, directionBullet, 0, degreesShoot);
				CreateCartridge (degreesShoot);
				RecoilWeapon (degreesShoot);
				if (modelWeapon == GlobalEnvironment.idBazookaWeapon) {
					objCameraController.ShakeCamera ();
				}
				PlaySoundWeapon (modelWeapon);
				ShotsFired ();
			}
		}

		// Detect the enemy with Raycast.
		bool IsRaycastEnemy (float xPosition, float yPosition) {
			bool isEnemy = false;
			RaycastHit2D hitRaycast = Physics2D.Raycast(transform.position, new Vector2 (xPosition, yPosition), Mathf.Infinity, maskLayerRaycast);
			#if UNITY_EDITOR
				//Debug.DrawLine(transform.position, new Vector2 (transform.position.x + (10 * xPosition), transform.position.y + (10 * yPosition)), Color.red, 3f, false);
			#endif
			if (hitRaycast.collider != null) {
				float widthCamera = objToolsGame.GetWidthCamera ();
				float distancePlayer = Mathf.Abs(hitRaycast.point.x - transform.position.x);
				bool isObjVisible = objToolsGame.IsVisibleInCamera (hitRaycast.transform.gameObject);
				if ( distancePlayer < widthCamera && isObjVisible) {
					isEnemy = true;
				}
			}
			return isEnemy;
		}

		public void SetMaskLayerRaycast (LayerMask maskRaycast) {
			maskLayerRaycast = maskRaycast;
		}

		public void SetFlipPlayer (bool bValue) {
			flipStatusPlayer = bValue;
			if (objWeaponSpriteRenderer) {
				objWeaponSpriteRenderer.flipX = bValue;
			}
		}

		public void SetTagPlayer (string tagValue) {
			tagPlayer = tagValue;
		}

		// Create the flame when shooting.
		void CreateFlame (Vector2 positionFlame, float directionFlame, int modeFlame, float degreesShoot) {
			if (!flamePrefab) {
				return;
			}
			GameObject getObj = Instantiate (flamePrefab, positionFlame, flamePrefab.transform.rotation) as GameObject;
			if (getObj) {
				SpriteRenderer objBulletSpriteRenderer = getObj.GetComponent <SpriteRenderer>();
				objBulletSpriteRenderer.flipX = false;
				if (directionFlame == -1.0f) {
					objBulletSpriteRenderer.flipX = true;
				}
				float widthSprite = objBulletSpriteRenderer.sprite.rect.width;
				float heightSprite = objBulletSpriteRenderer.sprite.rect.height;
				float heightNewFlame = heightFlame;
				float widthNewFlame = heightNewFlame * (widthSprite / heightSprite);
				objToolsGame.SetScaleSprite (getObj, widthNewFlame, heightNewFlame);
				objToolsGame.SetParentSprite (getObj);
				objToolsGame.SetRotateSprite (getObj, degreesShoot, false);
				StartCoroutine ("CoroutineFlameWeapon", getObj);
				Destroy (getObj, 0.1f);
			}
		}
			
		IEnumerator CoroutineFlameWeapon (GameObject objFlame) {
			bool isRunningCortn = true;
			Vector2 positionMovementFlame = Vector2.zero;
			while (isRunningCortn) {
				yield return new WaitForSeconds (0.01f);
				if (objFlame) {
					if (flipStatusPlayer) {
						objFlame.transform.position = leftBulletStand.transform.position;
					} else {
						objFlame.transform.position = rightBulletStand.transform.position;
					}

				} else {
					isRunningCortn = false;
				}
			}
		}

		void RecoilWeapon (float degreesShoot) {
			StartCoroutine ("CoroutineRecoilWeapon", degreesShoot);
		}

		IEnumerator CoroutineRecoilWeapon (float degreesShoot) {
			bool isRunningCortn = true;
			float degreesPlus = 0.0f;
			float degreesPlusTmp = 0.0f;
			float direcctionDesgrees = 1.0f;
			switch (modelWeapon) {
			case GlobalEnvironment.idGunWeapon:
				degreesPlus = recoilGun;
				break;
			case GlobalEnvironment.idMachineGunWeapon:
				degreesPlus = recoilMaGun;
				break;
			case GlobalEnvironment.idBazookaWeapon:
				degreesPlus = recoilBazooka;
				break;
			default:
				degreesPlus = recoilGun;
				break;
			}
			while (isRunningCortn) {
				degreesPlusTmp += ((degreesPlus / 2) * direcctionDesgrees); 
				if (degreesPlusTmp >= degreesPlus) {
					direcctionDesgrees = -1.0f;
				}
				if (direcctionDesgrees == -1.0f && degreesPlusTmp <= 0.0f) {
					break;
				}
				objToolsGame.SetRotateSprite (gameObject, degreesShoot + degreesPlusTmp, false);
				yield return new WaitForSeconds (0.01f);
			}
		}

		void CreateCartridge (float degreesShoot) {
			GameObject getObj;
			if (modelWeapon == GlobalEnvironment.idBazookaWeapon) {
				if (flipStatusPlayer) {
					getObj = Instantiate (cartridgePrefab, RightCartridgeStand.transform.position, RightCartridgeStand.transform.rotation) as GameObject;
				} else {
					getObj = Instantiate (cartridgePrefab, LeftCartridgeStand.transform.position, LeftCartridgeStand.transform.rotation) as GameObject;
				}
			} else {
				getObj = Instantiate (cartridgePrefab, LeftCartridgeStand.transform.position, LeftCartridgeStand.transform.rotation) as GameObject;
			}
			if (getObj) {
				SpriteRenderer objBulletSpriteRenderer = getObj.GetComponent <SpriteRenderer>();
				float widthSprite = objBulletSpriteRenderer.sprite.rect.width;
				float heightSprite = objBulletSpriteRenderer.sprite.rect.height;
				float heightNewBullet = heightCartridge;
				float widthNewBullet = heightNewBullet * (widthSprite / heightSprite);
				objToolsGame.SetScaleSprite (getObj, widthNewBullet, heightNewBullet);
				objToolsGame.SetParentSprite (getObj);
				var objCartridge = getObj.GetComponent <CartridgeWeapon>();
				if (objCartridge) {
					if (string.Equals (tagPlayer, GlobalEnvironment.idBotTag)) {
						getObj.layer = LayerMask.NameToLayer (GlobalEnvironment.idBotsLayer);
					}
					objCartridge.SetFlipPlayer (flipStatusPlayer);
					objCartridge.StartCartridge (xForceCartridge, yForceCartridge);
				}
			}
			float recoilForceTmp = (recoilPlayer * Screen.width) / GlobalEnvironment.resolutionWidth;
			if (string.Equals(tagPlayer, GlobalEnvironment.idPlayerTag)) {
				objPlayerController.RecoilPlayer (recoilForceTmp);
			}
			if (string.Equals(tagPlayer, GlobalEnvironment.idBotTag)) {
				objBotController.RecoilPlayer (recoilForceTmp);
			}

		}

		// Shots taken.
		void ShotsFired () {
			if (string.Equals (tagPlayer, GlobalEnvironment.idPlayerTag)) {
				descAmmunition--;
				if (string.Equals(tagPlayer, GlobalEnvironment.idPlayerTag)) {
					objUIScoreController.UpdateAmmunition (descAmmunition);
				}
			}
		}

		public void sendEventTouchShots (float xPosition, float yPosition, float maxDistance) {
			if (!isEnableGame) {
				return;
			}
			float degreesJoystickShoot = 0.0f;
			float getTangente = 0.0f;
			float getRadians = 0.0f;
			if (xPosition == 0 && yPosition == 0 && maxDistance == 0) {
				objToolsGame.SetRotateSprite (gameObject, 0.0f, true);
			} else {
				// First quadrant
				if (xPosition >= 0 && yPosition >= 0 && maxDistance > 0 && !flipStatusPlayer) {
					getTangente = yPosition / xPosition;
					getRadians = Mathf.Atan (getTangente);
					degreesJoystickShoot = Mathf.Rad2Deg * getRadians;
					objToolsGame.SetRotateSprite (gameObject, degreesJoystickShoot, false);
					shootWeapon (0, xPosition, yPosition, degreesJoystickShoot);
				}
				// Fourth quadrant
				if (xPosition >= 0 && yPosition < 0 && !flipStatusPlayer) {
					getTangente = Mathf.Abs (yPosition) / xPosition;
					getRadians = Mathf.Atan (getTangente);
					degreesJoystickShoot = (90.0f - (getRadians * Mathf.Rad2Deg)) + 270.0f;
					objToolsGame.SetRotateSprite (gameObject, degreesJoystickShoot, false);
					shootWeapon (0, xPosition, yPosition, degreesJoystickShoot);
				}
				// Second quadrant
				if (xPosition < 0 && yPosition >= 0 && flipStatusPlayer) {
					getTangente = yPosition / Mathf.Abs (xPosition);
					getRadians = Mathf.Atan (getTangente);
					degreesJoystickShoot = (90.0f - (getRadians * Mathf.Rad2Deg)) -  90.0f;
					objToolsGame.SetRotateSprite (gameObject, degreesJoystickShoot, false);
					shootWeapon (0, xPosition, yPosition, degreesJoystickShoot);
				}
				// Third quadrant
				if (xPosition < 0 && yPosition < 0 && flipStatusPlayer) {
					getTangente = Mathf.Abs (yPosition) / Mathf.Abs (xPosition);
					getRadians = Mathf.Atan (getTangente);
					degreesJoystickShoot = (getRadians * Mathf.Rad2Deg);
					objToolsGame.SetRotateSprite (gameObject, degreesJoystickShoot, false);
					shootWeapon (0, xPosition, yPosition, degreesJoystickShoot);
				}
			}
		}

		public void SetModelWeapon (int idWeapon) {
			modelWeapon = idWeapon;
			switch (idWeapon) {
			case GlobalEnvironment.idGunWeapon:
				shootingTime = shootTimeGun;
				maxAmmunition = ammunitionGun;
				descAmmunition = maxAmmunition;
				break;
			case GlobalEnvironment.idMachineGunWeapon:
				shootingTime = shootTimeMaGun;
				maxAmmunition = ammunitionMaGun;
				descAmmunition = maxAmmunition;
				break;
			case GlobalEnvironment.idBazookaWeapon:
				shootingTime = shootTimeBazooka;
				maxAmmunition = ammunitionBazooka;
				descAmmunition = maxAmmunition;
				break;
			default:
				shootingTime = shootTimeGun;
				maxAmmunition = ammunitionGun;
				descAmmunition = maxAmmunition;
				break;
			}
			shootingTimeTmp = shootingTime;
			if (string.Equals(tagPlayer, GlobalEnvironment.idPlayerTag)) {
				objUIScoreController.UpdateAmmunition (descAmmunition);
				objUIScoreController.UpdateWeapon (modelWeapon);
			}
		}

		// Returns the id of weapon.
		public int getModelWeapon () {
			return modelWeapon;
		
		}

		public void SetBotController (GameObject objValue) {
			objBotController = objValue.GetComponent<BotController> ();
		}

		// Reload the ammo.
		// Return true = if it recharges, false = does not recharge. 
		public bool RechargeAmmunition (int ammunitionReload) {
			bool bValue = true;
			if (descAmmunition == maxAmmunition) {
				bValue = false;
			}
			int descAmmunitionTmp = descAmmunition + ammunitionReload;
			if (descAmmunitionTmp > maxAmmunition) {
				descAmmunitionTmp = maxAmmunition;
			}
			descAmmunition = descAmmunitionTmp;
			if (string.Equals(tagPlayer, GlobalEnvironment.idPlayerTag)) {
				objUIScoreController.UpdateAmmunition (descAmmunition);
			}
			return bValue;
		}

		void PlaySoundWeapon (int idWeapon) {
			switch (idWeapon) {
			case GlobalEnvironment.idGunWeapon:
				objAudioController.PlaySounds (GlobalEnvironment.idGunShootSound);
				break;
			case GlobalEnvironment.idMachineGunWeapon:
				objAudioController.PlaySounds (GlobalEnvironment.idMachineGunShootSound);
				break;
			case GlobalEnvironment.idBazookaWeapon:
				objAudioController.PlaySounds (GlobalEnvironment.idBazookaShootSound);
				break;
			default:
				objAudioController.PlaySounds (GlobalEnvironment.idGunShootSound);
				break;
			}
			shootingTimeTmp = shootingTime;
		}

		// Enable the game.
		public void EnableGame (bool bGame) {
			isEnableGame = bGame;
		}

		public void ShutdownWeapon () {
			gameObject.AddComponent <Rigidbody2D> ();
			gameObject.AddComponent <BoxCollider2D> ().isTrigger = true;
		}

		void OnTriggerEnter2D(Collider2D otherobjs) {
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idPlatformTag)) {
				if (gameObject.GetComponent <BoxCollider2D> ().isTrigger) {
					gameObject.GetComponent <BoxCollider2D> ().isTrigger = false;
					gameObject.GetComponent <Rigidbody2D> ().isKinematic = true;
					Destroy (gameObject.GetComponent <Rigidbody2D> ());
					Destroy (gameObject, 1.0f);
				}
			}
		}

		// Returns the initial ammo.
		public int GetInitAmmunition () {
			return maxAmmunition;
		}

		// Returns the current ammo.
		public int GetCurrentAmmunition () {
			return descAmmunition;
		}

	}
}
