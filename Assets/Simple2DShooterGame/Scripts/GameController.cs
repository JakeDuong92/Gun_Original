// Create the level and characters.

using System.Collections;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{	
	public class GameController : MonoBehaviour 
	{
		private bool isEnableGame = false;
		private UIController objUIController;
		public float counterTime = 120.0f; // Initial counter time in seconds.
		private float timeGame = 120.0f; // Current counter time in seconds.
		private bool isEnableTime = false;
		private ToolsGame objToolsGame;
		private PlayerController objPlayerController;
		public GameObject backgroundLevelPrefab;
		public GameObject baseLevelPrefab;
		public GameObject[] allLevelPrefab;
		public GameObject[] weaponsCharacters; // Weapons of the characters.
		private int countKillsBots = 0; // Bot kill counter.
		private GameObject objCurrentLevel = null;
		private LevelProperties objLevelProperties = null;
		private bool lockTimeGame = false;
		private bool lockFinalTime = false;
		private AudioController objAudioController;
		GoogleAdsGame googleAdsGame;
		UIShopController objUIShopController;
		 

		void Awake () {
			DataFirstStart ();
			objUIController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIController> ();
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
			objPlayerController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<PlayerController> ();
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
			googleAdsGame = GameObject.Find(GlobalEnvironment.idAdsController).GetComponent<GoogleAdsGame> ();
			objUIShopController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIShopController> ();
		}
		
		// Update is called once per frame
		void Update () {
			if (!isEnableGame) {
				return;
			}
			if (isEnableTime) {
				StarstDownTime ();
			}

		}

		// Initialize the information the first time of the boot.
		void DataFirstStart () {
			int getIdCode = PlayerPrefs.GetInt(GlobalEnvironment.idGameFirstTime,0);
			if (GlobalEnvironment.idCodefirsttime != getIdCode) {
				PlayerPrefs.SetInt (GlobalEnvironment.idGameFirstTime, GlobalEnvironment.idCodefirsttime);
				PlayerPrefs.SetInt (GlobalEnvironment.idGameTutorial, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.id00Character,1);
				PlayerPrefs.SetInt (GlobalEnvironment.id01Character, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.id02Character, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.id03Character, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.id04Character, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.id05Character, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.id06Character, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.idCurrentCharacter, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.idScore, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.idBestKillScore, 0);
				PlayerPrefs.SetInt (GlobalEnvironment.idEnableSound, 1);
				PlayerPrefs.SetInt (GlobalEnvironment.idGdpr, 0);
			}
		}

		// Load the level.
		public void LoadLevel () {
			EnableDownTime (false);
			lockTimeGame = false;
			CreateBackgroundLevel ();
			objUIController.UpdateAmmunitionScore (0);
			objUIController.UpdateWeaponScore (GlobalEnvironment.idGunWeapon);
			objUIController.UpdateDeathScore (0);
			CreateLevel ();
			SetDownTime (counterTime);
			countKillsBots = 0;
			UpdateKillsBots (false);
			EnableDownTime (true);			
		}

		// Remove level.
		public void RemoveLevel () {
			var getObjs = GameObject.FindGameObjectsWithTag (GlobalEnvironment.idLevelTag);
			foreach (GameObject getObj in getObjs) {
				Destroy (getObj);
			}
			var getObjsWeapons = GameObject.FindGameObjectsWithTag (GlobalEnvironment.idChangeWeaponTag);
			foreach (GameObject getObj in getObjsWeapons) {
				Destroy (getObj);
			}
			var getObjsShowWeapons = GameObject.FindGameObjectsWithTag (GlobalEnvironment.idShowWeaponsTag);
			foreach (GameObject getObj in getObjsShowWeapons) {
				Destroy (getObj);
			}
			var getObjsBots = GameObject.FindGameObjectsWithTag (GlobalEnvironment.idBotTag);
			foreach (GameObject getObj in getObjsBots) {
				Destroy (getObj);
			}
			var getObjsAllWeapons = GameObject.FindGameObjectsWithTag (GlobalEnvironment.idWeaponTag);
			foreach (GameObject getObj in getObjsAllWeapons) {
				Destroy (getObj);
			}
			if (objPlayerController) {
				objPlayerController.ShowPlayer (true);
				objPlayerController.RemovePlayer ();
			}
			objLevelProperties = null;
			StopCoroutine ("CoroutineFinalTime");
		}

		public void SetDownTime (float timeData) {
			timeGame = timeData;
		}

		// Enable the counter.
		public void EnableDownTime (bool enableTime) {
			isEnableTime = enableTime;
			lockFinalTime = false;
			StopCoroutine ("CoroutineFinalTime");
			objUIController.UpdateTimeScore (GlobalEnvironment.idStartTimeLevelMenu);
		}

		// Start the counter.
		public void StarstDownTime () {
			int minutes = (int)(timeGame / 60);	
			int seconds = (int)(timeGame % 60);
			string minutesDesc = minutes.ToString ("0");
			string secondsDesc = seconds.ToString ("00");
			string timeDesc = minutesDesc + ":" + secondsDesc;
			objUIController.UpdateTimeScore (timeDesc);
			timeGame -= Time.deltaTime;
			if (timeGame < 11.0f && !lockFinalTime) {
				StartCoroutine ("CoroutineFinalTime");
				lockFinalTime = true;
			}
			if (timeGame < 0.0f && !lockTimeGame) {
				lockTimeGame = true;				
				objUIController.EnableTimeScale (false);
				if (objLevelProperties) {
					objLevelProperties.EnableGame (false);
				}
				int countBestKill = PlayerPrefs.GetInt (GlobalEnvironment.idBestKillScore, 0);
				if (countKillsBots > countBestKill) {
					PlayerPrefs.SetInt (GlobalEnvironment.idBestKillScore, countKillsBots);					
				}
				objUIController.UpdateKillBestScore ();				
				objUIController.EnableMenu (GlobalEnvironment.idGameOverMenu);
				StopCoroutine ("CoroutineFinalTime");
				googleAdsGame.ShowInterstialAdmob ();
				googleAdsGame.VideoAdActivation ();
			}
		}

		IEnumerator CoroutineFinalTime () {
			while (true) {
				objAudioController.PlaySounds (GlobalEnvironment.idFinalTimeSound);
				yield return new WaitForSeconds (1.0f);
			}
		}

		public float GetCurrentTime () {
			return timeGame;
		}

		public float GetInitTime () {
			return counterTime;
		}

		// Enable the game.
		public void EnableGame (bool bGame) {
			isEnableGame = bGame;
			if (objCurrentLevel && objLevelProperties) {
				objLevelProperties.EnableGame (bGame);
			}
		}

		void CreateBackgroundLevel () {
			if (!backgroundLevelPrefab) {
				return;
			}
			GameObject getBackground = Instantiate (backgroundLevelPrefab,Vector2.zero, backgroundLevelPrefab.transform.rotation) as GameObject;
			if (getBackground) {
				objToolsGame.SetParentSprite (getBackground);
			}
		}

		void CreateLevel () {
			if (!baseLevelPrefab) {
				return;
			}
			GameObject baseLevel = Instantiate (baseLevelPrefab, Vector2.zero, baseLevelPrefab.transform.rotation) as GameObject;
			if (baseLevel) {
				objToolsGame.SetParentSprite (baseLevel);
			}
			if (allLevelPrefab.Length > 0) {
				objCurrentLevel = Instantiate (allLevelPrefab[0], Vector2.zero, allLevelPrefab[0].transform.rotation) as GameObject;
				if (objCurrentLevel) {
					objToolsGame.SetParentSprite (objCurrentLevel);
					objLevelProperties = objCurrentLevel.GetComponent<LevelProperties> ();
				}
			}
		}

		public void UpdateKillsBots (bool bValue) {
			if (bValue) {
				countKillsBots++;
				int countDeathPlayer = PlayerPrefs.GetInt (GlobalEnvironment.idScore, 0);
				countDeathPlayer++;
				PlayerPrefs.SetInt (GlobalEnvironment.idScore, countDeathPlayer);
				objUIController.UpdateScore ();
			}
			objUIController.UpdateKillScore (countKillsBots);
		}

		// Returns the number of bot deaths.
		public int GetKillsBots () {
			return countKillsBots;
		}

		public void DeathPlayer () {
			if (objLevelProperties) {
				objLevelProperties.DeathPlayer ();
			}
		}

		public void ChangeWeapon (int id) {
			if (objLevelProperties) {
				objLevelProperties.ChangeWeapon (id);
			}
		}

	}
}
