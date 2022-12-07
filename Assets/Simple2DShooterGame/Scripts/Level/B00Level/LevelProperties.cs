// Level properties.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{	
	public class LevelProperties : MonoBehaviour 
	{
		
		private B00Level objGeneral = null;

		void Awake () {
			objGeneral = GetComponent <B00Level> ();
		}

		public void EnableGame (bool bGame) {
			if (objGeneral) {
				objGeneral.EnableGame (bGame);
			}
		}

		public void DeathPlayer () {
			if (objGeneral) {
				objGeneral.DeathPlayer ();
			}
		}

		public void ChangeWeapon (int id) {
			if (objGeneral) {
				objGeneral.ChangeWeapon (id);
			}
		}

	}
}
