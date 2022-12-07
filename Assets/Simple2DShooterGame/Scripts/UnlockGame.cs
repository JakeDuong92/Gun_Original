// Unlock store characters and restore default values.
// It works only in the editor.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class UnlockGame : MonoBehaviour 
	{
		[Header("Only Editor")]
		public bool unlockCharacters = false;
		public int setScore = 2000;
		[Header("Restore values")]
		public bool resetData = false;

		void Awake () {
			if (unlockCharacters) {
				UnlockCharactersShop ();
			}
			if (resetData) {
				EraseAllData ();
			}
		}

		void UnlockCharactersShop () {
			if (setScore > 0) { 
				PlayerPrefs.SetInt (GlobalEnvironment.idScore, setScore);
			}
		}

		// Restore default values.
		void EraseAllData () {
			PlayerPrefs.SetInt (GlobalEnvironment.id00Character,1);
			PlayerPrefs.SetInt (GlobalEnvironment.id01Character, 0);
			PlayerPrefs.SetInt (GlobalEnvironment.id02Character, 0);
			PlayerPrefs.SetInt (GlobalEnvironment.id03Character, 0);
			PlayerPrefs.SetInt (GlobalEnvironment.idCurrentCharacter, 0);
			PlayerPrefs.SetInt (GlobalEnvironment.idScore, 0);
			PlayerPrefs.SetInt (GlobalEnvironment.idGameTutorial, 0);
		}
	}
}
