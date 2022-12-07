// Bot controller.

using System.Collections;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class BotController : MonoBehaviour 
	{
		private bool isEnableGame = false;
		private ToolsGame objToolsGame;
		private float widthPlayer = 140.0f;
		private float heightPlayer = 140.0f;
		private float xPositionPlayer = -100.0f;
		private float yPositionPlayer = -220.0f;
		private float xMoveJoystickMotion = 0.0f;
		private bool jumpJoystickMotion = false;
		private bool jumpTmpJoystickMotion = false;
		private BoxCollider2D boxColliderPlayer;
		private CircleCollider2D circleColliderPlayer;
		private Vector2 velocityPlayer;
		private bool groundPlayer = false;
		private float speedPlayer = 8.0f; 
		private float maxDegrees = 90.0f;
		private float minDegrees = 0.0f;
		private float jumpSpeed = 30.0f;
		private float jumpPlayer = 3.0f; 
		private float jumpCurrentPlayer = 3.0f;
		private GameObject objPlayer = null;
		private SpriteRenderer objSpriteRenderer = null;
		private Animator objAnimPlayer = null;
		private float groundSpeed = 70.0f;
		private float acceleratePlayer = 75.0f;
		private float speedDeltaPlayer = 0.02f;
		private bool idleStatusAnimation = true;
		private bool jumpStatusAnimation = false;
		private bool updownStatusAnimation = false;
		private bool crouchStatusAnimation = false;
		private GameObject objWeaponStand = null; // Weapon support.
		private GameObject objWeaponSelect = null; // Selected weapon.
		private SpriteRenderer objWeaponSpriteRenderer = null;
		private WeaponCharacters objWeaponCharacters = null;
		private Vector2 positionWalking;
		private float jumpDifference = 0.0f;
		private Vector2 positionCrouch;
		private Collider2D[] getOverlaps;
		private int lenghtBoxCollider = 50;
		private AudioController objAudioController = null;
		private BotProperties propertiesPlayer = null;
		private float widthCamera = 0;
		private PlayerController objPlayerController;
		private bool isEnableDeathPlayer = false;
		private bool isEnableDeathPlayerAnimation = false;
		private bool isKickHeadAnimation = false;
		private BotsData objBotsData = null;
		private GameObject playerExplosion = null; // Animation of the explosion of the player.
		private bool doubleJumpStatusPlayer = false; // Double jump: false = enable , true = disable.
		private GameController objGameController = null;
		private bool modeSetPosicion = false; // Mode of the sprite position on the screen, false = pixel to world, true = world.
		private LayerMask maskLayerRaycast;
		private bool isAttackEnemy = false;
		private float maxDistanceControl = 62.5f;
		private float xDistanceControl = 62.24332f;
		private float yDistanceControl = 62.33965f;
		private float initLife = 0.0f; // Initial life.
		private float currentLife = 0.0f;  
		private bool lockAttack = false;
		private int modeAttack = 0; // Easy, medium and hard.
		private int typeBotAttack = 0; // Crouch, jump and normal
		private float searchTime = 1.0f; // Bot search time.
		private float distanceAttack = 2.0f;
		private CameraController objCameraController;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> (); 
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
			objPlayerController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<PlayerController> ();
			getOverlaps = new Collider2D[lenghtBoxCollider];
			objBotsData = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<BotsData> ();
			objGameController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<GameController> ();
			objCameraController = Camera.main.GetComponent<CameraController> ();
		}

		void OnEnable() {
			jumpPlayer = objBotsData.GetJumpBot ();
			jumpCurrentPlayer = objToolsGame.GetHeightJumpPlayer (jumpPlayer);
			speedPlayer = objBotsData.GetSpeedBot ();
			playerExplosion = objBotsData.GetPlayerExplosion ();
		}

		void Start () {
			distanceAttack = objToolsGame.GetDistancePlayer (distanceAttack);
			ResetPlayer ();
			StartCoroutine ("CoroutineSearchEnemy");
		}

		void Update () {
			if (!isEnableGame) {
				return;
			}
			switch (modeAttack) {
			case GlobalEnvironment.idEasyBotAttack:
				EasyAttackEnemy ();
				break;
			case GlobalEnvironment.idMediumBotAttack:
				MediumAttackEnemy ();
				break;
			case GlobalEnvironment.idHardBotAttack:
				HardAttackEnemy ();
				break;
			default:
				EasyAttackEnemy ();
				break;
			}
		}

		void FixedUpdate() {
			if (!isEnableGame) {
				return;
			}
			MovementOfPlayer ();
			#if UNITY_EDITOR
			TestKeywordControls ();
			#endif
		}

		void LateUpdate() {
			if (!isEnableGame) {
				return;
			}
			if (objWeaponSelect && objWeaponStand && objWeaponSpriteRenderer && objWeaponCharacters && !isEnableDeathPlayer) {
				if (objWeaponSpriteRenderer.flipX == false) {
					if (objWeaponCharacters) {
						objWeaponCharacters.SetPositionWeapon (objWeaponStand.transform.position, idleStatusAnimation, jumpStatusAnimation, crouchStatusAnimation, positionCrouch);
					}
				} else {
					float xPositionWeapon = (objWeaponStand.transform.position.x - objPlayer.transform.position.x) * -2;
					if (objWeaponCharacters) {
						Vector2 xyPositionWeapon = new Vector2 (objWeaponStand.transform.position.x + xPositionWeapon, objWeaponStand.transform.position.y);
						objWeaponCharacters.SetPositionWeapon (xyPositionWeapon, idleStatusAnimation, jumpStatusAnimation, crouchStatusAnimation, positionCrouch);
					}
				}
			}
			AnimationPlayer ();
		}

		void initPlayer () {

		}

		// Vertical and horizontal movement.
		void MovementOfPlayer () {
			if (!objPlayer || !objSpriteRenderer || !objToolsGame || !objWeaponSpriteRenderer || !objWeaponCharacters || !boxColliderPlayer) {
				return;
			}
			float speedUp = 0.0f;
			float speedDown = 0.0f;
			float xMotionPlayer = xMoveJoystickMotion;
			idleStatusAnimation = true;
			if (xMoveJoystickMotion > 0) {
				objSpriteRenderer.flipX = false;
				idleStatusAnimation = false;
				objWeaponCharacters.SetFlipPlayer (false);
			} 
			if (xMoveJoystickMotion < 0) {
				objSpriteRenderer.flipX = true;
				idleStatusAnimation = false;
				objWeaponCharacters.SetFlipPlayer (true);
			}
			xMoveJoystickMotion = 0;
			if (groundPlayer) {
				velocityPlayer.y = 0;
				if (jumpJoystickMotion) {
					velocityPlayer.y = Mathf.Sqrt(jumpCurrentPlayer * Mathf.Abs(Physics2D.gravity.y) * 2);
					jumpJoystickMotion = false;
					objToolsGame.createDust (objPlayer.transform.position);
				}
			}
			if (groundPlayer) {
				speedUp = acceleratePlayer;
				speedDown = groundSpeed;
				if (jumpStatusAnimation) {
					objToolsGame.createDust (objPlayer.transform.position);
					updownStatusAnimation = false;
				}
				jumpStatusAnimation = false;
				positionWalking = objPlayer.transform.position;
				updownStatusAnimation = false;
				jumpDifference = 0.0f;
			} else {
				speedUp = jumpSpeed;
				speedDown = 0.0f;
				jumpStatusAnimation = true;
				float yDiff = 0.0f;
				if (positionWalking.y >= 0.0f) {
					yDiff = objPlayer.transform.position.y - positionWalking.y;
				} else {
					yDiff = -1 * (positionWalking.y - objPlayer.transform.position.y);
				}
				if (yDiff < jumpDifference && !updownStatusAnimation) {
					updownStatusAnimation = true;
					if (doubleJumpStatusPlayer) {
						velocityPlayer.y = Mathf.Sqrt ((jumpCurrentPlayer / 2.0f) * Mathf.Abs (Physics2D.gravity.y) * 2);
						objToolsGame.createDust (objPlayer.transform.position);
						doubleJumpStatusPlayer = false;
					}
				}
				jumpDifference = yDiff;
			}
			if (xMotionPlayer != 0) {
				velocityPlayer.x = Mathf.MoveTowards(velocityPlayer.x, speedPlayer * xMotionPlayer, speedUp * speedDeltaPlayer);
			} else {
				velocityPlayer.x = Mathf.MoveTowards(velocityPlayer.x, 0, speedDown * speedDeltaPlayer);
			}
			groundPlayer = false;
			velocityPlayer.y += Physics2D.gravity.y * speedDeltaPlayer;
			objPlayer.transform.Translate(velocityPlayer * speedDeltaPlayer);
			int lenghtOverlaps = Physics2D.OverlapBoxNonAlloc(objPlayer.transform.position, boxColliderPlayer.size, 0, getOverlaps);
			int differencesOverlaps = lenghtOverlaps;
			if (differencesOverlaps > lenghtBoxCollider) {
				differencesOverlaps = lenghtBoxCollider;
			}
			for (int aloop = 0; aloop < differencesOverlaps; aloop++) {
				Collider2D overlap = getOverlaps [aloop];
				if (!string.Equals (overlap.gameObject.name, GlobalEnvironment.idUniqueCartridgeWeapon) && 
					!string.Equals (overlap.gameObject.name, GlobalEnvironment.idUniqueHeadTrigger) &&
					!string.Equals (overlap.gameObject.name, GlobalEnvironment.idUniqueGunChange) &&
					!string.Equals (overlap.gameObject.name, GlobalEnvironment.idUniqueMachineGunChange) &&
					!string.Equals (overlap.gameObject.name, GlobalEnvironment.idUniqueBazookaChange) &&
					!string.Equals (overlap.gameObject.tag, GlobalEnvironment.idPlayerTag) &&
					!string.Equals (overlap.gameObject.tag, GlobalEnvironment.idBotTag) &&
					!string.Equals (overlap.gameObject.tag, GlobalEnvironment.idBloodTag) &&
					!string.Equals (overlap.gameObject.tag, GlobalEnvironment.idMainCameraTag) &&
					!string.Equals (overlap.gameObject.tag, GlobalEnvironment.idWeaponTag)) {
					if (overlap == boxColliderPlayer) {
						continue;
					}
					ColliderDistance2D lenghtOverlap = overlap.Distance (boxColliderPlayer);
					if (lenghtOverlap.isOverlapped) {
						objPlayer.transform.Translate (lenghtOverlap.pointA - lenghtOverlap.pointB);
						if (Vector2.Angle (lenghtOverlap.normal, Vector2.up) < maxDegrees && velocityPlayer.y < minDegrees) {
							if (string.Equals (overlap.gameObject.tag, GlobalEnvironment.idPlatformTag)) {
								groundPlayer = true;
							}							
						}
					}
				}
			}
			if (xMotionPlayer != 0 && groundPlayer) {
				objAudioController.PlaySounds (GlobalEnvironment.idRunPlayerSound);
			}
			if (jumpStatusAnimation && !groundPlayer) {
				objAudioController.PlaySounds (GlobalEnvironment.idJumpPlayerSound);
			}
			if (xMotionPlayer != 0 && groundPlayer && crouchStatusAnimation && jumpDifference == 0.0f) {
				objAudioController.PlaySounds (GlobalEnvironment.idCrawlingPlayerSound);
			}
		}

		// Stops the player's jump.
		public void StopJumpPlayer () {
			velocityPlayer.y = 0;
		}

		void AnimationPlayer () {
			if (!objAnimPlayer) {
				return;
			}
			if (crouchStatusAnimation && jumpDifference == 0.0f) { // Kneeling Animation.
				propertiesPlayer.PositionVerticalLifeBar (false);
				if (isEnableDeathPlayer) {
					if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idFrontDeathCrouchAnimation) && !isEnableDeathPlayerAnimation) {
						objAnimPlayer.Play (GlobalEnvironment.idFrontDeathCrouchAnimation);
						isEnableDeathPlayerAnimation = true;
					}
					return;
				}
				if (isKickHeadAnimation) { // Blow to the head.
					if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idKickHeadCrouchAnimation)) {
						objAnimPlayer.Play (GlobalEnvironment.idKickHeadCrouchAnimation);
					}
					if (objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idKickHeadCrouchAnimation) &&
						objAnimPlayer.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						isKickHeadAnimation = false;
					}
					return;
				}
				if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idCrouchAnimation)) {
					objAnimPlayer.Play (GlobalEnvironment.idCrouchAnimation);
				}
			} else {
				if (isEnableDeathPlayer) {
					if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idFrontDeathAnimation) && !isEnableDeathPlayerAnimation) {
						objAnimPlayer.Play (GlobalEnvironment.idFrontDeathAnimation);
						isEnableDeathPlayerAnimation = true;
					}
					return;
				}
				if (isKickHeadAnimation) { // Blow to the head.
					if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idKickHeadAnimation)) {
						objAnimPlayer.Play (GlobalEnvironment.idKickHeadAnimation);
					}
					if (objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idKickHeadAnimation) &&
						objAnimPlayer.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
						isKickHeadAnimation = false;
					}
					return;
				}
				if (updownStatusAnimation) { // Player's fall.
					if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idGoDownAnimation)) {
						objAnimPlayer.Play (GlobalEnvironment.idGoDownAnimation);
						propertiesPlayer.PositionVerticalLifeBar (true);
					}
				} else {
					if (!jumpStatusAnimation) {
						if (idleStatusAnimation) {
							if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idIdleAnimation)) {
								objAnimPlayer.Play (GlobalEnvironment.idIdleAnimation);
							}
						} else {
							if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idRunAnimation)) {
								objAnimPlayer.Play (GlobalEnvironment.idRunAnimation);
							}
						}
					} else {
						if (!objAnimPlayer.GetCurrentAnimatorStateInfo (0).IsName (GlobalEnvironment.idJumpAnimation)) {
							objAnimPlayer.Play (GlobalEnvironment.idJumpAnimation);
						}
					}
					propertiesPlayer.PositionVerticalLifeBar (true);
				}
			}
		}
			
		// Velocity of displacement.
		public void SetSpeedPlayer (float speed) {
			speedPlayer = speed;
		}

		// Jump height.
		public void SetJumpPlayer (float jump) {
			jumpPlayer = jump;
			jumpCurrentPlayer = objToolsGame.GetHeightJumpPlayer (jump);
		}

		// Change the speed of the player.
		// float percentage = 1 - 100 full speed.
		public void SetSpeedPercentage (float percentage) {
			float newSpeedPlayer = (speedPlayer * percentage) / 100.0f;
			speedPlayer = newSpeedPlayer;
		}

		// Change the width, height, and position of the player.
		public void SetGeometryPlayer (Vector2 sizePlayer, Vector2 positionPlayer, bool modePosition) {
			widthPlayer = sizePlayer.x;
			heightPlayer = sizePlayer.y;
			xPositionPlayer = positionPlayer.x;
			yPositionPlayer = positionPlayer.y;
			modeSetPosicion = modePosition;
		}

		public void SetSearchTime (float time) {
			searchTime = time;
		}

		public void SetModeAttack (int mode) {
			modeAttack = mode;
		}

		public void SetDistanceAttack (float distance) {
			if (distance >= 0) {
				distanceAttack = objToolsGame.GetDistancePlayer (distance);
			}
		}

		// Create a bot.
		public void CreatePlayer (GlobalEnvironment.PlayerProperties botProperties) {
			GameObject playerSelect = botProperties.objPlayer;
			if (!playerSelect) {
				return;
			}
			RemovePlayer ();
			GameObject getObj = Instantiate (playerSelect, Vector2.zero, playerSelect.transform.rotation) as GameObject;
			if (getObj) {
				objToolsGame.SetParentSprite (getObj);
				objToolsGame.SetScaleSprite (getObj, widthPlayer, heightPlayer);
				if (modeSetPosicion) {
					getObj.transform.position = new Vector2 (xPositionPlayer, yPositionPlayer);
				} else {
					objToolsGame.SetPositionSprite (getObj, xPositionPlayer, yPositionPlayer);
				}
				objPlayer = getObj;
				boxColliderPlayer = objPlayer.GetComponent<BoxCollider2D>();
				circleColliderPlayer = objPlayer.GetComponent <CircleCollider2D>();
				objSpriteRenderer = objPlayer.GetComponent <SpriteRenderer>();
				objAnimPlayer = objPlayer.GetComponent <Animator>();
				propertiesPlayer = objPlayer.GetComponent <BotProperties>();
				propertiesPlayer.InitProperties ();
				propertiesPlayer.SetParentHead (gameObject);
				propertiesPlayer.SetLife (GlobalEnvironment.botCountLife);
				objWeaponStand = propertiesPlayer.GetWeaponStand ();
				float yPositionDiff = Mathf.Abs (objPlayer.transform.position.y) - Mathf.Abs (objWeaponStand.transform.position.y);
				positionCrouch = new Vector2 (0.0f, Mathf.Abs ((yPositionDiff / 2)));
				if (botProperties.speedPercentage > 0) {
					SetSpeedPercentage (botProperties.speedPercentage);
				}
				if (botProperties.searchTime > 0) {
					SetSearchTime (botProperties.searchTime);
				}
				CreateWeapon (botProperties.objWeapon);
			}
		}

		public void RemovePlayer () {
			if (objPlayer) {
				Destroy (objPlayer);
				objSpriteRenderer = null;
				objAnimPlayer = null;
				propertiesPlayer = null;
			}
			ResetPlayer ();
			RemoveWeapon ();
		}

		void ResetPlayer () {
			idleStatusAnimation = true;
			jumpStatusAnimation = false;
			StopJumpPlayer ();
			velocityPlayer.x = 0.0f;
			idleStatusAnimation = true;
			jumpStatusAnimation = false;
			crouchStatusAnimation = false;
			positionWalking = Vector2.zero;
			jumpDifference = 0.0f;
			widthCamera = objToolsGame.GetWidthCamera ();
			isEnableDeathPlayerAnimation = false;
			doubleJumpStatusPlayer = false;
			StopCoroutine ("CoroutineDestroyPlayer");
		}

		// Create the weapons.
		void CreateWeapon (GameObject objWeapon) {
			if (!objWeaponStand || objWeapon == null) {
				return;
			}
			GameObject weaponSelect = objWeapon;
			if (weaponSelect) {
				GameObject getObj = Instantiate (weaponSelect, objWeaponStand.transform.position, weaponSelect.transform.rotation) as GameObject;
				if (getObj) {
					objWeaponSelect = getObj;
					objToolsGame.SetLayerSprite (objWeaponSelect, GlobalEnvironment.idBotsLevelLayer, 3);
					objWeaponSpriteRenderer = objWeaponSelect.GetComponent <SpriteRenderer>();
					float widthSprite = objWeaponSpriteRenderer.sprite.rect.width;
					float heightSprite = objWeaponSpriteRenderer.sprite.rect.height;
					float heightNewWeapon = heightPlayer / 3;
					float widthNewWeapon = heightNewWeapon * (widthSprite / heightSprite);
					objToolsGame.SetScaleSprite (objWeaponSelect, widthNewWeapon, heightNewWeapon);
					objToolsGame.SetParentSprite (objWeaponSelect);
					objWeaponCharacters = objWeaponSelect.GetComponent <WeaponCharacters>();
					objWeaponCharacters.SetBotController (gameObject);
					objWeaponCharacters.SetTagPlayer (GlobalEnvironment.idBotTag);
					maskLayerRaycast = LayerMask.GetMask(GlobalEnvironment.idPlayerLayer);
					objWeaponCharacters.SetMaskLayerRaycast (maskLayerRaycast);
					objWeaponCharacters.EnableGame (true);
				}
			}
		}

		void RemoveWeapon () {
			if (objWeaponSelect) {
				Destroy (objWeaponSelect);
			}
			objWeaponSpriteRenderer = null;
			objWeaponCharacters = null;
		}

		void CreatePlayerExplosion () {
			if (!playerExplosion) {
				return;
			}
			int getRandom = Random.Range (0, 2);
			if (getRandom != 1) {
				return;
			}
			var getObjHead = propertiesPlayer.GetHead ();
			if (getObjHead) {
				GameObject getObj = Instantiate (playerExplosion, getObjHead.transform.position, playerExplosion.transform.rotation) as GameObject;
				if (getObj) {
					objToolsGame.SetParentSprite (getObj);
					objAudioController.PlaySounds (GlobalEnvironment.idPlayerExplosionSound);
					objCameraController.ShakeCamera ();
				}
			}
		}

		public void RecoilPlayer (float distance) {
			float directionForce = 1.0f;
			if (objSpriteRenderer.flipX) {
				directionForce = 1.0f;
			} else {
				directionForce = -1.0f;
			}	
			objPlayer.transform.Translate ( new Vector3 (distance * directionForce, 0, 0) );
		}

		// Enable the game.
		public void EnableGame (bool bGame) {
			isEnableGame = bGame;
		}

		// Enable double jumping.
		void EnableDoubleJump (bool bValue) {
			doubleJumpStatusPlayer = bValue;
		}

		// Send the events of the Joystick movement to the player.
		// float xPosition = X-axis offset, values - / +.
		// float yPosition = Y-axis offset, values - / +.
		// float maxDistance = Maximum distance of the x, y axes. 
		public void sendEventTouchMotion (float xPosition, float yPosition, float maxDistance) {
			if (!isEnableGame) {
				return;
			}
			float yDistance = maxDistance / 2.0f;
			if (maxDistance > 0) {
				xMoveJoystickMotion = xPosition / maxDistance;
				if (!jumpJoystickMotion && !jumpTmpJoystickMotion && yPosition >= yDistance) {
					if (groundPlayer) {
						jumpJoystickMotion = true;
						jumpTmpJoystickMotion = true;
						crouchStatusAnimation = false;
					} else {
						EnableDoubleJump (true);
					}
				}
			}
			if ( (yPosition < yDistance) || (xPosition == 0 && yPosition == 0 && maxDistance == 0)) {
				jumpTmpJoystickMotion = false;
			}
			if (yPosition < -yDistance && !crouchStatusAnimation) {
				crouchStatusAnimation = true;
				if (circleColliderPlayer) {
					circleColliderPlayer.enabled = false;
				}
				if (propertiesPlayer) {
					propertiesPlayer.EnableColliderHead (false);
				}
			}
			if ( (yPosition > -yDistance) || (xPosition == 0 && yPosition == 0 && maxDistance == 0)) {
				crouchStatusAnimation = false;
				if (circleColliderPlayer) {
					circleColliderPlayer.enabled = true;
				}
				if (propertiesPlayer) {
					propertiesPlayer.EnableColliderHead (true);
				}
			}
		}

		// Send the events of the Joystick Shots to the player.
		// float xPosition = X-axis offset, values - / +.
		// float yPosition = Y-axis offset, values - / +.
		// float maxDistance = Maximum distance of the x, y axes. 
		public void sendEventTouchShots (float xPosition, float yPosition, float maxDistance) {
			if (!isEnableGame) {
				return;
			}
			if (objPlayer) {
				Vector2 retPositionPlayer = objPlayerController.GetPositionPlayer ();
				float lenghtPositionPlayer = Mathf.Abs (retPositionPlayer.x - objPlayer.transform.position.x);
				if (lenghtPositionPlayer < widthCamera && objToolsGame.IsVisibleInCamera (objPlayer)) {
					if (objWeaponCharacters) {
						objWeaponCharacters.sendEventTouchShots (xPosition, yPosition, maxDistance);
					}
				}
			}
		}

		public void ChangeWeaponFromTrigger (GameObject objNewWeapon, int idWeapon, int ammunitionReload) {

		}

		// Character death.
		public void DeathPlayer () {
			if (objWeaponCharacters) {
				objWeaponCharacters.EnableGame (false);
				StopCoroutine ("CoroutineSearchEnemy");
				if (groundPlayer) {
					DestroyPlayer ();
				} else {
					StartCoroutine ("CoroutineDestroyPlayer");
				}
				objGameController.UpdateKillsBots (true);
			}
		}

		void DestroyPlayer () {
			EnableDeathAnimationPlayer (true);
			objWeaponCharacters.ShutdownWeapon ();
			if (boxColliderPlayer && circleColliderPlayer) {
				Destroy (boxColliderPlayer);
				Destroy (circleColliderPlayer);
			}
			CreatePlayerExplosion ();
			Destroy (objPlayer, 2);
		}

		IEnumerator CoroutineDestroyPlayer () {
			while (true) {
				if (groundPlayer) {
					DestroyPlayer ();
					yield break;
				}
				yield return new WaitForSeconds (0.2f);
			}
		}
			
		void EnableDeathAnimationPlayer (bool bValue) {
			isEnableDeathPlayer = bValue;
		}

		// Check if the player is active.
		// Return true = The player is active, false = The player is not active.
		public bool CheckPlayer () {
			if (objPlayer) {
				if (objPlayer.activeSelf) {
					return true;
				}
			}	
			return false;
		}

		// Tests for the bot, it works in the editor.
		void TestKeywordControls () {
			if (Input.GetKey ("space")) {
			} else if (Input.GetKey ("a")) {
				sendEventTouchMotion (-xDistanceControl, 0, maxDistanceControl);
			} else if (Input.GetKey ("d")) {
				sendEventTouchMotion (xDistanceControl, 0, maxDistanceControl);
			} else if (Input.GetKey ("s")) {
				sendEventTouchMotion (0, -xDistanceControl, maxDistanceControl);
			} else if (Input.GetKey ("w")) {
				sendEventTouchMotion (0, yDistanceControl, maxDistanceControl);
			} else {
				sendEventTouchMotion (0, 0, 0);
			}
			if (Input.GetKey ("e")) {
				sendEventTouchShots (-65, 0, maxDistanceControl);
			}
			if (Input.GetKey ("r")) {
				sendEventTouchShots (65, 0, maxDistanceControl);
			}
			if (Input.GetKey ("t")) {
				sendEventTouchShots (-30, 30, maxDistanceControl);
			}
			if (Input.GetKey ("y")) {
				sendEventTouchShots (0, 0, 0);
			}
		}

		// The player is falling?
		// True = Yes.
		// False = No.
		public bool IsPlayerFalling () {
			return updownStatusAnimation;
		}

		public void EnableKickHeadAnimation (bool bValue) {
			isKickHeadAnimation = bValue;
		}

		IEnumerator CoroutineSearchEnemy () {
			while (true) {
				if (isEnableGame) {
					switch (modeAttack) {
					case GlobalEnvironment.idEasyBotAttack:
						EasySearchEnemy ();
						break;
					case GlobalEnvironment.idMediumBotAttack:
						MediumSearchEnemy ();
						break;
					case GlobalEnvironment.idHardBotAttack:
						HardSearchEnemy ();
						break;
					default:
						EasySearchEnemy ();
						break;
					}
				}
				yield return new WaitForSeconds (searchTime);
			}
		}

		// Search the enemy.
		void EasySearchEnemy () {
			if (!objWeaponSpriteRenderer) {
				return;
			}
			initLife = propertiesPlayer.GetInitLife ();
			currentLife = propertiesPlayer.GetLife ();
			float xDirection = -1;
			if (objWeaponSpriteRenderer.flipX == false) {
				xDirection = 1;
			} 
			bool isPlayerNear = false;
			if (IsRaycastEnemy (xDirection, 1) > 0) { // top - right
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.5f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.25f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.125f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0) > 0) { // right
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.125f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.25f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.5f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -1) > 0) { // down - right
				isPlayerNear = true;
			} 
			if (isPlayerNear) { // Shoot the enemy.
				isAttackEnemy = true;
			} else {
				isAttackEnemy = false;
				if (objWeaponSpriteRenderer.flipX == false) {
					sendEventTouchMotion (-xDistanceControl, 0, maxDistanceControl);
				} else {
					sendEventTouchMotion (xDistanceControl, 0, maxDistanceControl);
				}
			}
			currentLife = propertiesPlayer.GetLife ();
			if (currentLife <= (initLife / 1.0f) && !lockAttack) {
				lockAttack = true;
			}
		}

		// Attack the enemy.
		void EasyAttackEnemy () {
			if (!objWeaponSpriteRenderer) {
				return;
			}
			float xDirection = -1;
			if (!isAttackEnemy) {
				return;
			}
			if (objWeaponSpriteRenderer.flipX == false) {
				xDirection = 1;
			}
			float distancePlayer = IsRaycastEnemy (xDirection, 0);
			if (distancePlayer > 0) { // Shoot the enemy.
				float yDistanceControlTmp = 0.0f;
				float xDistanceControlTmp = xDistanceControl;
				yDistanceControlTmp = 0.0f;
				if (objWeaponSpriteRenderer.flipX == false) {
					if (distancePlayer > distanceAttack) {
						sendEventTouchMotion (xDistanceControlTmp, yDistanceControlTmp, maxDistanceControl);
					}
					sendEventTouchShots (xDistanceControl, 0, maxDistanceControl);
				} else {
					if (distancePlayer > distanceAttack) {
						sendEventTouchMotion (-xDistanceControlTmp, yDistanceControlTmp, maxDistanceControl);
					}
					sendEventTouchShots (-xDistanceControl, 0, maxDistanceControl);
				}
			}
		}

		// Search the enemy.
		void MediumSearchEnemy () {
			if (!objWeaponSpriteRenderer) {
				return;
			}
			initLife = propertiesPlayer.GetInitLife ();
			currentLife = propertiesPlayer.GetLife ();
			float xDirection = -1;
			if (objWeaponSpriteRenderer.flipX == false) {
				xDirection = 1;
			} 
			bool isPlayerNear = false;
			if (IsRaycastEnemy (xDirection, 1) > 0) { // top - right
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.5f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.25f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.125f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0) > 0) { // right
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.125f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.25f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.5f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -1) > 0) { // down - right
				isPlayerNear = true;
			}
			if (isPlayerNear) { // Shoot the enemy.
				isAttackEnemy = true;
			} else {
				isAttackEnemy = false;
				if (objWeaponSpriteRenderer.flipX == false) {
					sendEventTouchMotion (-xDistanceControl, 0, maxDistanceControl);
				} else {
					sendEventTouchMotion (xDistanceControl, 0, maxDistanceControl);
				}
			}
			currentLife = propertiesPlayer.GetLife ();
			if (currentLife <= (initLife / 1.0f) && !lockAttack) {
				lockAttack = true;
				int typeAttack = Random.Range (0,3);
				switch (typeAttack) {
				case GlobalEnvironment.idNormalBotAttack:
					typeBotAttack = GlobalEnvironment.idNormalBotAttack;
					break;
				case GlobalEnvironment.idCrouchBotAttack:
					typeBotAttack = GlobalEnvironment.idCrouchBotAttack;
					break;
				case GlobalEnvironment.idJumpBotAttack:
					typeBotAttack = GlobalEnvironment.idJumpBotAttack;
					break;
				default:
					typeBotAttack = GlobalEnvironment.idNormalBotAttack;
					break;
				}
			}
		}

		// Attack the enemy.
		void MediumAttackEnemy () {
			if (!objWeaponSpriteRenderer) {
				return;
			}
			float xDirection = -1;
			if (!isAttackEnemy) {
				return;
			}
			if (objWeaponSpriteRenderer.flipX == false) {
				xDirection = 1;
			}
			float distancePlayer = IsRaycastEnemy (xDirection, 0);
			if (distancePlayer > 0) { // Shoot the enemy.

				float yDistanceControlTmp = 0.0f;
				float xDistanceControlTmp = xDistanceControl;

				switch (typeBotAttack) {
				case GlobalEnvironment.idNormalBotAttack:
					yDistanceControlTmp = 0.0f;
					break;
				case GlobalEnvironment.idCrouchBotAttack:
					yDistanceControlTmp = -yDistanceControl;
					xDistanceControlTmp = xDistanceControl / 2;
					break;
				case GlobalEnvironment.idJumpBotAttack:
					yDistanceControlTmp = yDistanceControl;
					break;
				default:
					yDistanceControlTmp = 0.0f;
					break;
				}
				if (objWeaponSpriteRenderer.flipX == false) {
					if (distancePlayer > distanceAttack) {
						sendEventTouchMotion (xDistanceControlTmp, yDistanceControlTmp, maxDistanceControl);
					}
					sendEventTouchShots (xDistanceControl, 0, maxDistanceControl);
				} else {
					if (distancePlayer > distanceAttack) {
						sendEventTouchMotion (-xDistanceControlTmp, yDistanceControlTmp, maxDistanceControl);
					}
					sendEventTouchShots (-xDistanceControl, 0, maxDistanceControl);
				}
			}
		}

		// Search the enemy.
		void HardSearchEnemy () {
			if (!objWeaponSpriteRenderer) {
				return;
			}
			bool isPlayerNear = false;
			float xDirection = -1;
			float xCorrectionDirection = -0.02f;
			if (objWeaponSpriteRenderer.flipX == false) {
				xDirection = 1;
				xCorrectionDirection = 0.02f;
			} 
			if (IsRaycastEnemy (xCorrectionDirection, 1) > 0) { // top
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 1) > 0) { // top - right
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.5f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.25f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0.125f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, 0) > 0) { // right
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.125f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.25f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xDirection, -0.5f) > 0) { 
				isPlayerNear = true;
			} else if (IsRaycastEnemy (xCorrectionDirection, -1) > 0) { // down - right
				isPlayerNear = true;
			}
			if (isPlayerNear) { 
				isAttackEnemy = true;
			} else {
				isAttackEnemy = false;
				if (objWeaponSpriteRenderer.flipX == false) {
					sendEventTouchMotion (-xDistanceControl, 0, maxDistanceControl);
				} else {
					sendEventTouchMotion (xDistanceControl, 0, maxDistanceControl);
				}
			}
		}

		// Attack the enemy.
		void HardAttackEnemy () {
			if (!objWeaponSpriteRenderer) {
				return;
			}
			bool isPlayerNear = false;
			float xDirection = -1;
			float xCorrectionDirection = -0.02f;
			float xDistanceShoot = 0.0f;
			float yDistanceShoot = 0.0f;
			float yDistanceControlTmp = 0.0f;
			float xDistanceControlTmp = 0.0f;
			float distancePlayer = 0.0f;
			if (!isAttackEnemy) {
				return;
			}
			if (objWeaponSpriteRenderer.flipX == false) {
				xDirection = 1;
				xCorrectionDirection = 0.02f;
			}
			initLife = propertiesPlayer.GetInitLife ();
			currentLife = propertiesPlayer.GetLife ();
			distancePlayer = IsRaycastEnemy (xCorrectionDirection, 1); // top
			if (distancePlayer > 0) {
				isPlayerNear = true;
				distancePlayer = distanceAttack;
				xDistanceControlTmp = xDistanceControl * xDirection;
				yDistanceControlTmp = 0.0f;
			} else {
				distancePlayer = IsRaycastEnemy (xDirection, 1); // top - right
				if (distancePlayer > 0) {
					isPlayerNear = true;
					xDistanceControlTmp = (xDistanceControl / 2) * xDirection;
					yDistanceControlTmp = yDistanceControl;
					xDistanceShoot = xDistanceControl * xDirection;
					yDistanceShoot = yDistanceControl;
				} else {
					distancePlayer = IsRaycastEnemy (xDirection, 0); // right
					if (distancePlayer > 0) {
						isPlayerNear = true;
						xDistanceControlTmp = xDistanceControl * xDirection;
						if (currentLife >= 0.4f && currentLife <= 0.6f) {
							yDistanceControlTmp = -yDistanceControl;
						} else {
							yDistanceControlTmp = 0.0f;
						}
						xDistanceShoot = xDistanceControl * xDirection;
						yDistanceShoot = 0.0f;
					} else {
						distancePlayer = IsRaycastEnemy (xDirection, -1); // down - right
						if (distancePlayer > 0) {
							isPlayerNear = true;
							xDistanceShoot = xDistanceControl * xDirection;
							yDistanceShoot = -yDistanceControl;
						} else {
							distancePlayer = IsRaycastEnemy (xCorrectionDirection, -1); // down
							if (distancePlayer > 0) {
								isPlayerNear = true;
							} else {
								distancePlayer = 0.0f;
								isPlayerNear = false;
							}
						}
					}
				}
			}
			if (isPlayerNear) { // Shoot the enemy.
				if (distancePlayer >= distanceAttack) {
					sendEventTouchMotion (xDistanceControlTmp, yDistanceControlTmp, maxDistanceControl);					
				}
				sendEventTouchShots (xDistanceShoot, yDistanceShoot, maxDistanceControl);
			} 
		}

		// Detect the enemy with Raycast.
		// Returns the distance that separates them.
		float IsRaycastEnemy (float xPosition, float yPosition) {
			if (!objWeaponStand) {
				return 0.0f;
			}
			RaycastHit2D hitRaycast = Physics2D.Raycast(objWeaponStand.transform.position, new Vector2 (xPosition, yPosition), Mathf.Infinity, maskLayerRaycast);
			#if UNITY_EDITOR
			//Debug.DrawLine(objWeaponStand.transform.position, new Vector2 (objWeaponStand.transform.position.x + (20 * xPosition), objWeaponStand.transform.position.y + (20 * yPosition)), Color.red, 0.5f, false);
			#endif
			if (hitRaycast.collider != null) {
				return Mathf.Abs(hitRaycast.point.x - objWeaponStand.transform.position.x);
			}
			return 0.0f;
		}

	}
}
