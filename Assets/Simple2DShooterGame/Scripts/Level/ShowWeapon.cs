// Enable weapons.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class ShowWeapon : MonoBehaviour 
	{
		private int modelWeapon = 0;
		private bool isEnableWeaponLevel = false;
		private GameObject objCurrentWeapon = null;

		public void SetModelWeapon (int idWeapon) {
			modelWeapon = idWeapon;
		}

		public int getModelWeapon () {
			return modelWeapon;
		}

		public void EnableWeapon (bool bStatus) {
			isEnableWeaponLevel = bStatus;
		}

		public bool IsEnableWeapon () {
			return isEnableWeaponLevel;
		}

		public void SetObjWeapon (GameObject objWeapon) {
			objCurrentWeapon = objWeapon;
		}

		public GameObject getObjWeapon () {
			return objCurrentWeapon;
		}
	}
}
