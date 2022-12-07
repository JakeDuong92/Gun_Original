// Level 0.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class B00Level : MonoBehaviour 
	{
		private GameController objGameController;
		private PlayerController objPlayerController;
		private ToolsGame objToolsGame;
		private BotsData objBotsData;
		public GameObject positionInitPlayer;
		public GameObject objDesignLevel;
		public GameObject[] botsCheckPoint;
		public GameObject[] weaponsCheckPoint;
		public GameObject[] changeWeapons;
		public GameObject[] wallLevel;
		private bool isEnableGame = false;
		private List<GameObject> listBots = new List<GameObject>();
		private int countNameBot = 0;
		private int countDeathPlayer = 0; // Player death counter.
		private UIController objUIController;
		private int countDifficulty = 0;
		private int idWeaponCurrent = 0;
		private List<GameObject> listWeapons = new List<GameObject>();
		private bool lockLife = false;
		private bool lockAmmunition = false;
		private GlobalEnvironment.PlayerProperties botProperties;
		private bool isAmmunitionCoroutine = false;
		private bool isLifeCoroutine = false;
		private bool lockWave = false;
		private bool isWaveCoroutine = false;
		private bool lockBazooka = false;
		private bool isBazookaCoroutine = false;
		private CameraController objCameraController;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> (); 
			objPlayerController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<PlayerController> ();
			objBotsData = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<BotsData> ();
			objGameController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<GameController> ();
			objUIController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIController> ();
			botProperties = new GlobalEnvironment.PlayerProperties ();
			botProperties.distanceAttack = -1;
			objCameraController = Camera.main.GetComponent<CameraController> ();
		}

		// Use this for initialization.
		void Start () {
			CreateWall ();
			objToolsGame.SetPositionWorldPointSprite (positionInitPlayer, objDesignLevel);
			positionInitPlayer.SetActive (false);
			idWeaponCurrent = GlobalEnvironment.idGunWeapon;
			CreatePlayer (positionInitPlayer.transform.position, idWeaponCurrent);
			ResetPlayer ();
			ResetLevel ();
			StartCoroutine ("CoroutineStatusGame");
		}
		
		// Update is called once per frame.
		void Update () {
			if (!isEnableGame) {
				return;
			}
		}

		// Enable the game.
		public void EnableGame (bool bGame) {
			isEnableGame = bGame;
			objPlayerController.EnableGame (bGame);
			objPlayerController.ShowPlayer (bGame);
		}

		void CreatePlayer (Vector2 position, int idWeapon) {
			int idPlayer = PlayerPrefs.GetInt(GlobalEnvironment.idCurrentCharacter,0);
			GlobalEnvironment.PlayerProperties botProperties = new GlobalEnvironment.PlayerProperties ();
			botProperties.positionPlayer = position;
			botProperties.idPlayer = idPlayer;	
			botProperties.idWeapon = idWeapon;
			objPlayerController.CreatePlayer (botProperties);
			objPlayerController.ShowPlayer (false);
		}

		void ResetPlayer () {
			if (!positionInitPlayer) {
				return;
			}
			StopCoroutine ("CoroutineDeathPlayer");
		}

		void ResetLevel () {
			if (!objDesignLevel) {
				return;
			}
			lockLife = false;
			lockAmmunition = false;
			isAmmunitionCoroutine = false;
			isLifeCoroutine = false;
			lockWave = false;
			isBazookaCoroutine = false;
			lockBazooka = false;						
			StopCoroutine ("CoroutineAmmunition");
			StopCoroutine ("CoroutineLife");
			StopCoroutine ("CoroutineWave");
			StopCoroutine ("CoroutineBazooka");			
			objUIController.UpdateDeathScore (0);
			objDesignLevel.SetActive (false);
			// Bot position.
			for (int aloop = 0; aloop < botsCheckPoint.Length; aloop++) {
				objToolsGame.SetPositionWorldPointSprite (botsCheckPoint [aloop], objDesignLevel);
				botsCheckPoint [aloop].SetActive (false);
			}
			// Weapons position.
			for (int aloop = 0; aloop < weaponsCheckPoint.Length; aloop++) {
				objToolsGame.SetPositionWorldPointSprite (weaponsCheckPoint [aloop], objDesignLevel);
				weaponsCheckPoint [aloop].SetActive (false);
			}
			int tutorialStatus = PlayerPrefs.GetInt(GlobalEnvironment.idGameTutorial, 0);
			if (tutorialStatus == 1) {
				countDifficulty = 3;
			}
			if (listWeapons.Count > 0) {
				listWeapons.Clear ();
			}
			if (listBots.Count > 0) {
				listBots.Clear ();
			}
		}

		public void DeathPlayer () {
			countDeathPlayer++;
			objUIController.UpdateDeathScore (countDeathPlayer);
			objCameraController.ShakeCamera ();
			StartCoroutine ("CoroutineDeathPlayer");
		}

		IEnumerator CoroutineDeathPlayer () {
			yield return new WaitForSeconds (3.0f);
			if (objPlayerController) {
				objPlayerController.ShowPlayer (true);
				objPlayerController.RemovePlayer ();
				CreatePlayer (positionInitPlayer.transform.position, idWeaponCurrent);
				ResetPlayer ();
				objPlayerController.ShowPlayer (true);
				objPlayerController.EnableGame (true);
			}
		}

		void CreateBots (GlobalEnvironment.PlayerProperties botProperties) {
			GameObject aBot = new GameObject (countNameBot.ToString () + " Boot");
			countNameBot++;
			objToolsGame.SetParentSprite (aBot);
			objToolsGame.SetTag (aBot, GlobalEnvironment.idBotTag);
			aBot.AddComponent <BotController> ();
			BotController objBotController = aBot.GetComponent <BotController> ();
			objBotController.SetGeometryPlayer (botProperties.sizePlayer, botProperties.positionPlayer, botProperties.modePosition);
			objBotController.CreatePlayer (botProperties);
			objBotController.EnableGame (true);
			objBotController.SetModeAttack (botProperties.modeAttack);
			if (botProperties.distanceAttack > 0) {
				objBotController.SetDistanceAttack (botProperties.distanceAttack);
			}
			listBots.Add (aBot);
		}

		IEnumerator CoroutineStatusGame () {
			while (true) {
				yield return new WaitForSeconds (2.0f);
				StatusGame ();
			}
		}

		// Verify player status and bots. 
		void StatusGame () {
			if (!isEnableGame || !objPlayerController || botsCheckPoint.Length == 0 || weaponsCheckPoint.Length == 0) {
				return;
			}
			Difficulty01Game ();
		}

		// Difficulty level - 01.
		void Difficulty01Game () {
			float initLife = objPlayerController.GetInitLife ();
			float currentLife = objPlayerController.GetCurrentLife ();
			float initAmmunition = objPlayerController.GetInitAmmunition ();
			float currentAmmunition = objPlayerController.GetCurrentAmmunition ();	
			switch (countDifficulty) {
				case 0: // Crear 2 bots.
				botProperties.sizePlayer = objBotsData.GetSizePlayer ();
				botProperties.positionPlayer = botsCheckPoint [0].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (GlobalEnvironment.idGunWeapon);
				botProperties.speedPercentage = 30;
				botProperties.searchTime = 2.0f;
				botProperties.modeAttack = GlobalEnvironment.idHardBotAttack;
				botProperties.distanceAttack = -1;
				CreateBots (botProperties);	
				botProperties.positionPlayer = botsCheckPoint [2].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (GlobalEnvironment.idGunWeapon);
				botProperties.speedPercentage = 50;
				botProperties.searchTime = 0.5f;
				botProperties.modeAttack = GlobalEnvironment.idEasyBotAttack;
				CreateBots (botProperties);	
				botProperties.positionPlayer = botsCheckPoint [4].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (GlobalEnvironment.idGunWeapon);
				botProperties.speedPercentage = 100;
				botProperties.searchTime = 1.0f;
				botProperties.modeAttack = GlobalEnvironment.idMediumBotAttack;
				CreateBots (botProperties);			
				botProperties.positionPlayer = botsCheckPoint [5].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (GlobalEnvironment.idGunWeapon);
				botProperties.speedPercentage = 70;
				botProperties.searchTime = 1.5f;
				botProperties.modeAttack = GlobalEnvironment.idHardBotAttack;
				CreateBots (botProperties);																
				countDifficulty++;
				break;
				
				case 1: // Check that bots have been removed.
				if (currentLife <= (initLife / 1.5f) && !lockLife) {
					CreateWeapon (GlobalEnvironment.idMachineGunWeapon);
					lockLife = true;
				}
				if (currentAmmunition <= (initAmmunition / 2.0f) && !lockAmmunition) {
					CreateWeapon (GlobalEnvironment.idMachineGunWeapon);
					lockAmmunition = true;
				}
				if (GetCountBotList (true) == 0) {
					listBots.Clear ();
					countDifficulty++;
				}
				break;

				case 2: // Create a weapon.
				PlayerPrefs.SetInt(GlobalEnvironment.idGameTutorial, 1);
				countDifficulty++;
				break;

				case 3:
					Difficulty02Game ();
				break;

				default:
				break;

			}
		}
		
		void Difficulty02Game () {
			float initLife = objPlayerController.GetInitLife ();
			float currentLife = objPlayerController.GetCurrentLife ();
			float initAmmunition = objPlayerController.GetInitAmmunition ();
			float currentAmmunition = objPlayerController.GetCurrentAmmunition ();		
			float initTimeCounter = objGameController.GetInitTime ();
			float currentTimeCounter = objGameController.GetCurrentTime ();				
			if (currentLife <= (initLife / 1.5f) && !lockLife) {
				CreateWeapon (GlobalEnvironment.idMachineGunWeapon);
				lockLife = true;
			}
			if (currentAmmunition <= (initAmmunition / 2.0f) && !lockAmmunition) {
				CreateWeapon (GlobalEnvironment.idMachineGunWeapon);
				lockAmmunition = true;
			}
			int countBots = GetCountBotList (false);
			if (countBots == 1) {
				CreateRandomBot (3);			
			}			
			if (countBots == 0) {
				CreateRandomBot (1);		
			}
			if (currentTimeCounter < (initTimeCounter / 2) && !lockWave) {
				CreateRandomBot (3);									
				lockWave = true;
				if (!isWaveCoroutine) {
					StartCoroutine ("CoroutineWave");
				}				
			}
			if (currentTimeCounter < (initTimeCounter / 1.5f) && !lockBazooka && 
				currentAmmunition <= (initAmmunition / 2.0f) && currentLife <= (initLife / 1.5f)) {
				CreateWeapon (GlobalEnvironment.idBazookaWeapon);							
				lockBazooka = true;
				if (!isBazookaCoroutine) {
					StartCoroutine ("CoroutineBazooka");
				}				
			}						
		}

		// Returns the number of bots that are active.
		int GetCountBotList (bool remove) {
			int countTotal = 0;
			for (int aloop = 0; aloop < listBots.Count; aloop++) {
				if (listBots [aloop]) {
					if (listBots [aloop].activeSelf ) {
						BotController objBotController = listBots [aloop].GetComponent <BotController> ();
						if (objBotController) {
							if (objBotController.CheckPlayer ()) {
								countTotal++;
							} else {
								if (remove) {
									Destroy (listBots [aloop]);
								}
							}
						}	
					} 
				}
			}
			return countTotal;
		}

		public void ChangeWeapon (int id) {
			idWeaponCurrent = id;
			if (!isAmmunitionCoroutine) {
				StartCoroutine ("CoroutineAmmunition");
			}
			if (!isLifeCoroutine) {
				StartCoroutine ("CoroutineLife");
			}							
		}

		IEnumerator CoroutineAmmunition () {
			isAmmunitionCoroutine = true;
			yield return new WaitForSeconds (10.0f);
			isAmmunitionCoroutine = false;
			lockAmmunition = false;
		}

		IEnumerator CoroutineLife () {
			isLifeCoroutine = true;
			yield return new WaitForSeconds (10.0f);
			isLifeCoroutine = false;
			lockLife = false;
		}

		IEnumerator CoroutineWave () {
			isWaveCoroutine = true;
			yield return new WaitForSeconds (30.0f);
			isWaveCoroutine = false;
			lockWave = false;
		}		

		IEnumerator CoroutineBazooka () {
			isBazookaCoroutine = true;
			yield return new WaitForSeconds (30.0f);
			isBazookaCoroutine = false;
			lockBazooka = false;
		}	

		void CreateWeapon (int id) {
			GameObject weaponSelect;
			float xLength = 0.0f;
			int positionCheckPoint = -1;
			bool isExistPoint = false;
			int weaponInUseCheckPoint = -1;
			List<float [,]> listDistancePlayer = new List<float [,]>();
			for (int aloop = 0; aloop < listWeapons.Count; aloop++ ) {
				if (listWeapons [aloop]) {
					if (listWeapons [aloop].activeSelf) {
						var objChangeWeapon = listWeapons [aloop].GetComponent <ChangeWeapon>();
						if (objChangeWeapon) {
							weaponInUseCheckPoint = objChangeWeapon.GetModelWeapon ();
							if (weaponInUseCheckPoint == id) { 
								return; // It does not allow duplicate weapons.
							}							
						}
					}					
				}
			}			
			if (changeWeapons.Length == 3 || weaponsCheckPoint.Length != 0) {
				Vector2 positionPlayerCurrent = objPlayerController.GetPositionPlayer ();
				for (int aloop = 0; aloop < weaponsCheckPoint.Length; aloop++) {
					xLength = Mathf.Abs (weaponsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
					float floop = (float) aloop * 1.0f;
					float[,] fArray = new float [,] { { xLength, floop } };
					listDistancePlayer.Add (fArray);		
				}
				listDistancePlayer.Sort (SortDistance);
				if (listWeapons.Count == 0) {
					float [,] fArray = listDistancePlayer [0];
					float position = fArray [0, 1];
					positionCheckPoint = (int)position;					
				} else {
					for (int bloop = 0; bloop < listDistancePlayer.Count; bloop++) {
						bool positionExist = false;
						float [,] fArray = listDistancePlayer [bloop];
						int position = (int)fArray [0, 1];						
						for (int aloop = 0; aloop < listWeapons.Count; aloop++ ) {
							if (listWeapons [aloop]) {
								if (listWeapons [aloop].activeSelf) {
									var objChangeWeapon = listWeapons [aloop].GetComponent <ChangeWeapon>();
									if (objChangeWeapon) {
										if (position == objChangeWeapon.GetPositionArray ()) {
											positionExist = true;
											break;
										}
									}
								}					
							}
						}
						if (!positionExist) {
							positionCheckPoint = position;
							break;
						}
					}
				}
				if (positionCheckPoint == -1 ) {
					isExistPoint = true;
				}
				if (!isExistPoint) {
					switch (id) {
					case GlobalEnvironment.idGunWeapon:
						weaponSelect = changeWeapons [0];
					break;
					case GlobalEnvironment.idMachineGunWeapon:
						weaponSelect = changeWeapons [1];
					break;
					case GlobalEnvironment.idBazookaWeapon:
						weaponSelect = changeWeapons [2];
					break;
					default:
						weaponSelect = changeWeapons [0];
					break;
					}
					GameObject getObj = Instantiate (weaponSelect, weaponsCheckPoint [positionCheckPoint].transform.position, changeWeapons [id].transform.rotation) as GameObject;
					if (getObj) {
						var objChangeWeapon = getObj.GetComponent <ChangeWeapon>();
						if (objChangeWeapon) {
							objChangeWeapon.SetPositionArray (positionCheckPoint);
						}
						listWeapons.Add (getObj);
					}
				}				
			}
		}

     	static int SortDistance (float [,] aArray, float [,] bArray) {
			float aData = aArray [0, 0];
			float bData = bArray [0, 0];			 
         	return aData.CompareTo (bData);
     	}

		// Create bots and weapons.
		void RandomBots () {
			float xLengthLast = 0.0f;
			float xLength = 0.0f;
			int positionCheckPoint = 0;		
			float initLife = objPlayerController.GetInitLife ();
			float currentLife = objPlayerController.GetCurrentLife ();
			float initAmmunition = objPlayerController.GetInitAmmunition ();
			float currentAmmunition = objPlayerController.GetCurrentAmmunition ();			
			int modelWeapon = objPlayerController.GetModelWeapon ();
			Vector2 positionPlayerCurrent = objPlayerController.GetPositionPlayer ();
			int botsCount = 0;
			botsCount = GetCountBotList (true);		
			if (currentLife <= (initLife / 1.5f) && !lockLife) {
				if (botsCount >= 3) {
					CreateWeapon (GlobalEnvironment.idBazookaWeapon);
				} else {
					CreateWeapon (GlobalEnvironment.idMachineGunWeapon);
				}				
				lockLife = true;
			}
			if (currentAmmunition <= (initAmmunition / 2.0f) && !lockAmmunition) {
				CreateWeapon (GlobalEnvironment.idMachineGunWeapon);
				lockAmmunition = true;
			}
			if (botsCount == 0) {
				for (int aloop = 0; aloop < botsCheckPoint.Length; aloop++) {
					if (positionPlayerCurrent.x >= 0 && botsCheckPoint [aloop].transform.position.x >= 0) { 
						if (aloop != 0) {
							xLength = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
							if (xLength < xLengthLast) {
								xLengthLast = xLength;
								positionCheckPoint = aloop;
							}
						} else {
							xLengthLast = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
							positionCheckPoint = aloop;
						}
					} else {
						if (aloop != 0) {
							xLength = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
							if (xLength < xLengthLast) {
								xLengthLast = xLength;
								positionCheckPoint = aloop;
							}
						} else {
							xLengthLast = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
							positionCheckPoint = aloop;
						}
					} 				
				}
				botProperties.sizePlayer = objBotsData.GetSizePlayer ();
				botProperties.positionPlayer = botsCheckPoint [positionCheckPoint].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (modelWeapon);
				botProperties.speedPercentage = RandomSpeedPercentage ();
				botProperties.searchTime = RandomSearchTime ();
				botProperties.modeAttack = GlobalEnvironment.idHardBotAttack;
				CreateBots (botProperties);		
				botProperties.sizePlayer = objBotsData.GetSizePlayer ();
				int posBot = RandomPositionBot ();
				botProperties.positionPlayer = botsCheckPoint [posBot].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				int modelWeaponTmp = modelWeapon;
				if (modelWeapon == GlobalEnvironment.idBazookaWeapon) {
					modelWeaponTmp = GlobalEnvironment.idMachineGunWeapon;
				}
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (modelWeaponTmp);
				botProperties.speedPercentage = RandomSpeedPercentage ();
				botProperties.searchTime = RandomSearchTime ();
				botProperties.modeAttack = GlobalEnvironment.idHardBotAttack;
				CreateBots (botProperties);								
			}
			if (botsCount == 1 && currentLife <= (initLife / 1.2f)) {
				for (int aloop = 0; aloop < botsCheckPoint.Length; aloop++) {
					if (positionPlayerCurrent.x >= 0 && botsCheckPoint [aloop].transform.position.x >= 0) { 
						if (aloop != 0) {
							xLength = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
							if (xLength < xLengthLast) {
								xLengthLast = xLength;
								positionCheckPoint = aloop;
							}
						} else {
							xLengthLast = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
							positionCheckPoint = aloop;
						}
					} else {
						if (aloop != 0) {
							xLength = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
							if (xLength < xLengthLast) {
								xLengthLast = xLength;
								positionCheckPoint = aloop;
							}
						} else {
							xLengthLast = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
							positionCheckPoint = aloop;
						}
					} 				
				}
				botProperties.sizePlayer = objBotsData.GetSizePlayer ();
				botProperties.positionPlayer = botsCheckPoint [positionCheckPoint].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				int modelWeaponRandom = objBotsData.GetRandomIdWeapon ();
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (modelWeaponRandom);
				botProperties.speedPercentage = RandomSpeedPercentage ();
				botProperties.searchTime = RandomSearchTime ();
				botProperties.modeAttack = GlobalEnvironment.idHardBotAttack;
				CreateBots (botProperties);		
				botProperties.sizePlayer = objBotsData.GetSizePlayer ();
				int posBot = RandomPositionBot ();
				botProperties.positionPlayer = botsCheckPoint [posBot].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				modelWeaponRandom = objBotsData.GetRandomIdWeapon ();
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (modelWeaponRandom);
				botProperties.speedPercentage = RandomSpeedPercentage ();
				botProperties.searchTime = RandomSearchTime ();
				botProperties.modeAttack = GlobalEnvironment.idHardBotAttack;
				CreateBots (botProperties);				
			}
		}

		void CreateRandomBot (int bots) {
			float xLength = 0.0f;
			int positionCheckPoint = 0;		
			Vector2 positionPlayerCurrent = objPlayerController.GetPositionPlayer ();
			List<float [,]> listDistancePlayer = new List<float [,]>();
			int botsCount = 0;
			botsCount = GetCountBotList (true);		
			if (botsCount > 6 || bots == 0) {
				return;
			}
			for (int aloop = 0; aloop < botsCheckPoint.Length; aloop++) {
				xLength = Mathf.Abs (botsCheckPoint [aloop].transform.position.x - positionPlayerCurrent.x);
				float floop = (float) aloop * 1.0f;
				float[,] fArray = new float [,] { {xLength, floop} };
				listDistancePlayer.Add (fArray);		
			}
			botsCount = bots;
			if (botsCount > listDistancePlayer.Count) {
				botsCount = listDistancePlayer.Count;
			}
			listDistancePlayer.Sort (SortDistance); // Sort from lowest to highest.
			for (int aloop = 0; aloop < botsCount; aloop++) {
				float [,] fArray = listDistancePlayer [aloop];
				positionCheckPoint = (int)fArray [0, 1];						
				botProperties.sizePlayer = objBotsData.GetSizePlayer ();
				botProperties.positionPlayer = botsCheckPoint [positionCheckPoint].transform.position;
				botProperties.modePosition = true;
				botProperties.objPlayer = objBotsData.GetRandomBotPrefab ();
				float distance = objBotsData.GetRandomDistance ();
				botProperties.distanceAttack = distance;
				int idRandomWeapon = objBotsData.GetRandomIdWeapon ();
				botProperties.objWeapon = objBotsData.GetWeaponPrefab (idRandomWeapon);
				botProperties.speedPercentage = RandomSpeedPercentage ();
				botProperties.searchTime = RandomSearchTime ();
				botProperties.modeAttack = GlobalEnvironment.idHardBotAttack;
				CreateBots (botProperties);		
			}						
		}

		float RandomSpeedPercentage () {
			return Random.Range (70.0f, 100.0f);
		}

		float RandomSearchTime () {
			return Random.Range (0.5f, 1.5f);
		}

		int RandomPositionBot () {
			return Random.Range (0, botsCheckPoint.Length);
		}

		void CreateWall () {
			GameObject objDesignLevel = null;
			var getObjsDesign = GameObject.FindGameObjectsWithTag (GlobalEnvironment.idDesignLevelTag);
			foreach (GameObject objDesign in getObjsDesign) {
				objDesignLevel = objDesign; 
			}
			if (!objDesignLevel) {
				return;
			}
			SpriteRenderer getDesignRenderer = objDesignLevel.GetComponent <SpriteRenderer>();
			float widthPattern = getDesignRenderer.sprite.bounds.size.x;
			float heightPattern = getDesignRenderer.sprite.bounds.size.y;
			foreach (GameObject objPlatform in wallLevel) {
				SpriteRenderer getRenderer = objPlatform.GetComponent <SpriteRenderer>();
				float xBoundsSprite = getRenderer.sprite.bounds.size.x;
				float yBoundsSprite = getRenderer.sprite.bounds.size.y;
				float widthSprite = getRenderer.sprite.rect.width;
				float heightSprite = getRenderer.sprite.rect.height;
				Vector3 positionSprite = objPlatform.gameObject.transform.position;
				Vector3 scaleSprite = objPlatform.gameObject.transform.localScale;
				float widthCurrent = (xBoundsSprite * scaleSprite.x *GlobalEnvironment.resolutionWidth) / widthPattern;
				float heightCurrent = (yBoundsSprite * scaleSprite.y * GlobalEnvironment.resolutionHeight) / heightPattern;
				objToolsGame.SetScaleSprite (objPlatform, widthCurrent, heightCurrent);
				float widthDesignPattern = widthPattern / 2;
				float widthScreenPattern = GlobalEnvironment.resolutionWidth / 2;
				if (positionSprite.x < 0) {
					widthScreenPattern *= -1.0f; 
					widthDesignPattern *= -1.0f;
				}
				float heightDesignPattern = heightPattern / 2;
				float heightScreenPattern = GlobalEnvironment.resolutionHeight / 2;
				if (positionSprite.y < 0) {
					heightScreenPattern *= -1.0f; 
					heightDesignPattern *= -1.0f;
				}
				float xPositionCurrent = (positionSprite.x * widthScreenPattern) / widthDesignPattern;
				float yPositionCurrent = (positionSprite.y * heightScreenPattern) / heightDesignPattern;
				objToolsGame.SetPositionSprite (objPlatform, xPositionCurrent, yPositionCurrent);
			}
		}

	}
}
