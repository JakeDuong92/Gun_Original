// Character shop.

using UnityEngine.UI;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{	
	public class UIShopController : MonoBehaviour 
	{
		public Sprite[] shopCharactersSprite;
		public int[] priceCharacters;
		private Image objCharacterImage = null;
		private int countCharacter = 0;
		private Button objPrevButton = null;
		private Button objNextButton = null;
		private Button objBuyButton = null;
		private Button objSelectButton = null;
		private Button objWatchVideoButton = null;
		private Image barPriceImage = null;
		private Text priceCharacterText = null;
		private UIController objUIController;
		public int giftCoins = 20;
		private AudioController objAudioController;	
		GoogleAdsGame googleAdsGame;


		void Awake () {
			objUIController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIController> ();
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();		
			googleAdsGame = GameObject.Find(GlobalEnvironment.idAdsController).GetComponent<GoogleAdsGame> ();
		}

		public void initShop (GameObject objMenu) {
			var objImages = objMenu.GetComponentsInChildren <Image> ();
			for (int iloop = 0; iloop < objImages.Length; iloop++) {
				string getNameImage = objImages [iloop].name;
				if (string.Equals(getNameImage, GlobalEnvironment.idImageCharacter)) {
					objCharacterImage = objImages [iloop];
				}
			}
			if (objCharacterImage) {
				int geIdCharacter = PlayerPrefs.GetInt(GlobalEnvironment.idCurrentCharacter,0);
				if ( (geIdCharacter + 1) > objImages.Length) {
					geIdCharacter = 0;
				}
				if (shopCharactersSprite.Length > 0) {
					objCharacterImage.sprite = shopCharactersSprite [geIdCharacter];
				}
			}
			var objButtons = objMenu.GetComponentsInChildren <Button> ();
			for (int iloop = 0; iloop < objButtons.Length; iloop++) {
				string getNameButton = objButtons [iloop].name;
				if (string.Equals(getNameButton, GlobalEnvironment.idPrevButtonShopMenu)) {
					objPrevButton = objButtons [iloop];
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idNextButtonShopMenu)) {
					objNextButton = objButtons [iloop];
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idBuyButtonShopMenu)) {
					objBuyButton = objButtons [iloop];
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idSelectButtonShopMenu)) {
					objSelectButton = objButtons [iloop];
				}
				if (string.Equals(getNameButton, GlobalEnvironment.idWatchVideoButtonShopMenu)) {
					objWatchVideoButton = objButtons [iloop];
				}				
			}
			if (objPrevButton) {
				objPrevButton.gameObject.SetActive (false);
			}
			if (shopCharactersSprite.Length == 0 || shopCharactersSprite.Length == 1) {
				if (objPrevButton) {
					objPrevButton.gameObject.SetActive (false);
				}
				if (objNextButton) {
					objNextButton.gameObject.SetActive (false);
				}
				if (objBuyButton) {
					objBuyButton.gameObject.SetActive (false);
				}
			}
			if (objSelectButton) {
				objSelectButton.gameObject.SetActive (false);
			}
			EnableWatchVideoButton (false);			
			var objBarPriceImages = objMenu.GetComponentsInChildren <Image> ();
			for (int iloop = 0; iloop < objBarPriceImages.Length; iloop++) {
				string getNameBar = objBarPriceImages [iloop].name;
				if (string.Equals(getNameBar, GlobalEnvironment.idBarPrice)) {
					barPriceImage = objBarPriceImages [iloop];
				}
			}
			var objPriceTexts = objMenu.GetComponentsInChildren <Text> ();
			for (int iloop = 0; iloop < objPriceTexts.Length; iloop++) {
				string getNamePrice = objPriceTexts [iloop].name;
				if (string.Equals(getNamePrice, GlobalEnvironment.idPriceCharacterText)) {
					priceCharacterText = objPriceTexts [iloop];
				}
			}
			SetCurrentCharacter ();
		}

		public void PrevButton () {
			if (shopCharactersSprite.Length == 0) {
				return;
			}
			countCharacter--;
			if (countCharacter <= 0) {
				countCharacter = 0;
				if (objPrevButton) {
					objPrevButton.gameObject.SetActive (false);
				}
			}
			if (objCharacterImage) {
				objCharacterImage.sprite = shopCharactersSprite [countCharacter];
			}
			if (objNextButton) {
				objNextButton.gameObject.SetActive (true);
			}
			SetPriceCharacter (countCharacter);
		}

		public void NextButton () {
			if (shopCharactersSprite.Length == 0) {
				return;
			}
			countCharacter++;
			if (countCharacter >= (shopCharactersSprite.Length - 1)) {
				countCharacter = shopCharactersSprite.Length - 1;
				if (countCharacter < 0) {
					countCharacter = 0;
				}
				if (objNextButton) {
					objNextButton.gameObject.SetActive (false);
				}
			}
			if (objCharacterImage) {
				objCharacterImage.sprite = shopCharactersSprite [countCharacter];
			}
			if (objPrevButton) {
				objPrevButton.gameObject.SetActive (true);
			}
			SetPriceCharacter (countCharacter);
		}

		// Put the price on the characters.
		void SetPriceCharacter (int idCharacter) {
			int getEnablePlayer = 0;
			if (priceCharacters.Length == 0 || (idCharacter + 1) > priceCharacters.Length) {
				return;
			}
			int getCurrentCharacter = PlayerPrefs.GetInt (GlobalEnvironment.idCurrentCharacter, 0);
			switch (idCharacter) {
			case 0:
				getEnablePlayer = PlayerPrefs.GetInt(GlobalEnvironment.id00Character,1);
				break;
			case 1:
				getEnablePlayer = PlayerPrefs.GetInt(GlobalEnvironment.id01Character,0);
				break;
			case 2:
				getEnablePlayer = PlayerPrefs.GetInt(GlobalEnvironment.id02Character,0);
				break;
			case 3:
				getEnablePlayer = PlayerPrefs.GetInt(GlobalEnvironment.id03Character,0);
				break;
			case 4:
				getEnablePlayer = PlayerPrefs.GetInt(GlobalEnvironment.id04Character,0);
				break;	
			case 5:
				getEnablePlayer = PlayerPrefs.GetInt(GlobalEnvironment.id05Character,0);
				break;	
			case 6:
				getEnablePlayer = PlayerPrefs.GetInt(GlobalEnvironment.id06Character,0);
				break;											
			default:
				break;
			}
			if (barPriceImage) {
				if (getEnablePlayer == 1) {
					barPriceImage.gameObject.SetActive (false);
					if (objBuyButton) {
						objBuyButton.gameObject.SetActive (false);
					}
					if (objSelectButton && getCurrentCharacter != idCharacter) {
						objSelectButton.gameObject.SetActive (true);
					}
				} else {
					if (priceCharacterText) {
						priceCharacterText.text = priceCharacters [idCharacter].ToString ();
					}
					barPriceImage.gameObject.SetActive (true);
					int getScore = PlayerPrefs.GetInt (GlobalEnvironment.idScore, 0);
					if (getScore >= priceCharacters [idCharacter]) {
						if (objBuyButton) {
							objBuyButton.gameObject.SetActive (true);
							objBuyButton.interactable = true;
						}
					} else {
						if (objBuyButton) {
							objBuyButton.gameObject.SetActive (true);
							objBuyButton.interactable = false;
						}
					}
					if (objSelectButton) {
						objSelectButton.gameObject.SetActive (false);
					}
				}
			}
		}

		void SetCurrentCharacter () {
			if (shopCharactersSprite.Length == 0) {
				return;
			}
			countCharacter = PlayerPrefs.GetInt (GlobalEnvironment.idCurrentCharacter, 0);
			if (objPrevButton) {
				objPrevButton.gameObject.SetActive (true);
			}
			if (countCharacter <= 0) {
				countCharacter = 0;
				if (objPrevButton) {
					objPrevButton.gameObject.SetActive (false);
				}
			}
			if (objNextButton) {
				objNextButton.gameObject.SetActive (true);
			}
			if (countCharacter >= (shopCharactersSprite.Length - 1)) {
				countCharacter = shopCharactersSprite.Length - 1;
				if (countCharacter < 0) {
					countCharacter = 0;
				}
				if (objNextButton) {
					objNextButton.gameObject.SetActive (false);
				}
			}
			if (objCharacterImage) {
				objCharacterImage.sprite = shopCharactersSprite [countCharacter];
			}
			SetPriceCharacter (countCharacter);
		}

		public void ReloadPriceCharacter () {
			SetPriceCharacter (countCharacter);
		}

		// Buy characters.
		public void BuyButton () {
			bool isBuy = false;
			if (shopCharactersSprite.Length == 0) {
				return;
			}
			switch (countCharacter) {
			case 0:
				PlayerPrefs.SetInt(GlobalEnvironment.id00Character, 1);
				isBuy = true;
				break;
			case 1:
				PlayerPrefs.SetInt(GlobalEnvironment.id01Character, 1);
				isBuy = true;
				break;
			case 2:
				PlayerPrefs.SetInt(GlobalEnvironment.id02Character, 1);
				isBuy = true;
				break;
			case 3:
				PlayerPrefs.SetInt(GlobalEnvironment.id03Character, 1);
				isBuy = true;
				break;
			case 4:
				PlayerPrefs.SetInt(GlobalEnvironment.id04Character, 1);
				isBuy = true;
				break;	
			case 5:
				PlayerPrefs.SetInt(GlobalEnvironment.id05Character, 1);
				isBuy = true;
				break;
			case 6:
				PlayerPrefs.SetInt(GlobalEnvironment.id06Character, 1);
				isBuy = true;
				break;												
			default:
				break;
			}
			int getPrice = priceCharacters [countCharacter];
			int getScore = PlayerPrefs.GetInt (GlobalEnvironment.idScore, 0);
			if (isBuy) {
				getScore -= getPrice;
				PlayerPrefs.SetInt (GlobalEnvironment.idScore, getScore);
				if (objBuyButton) {
					objBuyButton.gameObject.SetActive (false);
				}
				if (barPriceImage) {
					barPriceImage.gameObject.SetActive (false);
				}
				if (objSelectButton) {
					objSelectButton.gameObject.SetActive (true);
				}
				objUIController.UpdateScore ();
			}
		}

		public void SelectButton () {
			if (shopCharactersSprite.Length == 0) {
				return;
			}
			if (objSelectButton) {
				objSelectButton.gameObject.SetActive (false);
			}
			PlayerPrefs.SetInt (GlobalEnvironment.idCurrentCharacter, countCharacter);
		}

		public void WatchVideoButton () {
			googleAdsGame.ShowRewardVideoAdAdmob ();
		}

		public void EnableWatchVideoButton (bool enableButton) {
			if (objWatchVideoButton) {
				objWatchVideoButton.gameObject.SetActive (enableButton);
			}	
		}

		// Return coins after watching videos.
		public void BuyCoinsFromVideo () {
			int getScore = PlayerPrefs.GetInt (GlobalEnvironment.idScore, 0);
			getScore += giftCoins;
			PlayerPrefs.SetInt (GlobalEnvironment.idScore, getScore);
			objUIController.UpdateScore ();
			objAudioController.PlaySounds (GlobalEnvironment.idGiftCoinsSound);
			EnableWatchVideoButton (false);
		}
	}
}
