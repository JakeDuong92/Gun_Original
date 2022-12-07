// Play the sounds.

using UnityEngine.UI;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class AudioController : MonoBehaviour 
	{
		private AudioSource audioGame = null;
		public float volumeSound = 0.5f;
		private Button soundButton = null;
		private Image imageButton = null;
		public Sprite [] spriteSounds;
		public AudioClip explosionSound;
		public AudioClip gunSound;
		public AudioClip machineGunSound;
		public AudioClip bazookaSound;
		public AudioClip finalTimeSound;
		public AudioClip giftCoinsSound;

		void Awake () {
			var getUI = GameObject.FindGameObjectWithTag (GlobalEnvironment.idGameUITag);
			if (getUI) {
				foreach (Transform objChild in getUI.transform) {
					if (string.Equals(objChild.name, GlobalEnvironment.idUISettingMenu)) {
						foreach (Transform objContainerChild in objChild.transform) {
							if (string.Equals (objContainerChild.name, GlobalEnvironment.idContainerSettingMenu)) {
								foreach (Transform objButtonChild in objContainerChild.transform) {
									if (string.Equals (objButtonChild.name, GlobalEnvironment.idSoundButtonSettingMenu)) {
										soundButton = objButtonChild.gameObject.GetComponent <Button> ();
										imageButton = soundButton.GetComponent <Image> ();
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		// Use this for initialization
		void Start () {
			audioGame = GetComponent <AudioSource>();
			if (audioGame) {
				audioGame.volume = volumeSound;
			}
			int getEnableSound = PlayerPrefs.GetInt (GlobalEnvironment.idEnableSound, 1);
			ChangeSprite (getEnableSound);
		}

		// Receive the events of the sound button.
		public void SoundButton () {
			int getEnableSound = PlayerPrefs.GetInt (GlobalEnvironment.idEnableSound, 1);
			if (getEnableSound == 1) {
				getEnableSound = 0;
			} else {
				getEnableSound = 1;
			}
			PlayerPrefs.SetInt (GlobalEnvironment.idEnableSound, getEnableSound);
			ChangeSprite (getEnableSound);
		}

		// Change the Sprites of the sound button.
		private void ChangeSprite (int idSprite) {
			if (!soundButton || !imageButton || spriteSounds.Length != 2) {
				return;
			}
			switch (idSprite) {
			case 0:
				imageButton.sprite = spriteSounds [1];
				break;
			case 1:
				imageButton.sprite = spriteSounds [0];
				break;
			default:
				break;
			}
		}

		// Playing sounds.
		public void PlaySounds (int typeSound) {
			if ( (PlayerPrefs.GetInt (GlobalEnvironment.idEnableSound, 1) == 0) || !audioGame) {
				return;
			}
			switch (typeSound) {
				case GlobalEnvironment.idPlayerExplosionSound:
					if (explosionSound) {
						audioGame.PlayOneShot (explosionSound);
					}				
				break;
				case GlobalEnvironment.idGunShootSound:
					if (gunSound) {
						audioGame.PlayOneShot (gunSound);
					}				
				break;
				case GlobalEnvironment.idMachineGunShootSound:
					if (machineGunSound) {
						audioGame.PlayOneShot (machineGunSound);
					}				
				break;				
				case GlobalEnvironment.idBazookaShootSound:
					if (bazookaSound) {
						audioGame.PlayOneShot (bazookaSound);
					}				
				break;
				case GlobalEnvironment.idFinalTimeSound:
					if (finalTimeSound) {
						audioGame.PlayOneShot (finalTimeSound);
					}				
				break;
				case GlobalEnvironment.idGiftCoinsSound:
					if (giftCoinsSound) {
						audioGame.PlayOneShot (giftCoinsSound);
					}				
				break;													
				default:
				break;
			}
		}
	}
}
