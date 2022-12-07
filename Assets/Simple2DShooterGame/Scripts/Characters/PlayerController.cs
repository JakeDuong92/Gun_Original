// Player controller.

using System.Collections;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class PlayerController : MonoBehaviour 
	{
		private bool isEnableGame = false;
		public GameObject[] gameCharacters; // Characters available in the store.
		public GameObject[] weaponsCharacters; // Weapons of the characters.
		private ToolsGame objToolsGame;
		public float widthPlayer = 140.0f;
		public float heightPlayer = 140.0f;
		public float xPositionPlayer = -100.0f;
		public float yPositionPlayer = -220.0f;
		private float xMoveJoystickMotion = 0.0f;
		private bool jumpJoystickMotion = false;
		private bool jumpTmpJoystickMotion = false;
		private BoxCollider2D boxColliderPlayer;
		private CircleCollider2D circleColliderPlayer;
		private Vector2 velocityPlayer;
		private bool groundPlayer = false;
		public float speedPlayer = 8.0f;
		private float maxDegrees = 90.0f;
		private float minDegrees = 0.0f;
		private float jumpSpeed = 30.0f;
		public float jumpPlayer = 3.0f;
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
		private CharacterProperties propertiesPlayer = null;
		private GameController objGameController = null;
		private bool isEnableDeathPlayer = false;
		private bool isEnableDeathPlayerAnimation = false;
		private bool isKickHeadAnimation = false;
		private bool doubleJumpStatusPlayer = false; // Double jump: false = no , true = yes. 
		private bool isLockJoystick = false; // Lock the joysticks.
		private int countNamePlayer = 0;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> (); 
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
			objGameController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<GameController> ();
			getOverlaps = new Collider2D[lenghtBoxCollider];
		}

		void OnEnable() {
			jumpCurrentPlayer = objToolsGame.GetHeightJumpPlayer (jumpPlayer);
		}

		void Start () {
			ResetPlayer ();
		}

		void Update () {
			if (!isEnableGame) {
				return;
			}
			#if UNITY_EDITOR
				TestKeywordControls ();
			#endif
		}

		void FixedUpdate () {
			if (!isEnableGame) {
				return;
			}
			MovementOfPlayer ();
		}

		void LateUpdate () {
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
						velocityPlayer.y = Mathf.Sqrt ((jumpCurrentPlayer / 2) * Mathf.Abs (Physics2D.gravity.y) * 2);
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
			if (crouchStatusAnimation && jumpDifference == 0.0f) { // Kneeling player.
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
			
		public void CreatePlayer (GlobalEnvironment.PlayerProperties playerProperties) {
			if (gameCharacters.Length == 0) {
				return;
			}
			GameObject playerSelect = gameCharacters [playerProperties.idPlayer];
			if (!playerSelect) {
				return;
			}
			GameObject getObj = Instantiate (playerSelect, Vector2.zero, playerSelect.transform.rotation) as GameObject;
			if (getObj) {
				string namePlayer = getObj.name + "-" + countNamePlayer.ToString ();
				getObj.name = namePlayer;
				countNamePlayer++;
				objToolsGame.SetParentSprite (getObj);
				objToolsGame.SetScaleSprite (getObj, widthPlayer, heightPlayer);
				getObj.transform.position = playerProperties.positionPlayer;
				objPlayer = getObj;
				boxColliderPlayer = objPlayer.GetComponent <BoxCollider2D>();
				circleColliderPlayer = objPlayer.GetComponent <CircleCollider2D>();
				objSpriteRenderer = objPlayer.GetComponent <SpriteRenderer>();
				objAnimPlayer = objPlayer.GetComponent <Animator>();
				propertiesPlayer = objPlayer.GetComponent <CharacterProperties>();
				propertiesPlayer.InitProperties ();
				propertiesPlayer.SetParentHead (gameObject);
				propertiesPlayer.SetLife (GlobalEnvironment.playerCountLife);
				objWeaponStand = propertiesPlayer.GetWeaponStand ();
				float yPositionDiff = Mathf.Abs (objPlayer.transform.position.y) - Mathf.Abs (objWeaponStand.transform.position.y);
				positionCrouch = new Vector2 (0.0f, Mathf.Abs ((yPositionDiff / 2)));
				CreateWeapon (playerProperties.idWeapon);
			}
		}

		public void RemovePlayer () {
			EnableGame (false);
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
			EnableDeathAnimationPlayer (false);
			groundPlayer = false;
			jumpJoystickMotion = false;
			jumpTmpJoystickMotion = false;
			isEnableDeathPlayerAnimation = false;
			doubleJumpStatusPlayer = false;
			isLockJoystick = false;
			StopCoroutine ("CoroutineDestroyPlayer");
		}

		// Shows the player in scene.
		public void ShowPlayer (bool show) {
			if (objPlayer) {
				objPlayer.SetActive (show);
			}
		}

		// Create the weapons.
		void CreateWeapon (int idWeapon) {
			if (!objWeaponStand || weaponsCharacters.Length == 0) {
				return;
			}
			GameObject weaponSelect = null;
			switch (idWeapon) {
			case GlobalEnvironment.idGunWeapon:
				weaponSelect = weaponsCharacters [0];
				break;
			case GlobalEnvironment.idMachineGunWeapon:
				weaponSelect = weaponsCharacters [1];
				break;
			case GlobalEnvironment.idBazookaWeapon:
				weaponSelect = weaponsCharacters [2];
				break;
			default:
				weaponSelect = null;
				break;
			}
			if (weaponSelect) {
				GameObject getObj = Instantiate (weaponSelect, objWeaponStand.transform.position, weaponSelect.transform.rotation) as GameObject;
				if (getObj) {
					objWeaponSelect = getObj;
					objWeaponSpriteRenderer = objWeaponSelect.GetComponent <SpriteRenderer>();
					float widthSprite = objWeaponSpriteRenderer.sprite.rect.width;
					float heightSprite = objWeaponSpriteRenderer.sprite.rect.height;
					float heightNewWeapon = heightPlayer / 3;
					float widthNewWeapon = heightNewWeapon * (widthSprite / heightSprite);
					objToolsGame.SetScaleSprite (objWeaponSelect, widthNewWeapon, heightNewWeapon);
					objToolsGame.SetParentSprite (objWeaponSelect);
					objWeaponCharacters = objWeaponSelect.GetComponent <WeaponCharacters>();
					objWeaponCharacters.SetTagPlayer (GlobalEnvironment.idPlayerTag);
					objWeaponCharacters.SetModelWeapon (idWeapon);
					objWeaponCharacters.SetMaskLayerRaycast (LayerMask.GetMask(GlobalEnvironment.idBotsLayer));
					objWeaponCharacters.SetFlipPlayer (objSpriteRenderer.flipX);
					objWeaponSpriteRenderer.flipX = objSpriteRenderer.flipX;
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
			if (!isEnableGame || isLockJoystick) {
				return;
			}
			if (float.IsNaN (xPosition) || float.IsNaN (yPosition) || float.IsNaN (maxDistance)) {
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
			if ( (yPosition < yDistance) || (xPosition == 0 && yPosition == 0 && maxDistance == 0) ) {
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
			if ( (yPosition > -yDistance) || (xPosition == 0 && yPosition == 0 && maxDistance == 0) ) {
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
			if (!isEnableGame || isLockJoystick) {
				return;
			}
			if (float.IsNaN (xPosition) || float.IsNaN (yPosition) || float.IsNaN (maxDistance)) {
				return;
			}	
			if (objWeaponCharacters) {
				objWeaponCharacters.sendEventTouchShots (xPosition, yPosition, maxDistance);
			}
		}
			
		// Weapon change.
		public void ChangeWeaponFromTrigger (GameObject objNewWeapon, int idWeapon, int ammunitionReload) {
			if (!isEnableGame || isLockJoystick) {
				return;
			}			
			if (objWeaponCharacters) {
				int currentModelWeapon = objWeaponCharacters.getModelWeapon ();
				var objWeapon = objNewWeapon.GetComponent <ChangeWeapon> ();
				if (currentModelWeapon == idWeapon) {
					if (objWeaponCharacters.RechargeAmmunition (ammunitionReload)) {
						if (objWeapon) {
							objWeapon.StopTrigger ();
							objWeapon.DestroyWeapon ();
						}
					}
				} else {
					RemoveWeapon ();
					CreateWeapon (idWeapon);
					if (objWeapon) {
						objWeapon.StopTrigger ();
						objWeapon.DestroyWeapon ();
					}
					if (objGameController) {
						objGameController.ChangeWeapon (idWeapon);
					}					
				}
			}
		}

		public Vector2 GetPositionPlayer () {
			if (objPlayer) {
				return objPlayer.transform.position;
			}
			return Vector2.zero;
		}

		public void SetPositionPlayer (Vector2 position) {
			if (objPlayer) {
				objPlayer.transform.position = position;
			}
		}

		// Returns the number of lives.
		public float GetCurrentLife () {
			if (propertiesPlayer) {
				return propertiesPlayer.GetLife ();
			}
			return 0.0f;
		}

		public float GetInitLife () {
			if (propertiesPlayer) {
				return propertiesPlayer.GetInitLife ();
			}
			return 0.0f;
		}

		// Returns the weapon model.
		public int GetModelWeapon () {
			if (objWeaponCharacters) {
				return objWeaponCharacters.getModelWeapon ();
			}
			return GlobalEnvironment.idGunWeapon;
		}

		// Character death.
		public void DeathPlayer () {
			isLockJoystick = true;
			if (objWeaponCharacters) {
				objWeaponCharacters.EnableGame (false);
				if (groundPlayer) {
					DestroyPlayer ();
				} else {
					StartCoroutine ("CoroutineDestroyPlayer");
				}
			}
		}

		void DestroyPlayer () {
			EnableDeathAnimationPlayer (true);
			objWeaponCharacters.ShutdownWeapon ();
			if (boxColliderPlayer && circleColliderPlayer) {
				Destroy (boxColliderPlayer);
				Destroy (circleColliderPlayer);
			}
			if (objGameController) {
				objGameController.DeathPlayer ();
			}
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

		// Tests for the bot, it works in the editor.
		void TestKeywordControls () {
			if (Input.GetKey ("u")) {
				sendEventTouchShots (-65, 0, 65);
			}
			if (Input.GetKey ("i")) {
				sendEventTouchShots (65, 0, 65);
			}
			if (Input.GetKey ("o")) {
				sendEventTouchShots (-30, 30, 65);
			}
			if (Input.GetKey ("p")) {
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

		// Returns the initial ammo.
		public int GetInitAmmunition () {
			if (objWeaponCharacters) {
				return objWeaponCharacters.GetInitAmmunition ();
			}
			return 0;
		}

		// Returns the current ammo.
		public int GetCurrentAmmunition () {
			if (objWeaponCharacters) {
				return objWeaponCharacters.GetCurrentAmmunition ();
			}
			return 0;
		}

	}
}
