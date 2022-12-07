// Global variables.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class GlobalEnvironment : MonoBehaviour 
	{
		public const string idGameFirstTime = "idGameFirstTime";
		public const string idGameTutorial = "idGameTutorial";
		public const int idCodefirsttime = 65367925;
		// Characters - init
		public const string id00Character = "id00Character";
		public const string id01Character = "id01Character";
		public const string id02Character = "id02Character";
		public const string id03Character = "id03Character";
		public const string id04Character = "id04Character";
		public const string id05Character = "id05Character";
		public const string id06Character = "id06Character";
		public const string idCurrentCharacter = "idCurrentCharacter";
		public const string idImageCharacter = "CharacterImage";
		public const string idBarPrice = "BarPriceImage";
		public const string idPriceCharacterText = "PriceCharacterText";
		// Characters - end
		// Screen resolution for design
		public const float resolutionWidth = 1280.0f;
		public const float resolutionHeight = 720.0f;
		// id Menu
		public const int idMainMenu = 0; // Main menu
		public const int idSettingMenu = 1; // Setting menu
		public const int idShopMenu = 2; // Shop menu
		public const int idQuitMenu = 3; // Quit menu
		public const int idLevelMenu = 4; // Level menu
		public const int idPauseMenu = 5; // Pause menu
		public const int idGameOverMenu = 6; // Game over menu
		public const int idLoadingWindow = 7; // Game over menu
		public const int idRestartAllMenu = 8; // Restart all menus
		public const int idGdprMenu = 9; // Gdpr menu
		// Main menu - init
		public const string idPlayButtonMainMenu = "PlayButton";
		public const string idShopButtonMainMenu = "ShopButton";
		public const string idSettingButtonMainMenu = "SettingButton";
		public const string idScoreMainMenu = "ScoreText";
		// Main menu - end
		// Setting menu - init
		public const string idUISettingMenu = "SettingMenu";
		public const string idContainerSettingMenu = "WindowContainer";
		public const string idBackButtonSettingMenu = "BackButton";
		public const string idSoundButtonSettingMenu = "SoundButton";
		public const string idRateUsButtonSettingMenu = "RateUsButton";
		public const string idTermsButtonSettingMenu = "TermsButton";
		public const string idEnableSound = "EnableSound"; // 0 = On sound , 1 = Off sound
		// Setting menu - end
		// Shop menu - init
		public const string idBackButtonShopMenu = "BackButton";
		public const string idPrevButtonShopMenu = "PrevButton";
		public const string idNextButtonShopMenu = "NextButton";
		public const string idBuyButtonShopMenu = "BuyButton";
		public const string idSelectButtonShopMenu = "SelectButton";
		public const string idWatchVideoButtonShopMenu = "WatchVideoButton";
		public const string idScoreShopMenu = "ScoreText";
		// Shop menu - end
		// Quit menu - init
		public const string idOkButtonQuitMenu = "OkButton";
		public const string idNoButtonQuitMenu = "NoButton";
		// Quit menu - end
		// Level menu - init
		public const string idPauseButtonLevelMenu = "PauseButton";
		public const string idBorderMotionImageLevelMenu = "BorderMotionImage";
		public const string idCircleMotionImageLevelMenu = "CircleMotionImage";
		public const string idBorderShotsImageLevelMenu = "BorderShotsImage";
		public const string idCircleShotsImageLevelMenu = "CircleShotsImage";
		public const string idScoreLevelMenu = "ScoreText";
		public const string idDeathScoreLevelMenu = "DeathScoreText";
		public const string idTimeLevelMenu = "TimeText";
		public const string idStartTimeLevelMenu = "0:00";
		public const string idAmmunitionLevelMenu = "AmmunitionText";
		public const string idWeaponLevelMenu = "WeaponImage";
		public const int idGunWeapon = 0;
		public const int idMachineGunWeapon = 1;
		public const int idBazookaWeapon = 2;
		// Level menu - end
		// Pause menu - init
		public const string idHomeButtonPauseMenu = "HomeButton";
		public const string idReloadButtonPauseMenu = "ReloadButton";
		public const string idContinueButtonPauseMenu = "ContinueButton";
		// Pause menu - end
		// Game Over menu - init
		public const string idHomeButtonGameOverMenu = "HomeButton";
		public const string idReloadButtonGameOverMenu = "ReloadButton";
		public const string idScoreGameOverMenu = "ScoreText";
		public const string idDeathScoreGameOverMenu = "DeathScoreText";
		// Game Over - end
		// View joysticks - init
		public const int idStopMotionJoystick = 0;
		public const int idStartMotionJoystick = 1;
		public const int idStopShotsJoystick = 2;
		public const int idStartShotsJoystick = 3;
		// View joysticks - end
		public const string idGdpr = "Gdpr";
		// Score - init
		public const string idScore = "Score";
		public const string idBestKillScore = "BestKillScore";
		// Score - end
		// Layers - init
		public const string idBackgroundLevelLayer = "BackgroundLevel";
		public const string idPlayerLevelLayer = "PlayerLevel";
		public const string idPlayerLayer = "Player";
		public const string idBotsLayer = "Bots";
		public const string idBotsLevelLayer = "BotsLevel";
		// Layers - end
		// Tags - init
		public const string idLevelTag = "Level";
		public const string idPlayerTag = "Player";
		public const string idGameUITag = "GameUI";
		public const string idDesignLevelTag = "DesignLevel";
		public const string idPlatformTag = "Platform";
		public const string idWeaponTag = "Weapon";
		public const string idWeaponStandTag = "WeaponStand";
		public const string idShowWeaponsTag = "ShowWeapons";
		public const string idChangeWeaponTag = "ChangeWeapon";
		public const string idBulletTag = "Bullet";
		public const string idBotTag = "Bot";
		public const string idBloodTag = "Blood";
		public const string idBackgroundTag = "Background";
		public const string idCameraImpactTag = "CameraImpact";
		public const string idMainCameraTag = "MainCamera";
		// Tags - end
		// Level game - init
		public const string idFirstBackgroundLevel = "FirstBackgroundSprite";
		public const string idSecondBackgroundLevel = "SecondBackgroundSprite";
		public const string idThirdBackgroundLevel = "ThirdBackgroundSprite";
		public const string idFourthBackgroundLevel = "FourthBackgroundSprite";
		// Level game - end
		// Scripts controllers - init
		public const string idUIController = "UIController";
		public const string idGameController = "GameController";
		public const string idAudioController = "AudioController";
		public const string idAdsController = "AdsController";
		// Scripts controllers - end
		// Animations - init
		public const string idIdleAnimation = "IdleA00Character";
		public const string idRunAnimation = "RunA00Character";
		public const string idJumpAnimation = "JumpA00Character";
		public const string idGoDownAnimation = "GoDownA00Character";
		public const string idCrouchAnimation = "CrouchA00Character";
		public const string idFrontDeathAnimation = "FrontDeathA00Character";
		public const string idFrontDeathCrouchAnimation = "FrontDeathCrouchA00Character";
		public const string idKickHeadAnimation = "KickHeadA00Character";
		public const string idKickHeadCrouchAnimation = "KickHeadCrouchA00Character";
		// Animations - end
		// Id unique - init
		public const string idUniqueCartridgeWeapon = "CartridgeWeapon-A000";
		public const string idUniqueHeadTrigger = "HeadTrigger-A001";
		public const string idUniqueGunChange = "GunChange-A002";
		public const string idUniqueMachineGunChange = "MachineGunChange-A003";
		public const string idUniqueBazookaChange = "BazookaChange-A004";
		// Id unique - end
		// Sounds - init
		public const int idGunShootSound = 0;
		public const int idMachineGunShootSound = 1;
		public const int idBazookaShootSound = 2;
		public const int idRunPlayerSound = 3;
		public const int idJumpPlayerSound = 4;
		public const int idCrawlingPlayerSound = 5;
		public const int idGunChangeSound = 6;
		public const int idMachineGunChangeSound = 7;
		public const int idBazookaChangeSound = 8;
		public const int idWithoutAmmunitionSound = 9;
		public const int idDeathPlayerSound = 10;
		public const int idKickHeadSound = 11;
		public const int idPlayerExplosionSound = 12;
		public const int idFinalTimeSound = 13;
		public const int idGiftCoinsSound = 14;
		// Sounds - end
		// Life bar - init
		public const float playerCountLife = 20; // Player lives.
		public const float botCountLife = 10; // Bot lives.
		// Life bar - end
		// Bot attack - init
		public const int idNormalBotAttack = 0;
		public const int idCrouchBotAttack = 1;
		public const int idJumpBotAttack = 2;
		public const int idEasyBotAttack = 0;
		public const int idMediumBotAttack = 1;
		public const int idHardBotAttack = 2;
		// Bot attack - end

		public struct PlayerProperties {
			public GameObject objPlayer; // Character.
			public int idPlayer; // Character id.
			public Vector2 sizePlayer; // Character width and height.
			public Vector2 positionPlayer; // Character position.
			public bool modePosition; // Character position mode.
			public GameObject objWeapon; // Weapon.
			public int idWeapon; // Weapon id.
			public float speedPercentage; // Speed percentage, values between 1 - 100.
			public float searchTime; // Bot Search Time.
			public int modeAttack; // Modes easy, medium and hard.
			public float distanceAttack; // Bot attack distance.
		}

	}
}
