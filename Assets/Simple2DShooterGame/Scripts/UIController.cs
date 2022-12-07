// Controller the graphical interface.

using System.Collections;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{	
	public class UIController : MonoBehaviour 
	{
		[Header("Set Menus")]
		public GameObject mainMenu;
		public GameObject settingMenu;
		public GameObject shopMenu;
		public GameObject quitMenu;
		public GameObject levelMenu;
		public GameObject pauseMenu;
		public GameObject gameOverMenu;
		public GameObject gdprMenu;
		public GameObject loadingWindow;
		private UISetListener objSetListener;
		private UIInputGame objUIInputGame;
		private UIShopController objUIShopController;
		private UIScoreController objUIScoreController;
		private GameController objGameController;
		private AudioController objAudioController;
		[Header("Link for Rate Us")]
		public string linkRateUs;
		[Header("Link for Terms of Use")]
		public string linkTermsOfUse;
		GoogleAdsGame googleAdsGame; 

		void Awake () {
			objSetListener = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UISetListener> ();
			objUIInputGame = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIInputGame> ();
			objUIShopController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIShopController> ();
			objUIScoreController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIScoreController> ();
			objGameController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<GameController> ();
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
			googleAdsGame = GameObject.Find(GlobalEnvironment.idAdsController).GetComponent<GoogleAdsGame> ();
		}

		// Use this for initialization
		void Start () {
			if (mainMenu && settingMenu && settingMenu && shopMenu && quitMenu && levelMenu && pauseMenu && gameOverMenu && gdprMenu) {
				if (objSetListener && objUIInputGame) {
					objSetListener.SetupButtons (GlobalEnvironment.idMainMenu, mainMenu);
					objSetListener.SetupButtons (GlobalEnvironment.idSettingMenu, settingMenu);
					objSetListener.SetupButtons (GlobalEnvironment.idShopMenu, shopMenu);
					objSetListener.SetupButtons (GlobalEnvironment.idQuitMenu, quitMenu);
					objSetListener.SetupButtons (GlobalEnvironment.idLevelMenu, levelMenu);
					objSetListener.SetupButtons (GlobalEnvironment.idPauseMenu, pauseMenu);
					objSetListener.SetupButtons (GlobalEnvironment.idGameOverMenu, gameOverMenu);
					objUIInputGame.SetJoysticks (levelMenu);
					objUIShopController.initShop (shopMenu);
					objUIScoreController.SetScoreMainMenu (mainMenu);
					objUIScoreController.SetScoreShopMenu (shopMenu);
					objUIScoreController.SetScoreLevelMenu (levelMenu);
					objUIScoreController.SetScoreGameOverMenu (gameOverMenu);
					objUIScoreController.UpdateScore ();
					EnableMenu (GlobalEnvironment.idRestartAllMenu);
					if (PlayerPrefs.GetInt (GlobalEnvironment.idGdpr, 0) == 0) {
						EnableMenu (GlobalEnvironment.idGdprMenu);
					} else {
						EnableMenu (GlobalEnvironment.idMainMenu);
					}
				}
			}
		}

		// Enable game menus.
		public void EnableMenu (int idMenu) {
			switch (idMenu) {
			case GlobalEnvironment.idMainMenu:
				if (objUIInputGame) {
					objUIInputGame.EnableGame (false);
				}
				StopCoroutine ("CoroutineLoadingWindow");
				mainMenu.SetActive (true);
				break;
			case GlobalEnvironment.idSettingMenu:
				settingMenu.SetActive (true);
				googleAdsGame.EnableBannerAdmob (true);
				break;
			case GlobalEnvironment.idShopMenu:
				shopMenu.SetActive (true);
				googleAdsGame.EnableBannerAdmob (true);				
				if (googleAdsGame.CheckRewardVideoIsAvailable ()) {
					if (objUIShopController != null) {
						objUIShopController.EnableWatchVideoButton (true);	
					}
				} else {
					if (objUIShopController != null) {
						objUIShopController.EnableWatchVideoButton (false);	
					}					
				}
				break;
			case GlobalEnvironment.idQuitMenu:
				quitMenu.SetActive (true);
				googleAdsGame.EnableBannerAdmob (true);
				break;
			case GlobalEnvironment.idLevelMenu:
				levelMenu.SetActive (true);
				googleAdsGame.EnableBannerAdmob (false);
				break;
			case GlobalEnvironment.idPauseMenu:
				levelMenu.SetActive (true);
				pauseMenu.SetActive (true);
				googleAdsGame.EnableBannerAdmob (true);
				break;
			case GlobalEnvironment.idGameOverMenu:
				levelMenu.SetActive (false);
				gameOverMenu.SetActive (true);
				googleAdsGame.EnableBannerAdmob (true);
				break;
			case GlobalEnvironment.idLoadingWindow:
				loadingWindow.SetActive (true);
				googleAdsGame.EnableBannerAdmob (true);
				break;
			case GlobalEnvironment.idGdprMenu:
				gdprMenu.SetActive (true);
				break;				
			case GlobalEnvironment.idRestartAllMenu:
				mainMenu.SetActive (false);
				settingMenu.SetActive (false);
				shopMenu.SetActive (false);
				quitMenu.SetActive (false);
				levelMenu.SetActive (false);
				pauseMenu.SetActive (false);
				gameOverMenu.SetActive (false);
				loadingWindow.SetActive (false);
				gdprMenu.SetActive (false);
				break;
			default:
				break;
			}
		}

		// Main menu - init
		public void PlayButtonMainMenu () {
			startLoadingWindow ();
		}

		public void SettingButtonMainMenu () {
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idSettingMenu);
		}
			
		public void ShopButtonMainMenu () {
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idShopMenu);
			objUIShopController.ReloadPriceCharacter ();
		}
		// Main menu - end

		// Loading windows - init
		public void startLoadingWindow () {
			StartCoroutine ("CoroutineLoadingWindow");
		}

		IEnumerator CoroutineLoadingWindow () {
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idLoadingWindow);
			if (objUIInputGame) {
				objUIInputGame.EnableGame (false);
			}
			if (objGameController) {
				objGameController.EnableGame (false);
				objGameController.RemoveLevel ();
				yield return new WaitForSeconds (0.5f);
				objGameController.LoadLevel ();
			}
			yield return new WaitForSeconds (1.5f);
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idLevelMenu);
			if (objUIInputGame) {
				objUIInputGame.EnableGame (true);
			}
			if (objGameController) {
				objGameController.EnableGame (true);
			}
			EnableTimeScale (true);
		}
		// Loading windows - end

		// Setting menu - init
		public void BackButtonSettingMenu () {
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idMainMenu);
		} 

		public void SoundButtonSettingMenu () {
			if (objAudioController) {
				objAudioController.SoundButton ();
			}
		}

		public void RateUsButtonSettingMenu () {
			if (linkRateUs != null) {
				if (string.IsNullOrEmpty (linkRateUs)) {
					return;
				}
				Application.OpenURL (linkRateUs);
			}
		}

		public void TermsButtonSettingMenu () {
			if (linkTermsOfUse != null) {
				if (string.IsNullOrEmpty (linkTermsOfUse)) {
					return;
				}
				Application.OpenURL (linkTermsOfUse);
			}
		}
		// Setting menu - end

		// Shop menu - init
		public void BackButtonShopMenu () {
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idMainMenu);
		}

		public void PrevButtonShopMenu () {
			if (objUIShopController) {
				objUIShopController.PrevButton ();
			}
		}

		public void NextButtonShopMenu () {
			if (objUIShopController) {
				objUIShopController.NextButton ();
			}
		}

		public void BuyButtonShopMenu () {
			if (objUIShopController) {
				objUIShopController.BuyButton ();
			}
		}

		public void WatchVideoButtonShopMenu () {
			if (objUIShopController) {
				objUIShopController.WatchVideoButton ();
			}
		}

		public void SelectButtonShopMenu () {
			if (objUIShopController) {
				objUIShopController.SelectButton ();
			}
		}
		// Shop menu - end

		public void OkButtonQuitMenu () {
			Application.Quit();
		}

		public void NoButtonQuitMenu () {
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idMainMenu);
		}
		// Quit menu - end

		// Level menu - init
		public void PauseButtonLevelMenu () {
			if (objUIInputGame) {
				objUIInputGame.EnableGame (false);
			}
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idPauseMenu);
			EnableTimeScale (false);
		}
		// Level menu - end

		// Pause menu - init
		public void HomeButtonPauseMenu () {
			if (objGameController) {
				objGameController.EnableGame (false);
				objGameController.RemoveLevel ();
			}
			EnableTimeScale (true);
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idMainMenu);
		}

		public void ReloadButtonPauseMenu () {
			EnableTimeScale (true);
			startLoadingWindow ();
		}

		public void ContinueButtonPauseMenu () {
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idLevelMenu);
			EnableTimeScale (true);
			if (objUIInputGame) {
				objUIInputGame.EnableGame (true);
			}
		}
		// Pause menu - end

		// Game over menu - init
		public void HomeButtonGameOverMenu () {
			if (objGameController) {
				objGameController.EnableGame (false);
				objGameController.RemoveLevel ();
			}			
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idMainMenu);
			EnableTimeScale (true);
		}

		public void ReloadButtonGameOverMenu () {
			EnableTimeScale (true);
			startLoadingWindow ();
		}
		// Game over  menu - end

		public void UpdateScore () {
			objUIScoreController.UpdateScore ();
		}

		public void UpdateDeathScore (int deaths) {
			objUIScoreController.UpdateDeathScore (deaths);
		}

		public void UpdateKillScore (int kills) {
			objUIScoreController.UpdateKillScore (kills);
		}

		public void UpdateKillBestScore () {
			objUIScoreController.UpdateKillBestScore ();
		}		

		public void UpdateTimeScore (string timeData) {
			objUIScoreController.UpdateTime (timeData);
		}

		public void UpdateAmmunitionScore (int ammunition) {
			objUIScoreController.UpdateAmmunition (ammunition);
		}

		public void UpdateWeaponScore (int weapon) {
			objUIScoreController.UpdateWeapon (weapon);
		}

		public void EnableTimeScale (bool bValue) {
			if (bValue) {
				Time.timeScale = 1.0f;
			} else {
				Time.timeScale = 0.0f;
			}
		}

		public void EscapeButton () {
			#if UNITY_ANDROID
			if (mainMenu.activeSelf) {
				EnableMenu (GlobalEnvironment.idRestartAllMenu);
				EnableMenu (GlobalEnvironment.idMainMenu);
				EnableMenu (GlobalEnvironment.idQuitMenu);
			} else if (settingMenu.activeSelf) {
				EnableMenu (GlobalEnvironment.idRestartAllMenu);
				EnableMenu (GlobalEnvironment.idMainMenu);
			} else if (shopMenu.activeSelf) {
				EnableMenu (GlobalEnvironment.idRestartAllMenu);
				EnableMenu (GlobalEnvironment.idMainMenu);
			} else if (levelMenu.activeSelf && !pauseMenu.activeSelf) {
				EnableTimeScale (false);
				EnableMenu (GlobalEnvironment.idRestartAllMenu);
				EnableMenu (GlobalEnvironment.idLevelMenu);
				EnableMenu (GlobalEnvironment.idPauseMenu);
			}
			#endif
		}

		public void AcceptGdpr () {
			PlayerPrefs.SetInt (GlobalEnvironment.idGdpr, 1);
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idMainMenu);
		}

		public void DeclineGdpr () {
			PlayerPrefs.SetInt (GlobalEnvironment.idGdpr, 1);
			EnableMenu (GlobalEnvironment.idRestartAllMenu);
			EnableMenu (GlobalEnvironment.idMainMenu);			
		}		
	}
}
