// Bot information.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class BotsData : MonoBehaviour 
	{
		public GameObject[] gameBots; // Characters available in the store.
		public GameObject[] weaponsBots; // Weapons.
		public GameObject explosionPrefab;
		public float widthPlayer = 140.0f;
		public float heightPlayer = 140.0f;
		public float xPositionPlayer = -100.0f;
		public float yPositionPlayer = -220.0f;
		public float speedPlayer = 8.0f;
		public float jumpPlayer = 3.0f;
		private int lastIndexArray = 0;

		// Jump height.
		public float GetJumpBot () {
			return jumpPlayer;
		}

		// Speed returns.
		public float GetSpeedBot () {
			return speedPlayer;
		}

		public int GetLenghtBots () {
			if (gameBots == null) {
				return 0;
			}
			return gameBots.Length;
		}

		public GameObject GetBotPrefab (int indexObj) {
			if (gameBots == null) {
				return null;
			}
			if (gameBots.Length == 0) {
				return null;
			}
			int indexArray = indexObj;
			if (indexArray > gameBots.Length) {
				indexArray = gameBots.Length;
			}
			return gameBots [indexArray];
		}

		public GameObject GetRandomBotPrefab () {
			if (gameBots == null) {
				return null;
			}
			if (gameBots.Length == 0) {
				return null;
			}
			int indexArray = Random.Range (0, (int)(gameBots.Length - 1));
			if (lastIndexArray == indexArray) {
				indexArray++;
				if (indexArray >= gameBots.Length) {
					indexArray = 0;
				}
			}
			lastIndexArray = indexArray;
			return gameBots [indexArray];
		}

		public int GetLenghtWeapons () {
			if (weaponsBots == null) {
				return 0;
			}
			return weaponsBots.Length;
		}

		public GameObject GetWeaponPrefab (int indexObj) {
			GameObject objWeaponCurrent = null;
			if (weaponsBots == null) {
				return null;
			}
			if (weaponsBots.Length != 3) {
				return null;
			}
			switch (indexObj) {
			case GlobalEnvironment.idGunWeapon:
				objWeaponCurrent = weaponsBots [0];
				break;
			case GlobalEnvironment.idMachineGunWeapon:
				objWeaponCurrent = weaponsBots [1];
				break;
			case GlobalEnvironment.idBazookaWeapon:
				objWeaponCurrent = weaponsBots [2];
				break;
			default:
				objWeaponCurrent = weaponsBots [0];
				break;
			}
			return objWeaponCurrent;
		}

		public GameObject GetPlayerExplosion () {
			return explosionPrefab;
		}

		public Vector2 GetSizePlayer () {
			return new Vector2 (widthPlayer, heightPlayer);
		}

		public int GetRandomIdWeapon () {
			int index = Random.Range (0, 3);
			int idWeaponTmp = 0;
			switch (index) {
			case 0:
				idWeaponTmp = GlobalEnvironment.idGunWeapon;
				break;
			case 1:
				idWeaponTmp = GlobalEnvironment.idMachineGunWeapon;
				break;
			case 2:
				idWeaponTmp = GlobalEnvironment.idBazookaWeapon;
				break;
			default:
				idWeaponTmp = GlobalEnvironment.idGunWeapon;
				break;
			}
			return idWeaponTmp;
		}

		public float GetRandomDistance () {
			return Random.Range (2.0f, 4.0f);
		}				
	}
}
