// Detects head bumps on the platform.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class BotHeadTrigger : MonoBehaviour 
	{

		private BotController objBotController = null;
		private ToolsGame objToolsGame;
		private AudioController objAudioController;

		void Awake () {
			gameObject.name = GlobalEnvironment.idUniqueHeadTrigger;
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
		}

		public void SetBotController (GameObject objValue) {
			objBotController = objValue.GetComponent<BotController> ();
		}

		void OnTriggerEnter2D (Collider2D otherobjs) {
			PlayerController objPlayerController = null;
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idPlayerTag)) {
				var getObj = gameObject.transform.parent.GetComponent <BotProperties> ();
				if (getObj) {
					var objProperties = otherobjs.gameObject.GetComponent <CharacterProperties> ();
					if (objProperties) {
						var objParentHead = objProperties.GetParentHead ();
						objPlayerController = objParentHead.GetComponent <PlayerController> ();
					}
					if (objPlayerController) {
						if (objPlayerController.IsPlayerFalling ()) {
							getObj.ReceiveImpactPlayer (otherobjs.transform, 1);
							if (objBotController) {
								objBotController.EnableKickHeadAnimation (true);
								objToolsGame.createDust (otherobjs.transform.position);
								objAudioController.PlaySounds (GlobalEnvironment.idKickHeadSound);
							}
						}
					}
				}
			}
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idPlatformTag)) {
				if (objBotController) {
					objBotController.StopJumpPlayer ();
				}
			}
		}

	}
}
