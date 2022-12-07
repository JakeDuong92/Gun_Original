// Adds events to the ui buttons.

using UnityEngine.UI;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class UISetListener : MonoBehaviour 
	{
		private UIController objUIController;
		void Awake () {
			objUIController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIController> ();
		}
	
		public void SetupButtons (int idMenu, GameObject objMenu) {
			switch (idMenu) {
			case GlobalEnvironment.idMainMenu:
				MainMenuSetupButtons (objMenu);
				break;
			case GlobalEnvironment.idSettingMenu:
				SettingMenuSetupButtons (objMenu);
				break;
			case GlobalEnvironment.idShopMenu:
				ShopMenuSetupButtons (objMenu);
				break;
			case GlobalEnvironment.idQuitMenu:
				QuitMenuSetupButtons (objMenu);
				break;
			case GlobalEnvironment.idLevelMenu:
				LevelMenuSetupButtons (objMenu);
				break;
			case GlobalEnvironment.idPauseMenu:
				PauseMenuSetupButtons (objMenu);
				break;
			case GlobalEnvironment.idGameOverMenu:
				GameOverMenuSetupButtons (objMenu);
				break;
			default:
				break;
			}
		}

		// Main menu - init
		void MainMenuSetupButtons (GameObject objMenu) {
			var objButtons = objMenu.GetComponentsInChildren <Button> ();
			for (int iloop = 0; iloop < objButtons.Length; iloop++) {
				string getNameButton = objButtons [iloop].name;
				if (string.Equals(getNameButton, GlobalEnvironment.idPlayButtonMainMenu)) {
					objButtons [iloop].onClick.AddListener (PlayMainMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idShopButtonMainMenu)) {
					objButtons [iloop].onClick.AddListener (ShopMainMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idSettingButtonMainMenu)) {
					objButtons [iloop].onClick.AddListener (SettingMainMenuOnClick);
				}
			}
		}

		void PlayMainMenuOnClick () {
			if (objUIController) {
				objUIController.PlayButtonMainMenu ();
			}
		}

		void SettingMainMenuOnClick () {
			if (objUIController) {
				objUIController.SettingButtonMainMenu ();
			}
		}

		void ShopMainMenuOnClick () {
			if (objUIController) {
				objUIController.ShopButtonMainMenu ();
			}
		}
		// Main menu - end

		// Setting menu - init
		void SettingMenuSetupButtons (GameObject objMenu) {
			var objButtons = objMenu.GetComponentsInChildren <Button> ();
			for (int iloop = 0; iloop < objButtons.Length; iloop++) {
				string getNameButton = objButtons [iloop].name;
				if (string.Equals(getNameButton, GlobalEnvironment.idBackButtonSettingMenu)) {
					objButtons [iloop].onClick.AddListener (BackSettingMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idSoundButtonSettingMenu)) {
					objButtons [iloop].onClick.AddListener (SoundSettingMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idRateUsButtonSettingMenu)) {
					objButtons [iloop].onClick.AddListener (RateUsSettingMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idTermsButtonSettingMenu)) {
					objButtons [iloop].onClick.AddListener (TermsSettingMenuOnClick);
				}
			}
		}

		void BackSettingMenuOnClick () {
			if (objUIController) {
				objUIController.BackButtonSettingMenu ();
			}
		}

		void SoundSettingMenuOnClick () {
			if (objUIController) {
				objUIController.SoundButtonSettingMenu ();
			}
		}

		void RateUsSettingMenuOnClick () {
			if (objUIController) {
				objUIController.RateUsButtonSettingMenu ();
			}
		}

		void TermsSettingMenuOnClick () {
			if (objUIController) {
				objUIController.TermsButtonSettingMenu ();
			}
		}
		// Setting menu - end

		// Shop menu - init
		void ShopMenuSetupButtons (GameObject objMenu) {
			var objButtons = objMenu.GetComponentsInChildren <Button> ();
			for (int iloop = 0; iloop < objButtons.Length; iloop++) {
				string getNameButton = objButtons [iloop].name;
				if (string.Equals(getNameButton, GlobalEnvironment.idBackButtonShopMenu)) {
					objButtons [iloop].onClick.AddListener (BackShopMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idPrevButtonShopMenu)) {
					objButtons [iloop].onClick.AddListener (PrevShopMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idNextButtonShopMenu)) {
					objButtons [iloop].onClick.AddListener (NextShopMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idBuyButtonShopMenu)) {
					objButtons [iloop].onClick.AddListener (BuyShopMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idWatchVideoButtonShopMenu)) {
					objButtons [iloop].onClick.AddListener (WatchVideoShopMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idSelectButtonShopMenu)) {
					objButtons [iloop].onClick.AddListener (SelectShopMenuOnClick);
				}
			}
		}

		void BackShopMenuOnClick () {
			if (objUIController) {
				objUIController.BackButtonShopMenu ();
			}
		}

		void PrevShopMenuOnClick () {
			if (objUIController) {
				objUIController.PrevButtonShopMenu ();
			}
		}

		void NextShopMenuOnClick () {
			if (objUIController) {
				objUIController.NextButtonShopMenu ();
			}
		}

		void BuyShopMenuOnClick () {
			if (objUIController) {
				objUIController.BuyButtonShopMenu ();
			}
		}

		void WatchVideoShopMenuOnClick () {
			if (objUIController) {
				objUIController.WatchVideoButtonShopMenu ();
			}
		}

		void SelectShopMenuOnClick () {
			if (objUIController) {
				objUIController.SelectButtonShopMenu ();
			}
		}
		// Shop menu - end

		// Quit menu - init
		void QuitMenuSetupButtons (GameObject objMenu) {
			var objButtons = objMenu.GetComponentsInChildren <Button> ();
			for (int iloop = 0; iloop < objButtons.Length; iloop++) {
				string getNameButton = objButtons [iloop].name;
				if (string.Equals(getNameButton, GlobalEnvironment.idOkButtonQuitMenu)) {
					objButtons [iloop].onClick.AddListener (OkQuitMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idNoButtonQuitMenu)) {
					objButtons [iloop].onClick.AddListener (NoQuitMenuOnClick);
				}
			}
		}

		void OkQuitMenuOnClick () {
			if (objUIController) {
				objUIController.OkButtonQuitMenu ();
			}
		}

		void NoQuitMenuOnClick () {
			if (objUIController) {
				objUIController.NoButtonQuitMenu ();
			}
		}
		// Quit menu - end

		// Level menu - init
		void LevelMenuSetupButtons (GameObject objMenu) {
			var objButtons = objMenu.GetComponentsInChildren <Button> ();
			for (int iloop = 0; iloop < objButtons.Length; iloop++) {
				string getNameButton = objButtons [iloop].name;
				if (string.Equals(getNameButton, GlobalEnvironment.idPauseButtonLevelMenu)) {
					objButtons [iloop].onClick.AddListener (PauseLevelMenuOnClick);
				}
			}
		}

		void PauseLevelMenuOnClick () {
			if (objUIController) {
				objUIController.PauseButtonLevelMenu ();
			}
		}
		// Level menu - end

		// Pause menu - init
		void PauseMenuSetupButtons (GameObject objMenu) {
			var objButtons = objMenu.GetComponentsInChildren <Button> ();
			for (int iloop = 0; iloop < objButtons.Length; iloop++) {
				string getNameButton = objButtons [iloop].name;
				if (string.Equals(getNameButton, GlobalEnvironment.idHomeButtonPauseMenu)) {
					objButtons [iloop].onClick.AddListener (HomePauseMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idReloadButtonPauseMenu)) {
					objButtons [iloop].onClick.AddListener (ReloadPauseMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idContinueButtonPauseMenu)) {
					objButtons [iloop].onClick.AddListener (ContinuePauseMenuOnClick);
				}
			}
		}

		void HomePauseMenuOnClick () {
			if (objUIController) {
				objUIController.HomeButtonPauseMenu ();
			}
		}

		void ReloadPauseMenuOnClick () {
			if (objUIController) {
				objUIController.ReloadButtonPauseMenu ();
			}
		}

		void ContinuePauseMenuOnClick () {
			if (objUIController) {
				objUIController.ContinueButtonPauseMenu ();
			}
		}
		// Pause menu - end

		// Game Over menu - init
		void GameOverMenuSetupButtons (GameObject objMenu) {
			var objButtons = objMenu.GetComponentsInChildren <Button> ();
			for (int iloop = 0; iloop < objButtons.Length; iloop++) {
				string getNameButton = objButtons [iloop].name;
				if (string.Equals(getNameButton, GlobalEnvironment.idHomeButtonGameOverMenu)) {
					objButtons [iloop].onClick.AddListener (HomeGameOverMenuOnClick);
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idReloadButtonGameOverMenu)) {
					objButtons [iloop].onClick.AddListener (ReloadGameOverMenuOnClick);
				}
			}
		}

		void HomeGameOverMenuOnClick () {
			if (objUIController) {
				objUIController.HomeButtonGameOverMenu ();
			}
		}

		void ReloadGameOverMenuOnClick () {
			if (objUIController) {
				objUIController.ReloadButtonGameOverMenu ();
			}
		}
		// Game Over menu - end
	}
}
