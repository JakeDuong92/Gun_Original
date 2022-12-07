// Game score controller.

using UnityEngine.UI;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class UIScoreController : MonoBehaviour 
	{
		private Text scoreMainMenuText = null;
		private Text scoreShopMenuText = null;
		private Text scoreLevelMenuText = null;
		private Text deathScoreLevelMenuText = null;
		private Text timeLevelMenuText = null;
		private Text scoreGameOverMenuText = null;
		private Text deathScoreGameOverMenuText = null;
		private Text ammunitionLevelMenuText = null;

		public void SetScoreMainMenu (GameObject objMenu) {
			var objTexts = objMenu.GetComponentsInChildren <Text> ();
			for (int iloop = 0; iloop < objTexts.Length; iloop++) {
				string getNameText = objTexts [iloop].name;
				if (string.Equals(getNameText, GlobalEnvironment.idScoreMainMenu)) {
					scoreMainMenuText = objTexts [iloop];
				}
			}
		}

		public void SetScoreShopMenu (GameObject objMenu) {
			var objTexts = objMenu.GetComponentsInChildren <Text> ();
			for (int iloop = 0; iloop < objTexts.Length; iloop++) {
				string getNameText = objTexts [iloop].name;
				if (string.Equals(getNameText, GlobalEnvironment.idScoreShopMenu)) {
					scoreShopMenuText = objTexts [iloop];
				}
			}
		}

		public void SetScoreLevelMenu (GameObject objMenu) {
			var objTexts = objMenu.GetComponentsInChildren <Text> ();
			for (int iloop = 0; iloop < objTexts.Length; iloop++) {
				string getNameText = objTexts [iloop].name;
				if (string.Equals(getNameText, GlobalEnvironment.idScoreLevelMenu)) {
					scoreLevelMenuText = objTexts [iloop];
				}
				if (string.Equals(getNameText, GlobalEnvironment.idDeathScoreLevelMenu)) {
					deathScoreLevelMenuText = objTexts [iloop];
				}
				if (string.Equals(getNameText, GlobalEnvironment.idTimeLevelMenu)) {
					timeLevelMenuText = objTexts [iloop];
				}
				if (string.Equals(getNameText, GlobalEnvironment.idAmmunitionLevelMenu)) {
					ammunitionLevelMenuText = objTexts [iloop];
				}
			}
		}

		public void SetScoreGameOverMenu (GameObject objMenu) {
			var objTexts = objMenu.GetComponentsInChildren <Text> ();
			for (int iloop = 0; iloop < objTexts.Length; iloop++) {
				string getNameText = objTexts [iloop].name;
				if (string.Equals(getNameText, GlobalEnvironment.idScoreGameOverMenu)) {
					scoreGameOverMenuText = objTexts [iloop];
				}
				if (string.Equals(getNameText, GlobalEnvironment.idDeathScoreGameOverMenu)) {
					deathScoreGameOverMenuText = objTexts [iloop];
				}
			}
		}

		public void UpdateScore () {
			int getScore = PlayerPrefs.GetInt (GlobalEnvironment.idScore, 0);
			if (scoreMainMenuText) {
				scoreMainMenuText.text = getScore.ToString ();
			}
			if (scoreShopMenuText) {
				scoreShopMenuText.text = getScore.ToString ();
			}
		}

		// Update the death score the player had.
		public void UpdateDeathScore (int deaths) {
			if (deaths < 0) {
				return;
			}
			if (deathScoreLevelMenuText) {
				deathScoreLevelMenuText.text = deaths.ToString ();
			}
		}

		// Update the death score that the player achieved.
		public void UpdateKillScore (int kills) {
			if (scoreLevelMenuText) {
				scoreLevelMenuText.text = kills.ToString ();
			}
			if (scoreGameOverMenuText) {
				scoreGameOverMenuText.text = kills.ToString ();
			}
		}

		// Update the best score the player achieved.
		public void UpdateKillBestScore () {
			int countBestKill = PlayerPrefs.GetInt (GlobalEnvironment.idBestKillScore, 0);	
			if (deathScoreGameOverMenuText) {
				deathScoreGameOverMenuText.text = countBestKill.ToString ();
			}
		}

		public void UpdateTime (string timeData) {
			if (timeLevelMenuText) {
				timeLevelMenuText.text = timeData;
			}
		}

		public void UpdateAmmunition (int ammunition) {
			if (ammunition < 0) {
				return;
			}
			if (ammunitionLevelMenuText) {
				ammunitionLevelMenuText.text = ammunition.ToString ();
			}
		}

		public void UpdateWeapon (int weapon) {

		}
	}
}
