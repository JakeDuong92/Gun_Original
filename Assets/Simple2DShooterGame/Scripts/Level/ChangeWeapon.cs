// Control the exchange of weapons.

using System.Collections;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class ChangeWeapon : MonoBehaviour 
	{
		public int modelWeapon = 0; // Weapon Models: 0 = Gun, 1 = machineGun, 2 = Bazooka
		public float widthSprite = 100.0f;
		public float heightSprite = 100.0f;
		private Vector2 positionSprite;
		private ToolsGame objToolsGame;
		private float countUpDownWeapon = 0.0f;
		private PlayerController objPlayerController = null;
		private AudioController objAudioController = null;
		public int ammunitionReload = 16;
		private bool isEnableTrigger = true;
		private int currentModelWeapon = 0;
		private int weaponSound = 0;
		private int positionArray = -1;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
			objPlayerController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<PlayerController> ();
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
		}

		void OnEnable () {
			setModelWeapon (modelWeapon);
			objToolsGame.SetScaleSprite (gameObject , widthSprite, heightSprite);
			objToolsGame.SetParentSprite (gameObject);
			positionSprite = gameObject.transform.position;
		}

		// Use this for initialization
		void Start () {
			StartCoroutine ("CoroutineChangeWeapon");
		}

		// Update is called once per frame
		void Update () {
			gameObject.transform.position = new Vector2 (positionSprite.x, positionSprite.y + countUpDownWeapon);
		}

		IEnumerator CoroutineChangeWeapon () {
			float countTmp = 1.0f;
			float maxCount = (0.1f * Screen.height) / GlobalEnvironment.resolutionHeight;
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

		void OnTriggerEnter2D(Collider2D otherobjs) {
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idPlayerTag) && isEnableTrigger) {
				if (objPlayerController) {
					objPlayerController.ChangeWeaponFromTrigger (gameObject, currentModelWeapon, ammunitionReload);
				}
			}
		}

		public void DestroyWeapon () {
			objAudioController.PlaySounds (weaponSound);
			Destroy (gameObject);
		}

		public void StopTrigger () {
			isEnableTrigger = false;
		}

		void setModelWeapon (int idWeapon) {
			switch (idWeapon) {
			case 0:
				currentModelWeapon = GlobalEnvironment.idGunWeapon;
				gameObject.name = GlobalEnvironment.idUniqueGunChange;
				weaponSound = GlobalEnvironment.idGunChangeSound;
				break;
			case 1:
				currentModelWeapon = GlobalEnvironment.idMachineGunWeapon;
				gameObject.name = GlobalEnvironment.idUniqueMachineGunChange;
				weaponSound = GlobalEnvironment.idMachineGunChangeSound;
				break;
			case 2:
				currentModelWeapon = GlobalEnvironment.idBazookaWeapon;
				gameObject.name = GlobalEnvironment.idUniqueBazookaChange;
				weaponSound = GlobalEnvironment.idBazookaChangeSound;
				break;
			default:
				currentModelWeapon = GlobalEnvironment.idGunWeapon;
				gameObject.name = GlobalEnvironment.idUniqueGunChange;
				weaponSound = GlobalEnvironment.idGunChangeSound;
				break;
			}
		}

		public void SetPositionArray (int position) {
			positionArray = position;
		}

		public int GetPositionArray () {
			return positionArray;
		}

		public int GetModelWeapon () {
			return currentModelWeapon;
		}

	}
}
