// Type of bullet that is being used.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class BulletWeapon : MonoBehaviour 
	{
		private int modelWeapon = 0;
		private string tagPlayer = null;
		private int countImpactMachineGun = 0;

		public void SetModelWeapon (int idWeapon) {
			modelWeapon = idWeapon;
		}

		public int GetModelWeapon () {
			return modelWeapon;
		}

		public void SetTagPlayer (string tagValue) {
			tagPlayer = tagValue;
		}

		public string GetTagPlayer () {
			return tagPlayer;
		}

		public void PlusImpactMachineGun () {
			countImpactMachineGun++;
		}

		public int GetImpactMachineGun () {
			return countImpactMachineGun;
		}

	}
}
