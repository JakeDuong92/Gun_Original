// Character properties.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class CharacterProperties : MonoBehaviour 
	{
		public GameObject weaponStand;
		public GameObject objHead;
		private string idObj = null;
		[Header("Life Bar")]
		public GameObject borderLifeBar;
		public GameObject lifeBar;
		public float alphaColorBar = 0.5f;
		public float widthBorderLifeBar = 170.0f;
		public float heightBorderLifeBar = 34.0f;
		private SpriteRenderer objBorderRenderer = null;
		private Color colorBorderLifeBar;
		private SpriteRenderer objLifeRenderer = null;
		private Color colorLifeBar;
		private float yPositionBorder = 0.0f;
		private ToolsGame objToolsGame;
		private float countLife = 1.0f; // Countdown of lives.
		private float initLife = 1.0f; // Initial life.
		private float stepLife = 1.0f;
		private GameObject objParentPlayer = null;
		private AudioController objAudioController;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
			float getRdm = Random.Range (0, 10000);
			idObj = System.DateTime.Now.TimeOfDay.ToString () + "-" + getRdm.ToString ();
		}

		// Use this for initialization
		void Start () {
			if (borderLifeBar) {
				objBorderRenderer = borderLifeBar.GetComponent <SpriteRenderer> ();
				colorBorderLifeBar = objBorderRenderer.color;
				SetAlphaSprite (objBorderRenderer, colorBorderLifeBar, alphaColorBar);
			}
			if (lifeBar) {
				objLifeRenderer = lifeBar.GetComponent <SpriteRenderer> ();
				colorLifeBar = objLifeRenderer.color;
				SetAlphaSprite (objLifeRenderer, colorLifeBar, alphaColorBar);
			}
		}

		public void InitProperties () {
			SetScaleLifeBar ();
			if (borderLifeBar) {
				yPositionBorder = Mathf.Abs (borderLifeBar.transform.position.y - transform.position.y); 
			}
		}

		public GameObject GetWeaponStand () {
			return weaponStand;
		}

		// Returns the id of the object.
		public string GetIdObj () {
			return idObj;
		}

		public void SetParentHead (GameObject objValue) {
			objParentPlayer = objValue;
		}

		public GameObject GetParentHead () {
			return objParentPlayer;
		}

		public void EnableColliderHead (bool bValue) {
			if (objHead) {
				objHead.SetActive (bValue);
			}
		}

		// Change the alpha color of the sprite.
		void SetAlphaSprite (SpriteRenderer objRenderer, Color objColor, float alphaColor) {
			if (objRenderer) {
				objColor.a = alphaColor;
				objRenderer.color = objColor;
			}
		}

		// Change the position of the lifebar.
		public void PositionVerticalLifeBar (bool topDown) {
			if (borderLifeBar && lifeBar) {
				if (topDown) { // top
					borderLifeBar.transform.position = new Vector2 (transform.position.x, transform.position.y + yPositionBorder);
				} else { // down
					borderLifeBar.transform.position = new Vector2 (transform.position.x, transform.position.y + (yPositionBorder / 1.2f));
				}	
			}
		}

		void SetScaleLifeBar () {
			if (!borderLifeBar && !lifeBar) {
				return;
			}	
			objToolsGame.SetScaleSprite (borderLifeBar, widthBorderLifeBar, heightBorderLifeBar);
		}

		// Life bar level.
		// float life: Values between 0.0f to 10.0f. 
		public void SetLife (float life) {
			if (!lifeBar) {
				return;
			}
			initLife = life;
			stepLife = 1.0f / life;
			if (stepLife > 1.0f || stepLife < 0.0f) {
				stepLife = 1.0f;
			}
			countLife = 1.0f;
			initLife = countLife;
			lifeBar.transform.localScale = new Vector3 (countLife, 1.0f, 1.0f);
		}

		public void DecreaseLife (float impactShot) {
			if (!lifeBar) {
				return;
			}
			countLife -= (stepLife * impactShot);
			if (countLife < 0.0f) {
				countLife = 0.0f;
			}
			lifeBar.transform.localScale = new Vector3 (countLife, 1.0f, 1.0f);
			if (countLife == 0.0f) {

			}
		}

		public float GetLife () {
			return countLife;
		}

		public float GetInitLife () {
			return initLife;
		}

		// Receive the impact of the bullets.
		public void ReceiveImpactPlayer (Transform transformImpact, float numImpact) {
			if (countLife == 0.0f) {
				return;
			}
			DecreaseLife (numImpact);
			objToolsGame.CreateBloodCharacters (transformImpact);
			if (countLife == 0.0f) {
				if (borderLifeBar && lifeBar) {
					borderLifeBar.SetActive (false);
					lifeBar.SetActive (false);
				}
				if (objParentPlayer) {
					var objController = objParentPlayer.GetComponent<PlayerController> ();
					if (objHead) {
						var getColliderPlayer = objHead.GetComponent<BoxCollider2D> ();
						if (getColliderPlayer) {
							Destroy (getColliderPlayer);
						}
					}
					objAudioController.PlaySounds (GlobalEnvironment.idDeathPlayerSound);
					objController.DeathPlayer ();
				}
			}
		}

		// Detects the impact of bullets.
		void OnTriggerEnter2D(Collider2D otherobjs) {
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idBulletTag)) {
				var bulletWeapon = otherobjs.gameObject.GetComponent<BulletWeapon> ();
				string retTag = bulletWeapon.GetTagPlayer ();
				if (!string.Equals (retTag, GlobalEnvironment.idPlayerTag)) {
					int modelWeapon = bulletWeapon.GetModelWeapon ();
					float impactLevel = 0;
					Transform getObjTransform = otherobjs.gameObject.transform;
					switch (modelWeapon) {
					case GlobalEnvironment.idGunWeapon:
						impactLevel = 1;
						Destroy (otherobjs.gameObject);
						break;
					case GlobalEnvironment.idMachineGunWeapon:
						impactLevel = 2;
						break;
					case GlobalEnvironment.idBazookaWeapon:
						impactLevel = 5;
						break;
					default:
						impactLevel = 1;
						break;
					}
					ReceiveImpactPlayer (getObjTransform, impactLevel);
				}
			}
		}

	}
}
