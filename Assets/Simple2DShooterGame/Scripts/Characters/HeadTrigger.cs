// Detects head bumps on the platform.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class HeadTrigger : MonoBehaviour 
	{
		private PlayerController objPlayerController = null;
		private ToolsGame objToolsGame;
		private AudioController objAudioController;

		void Awake () {
			gameObject.name = GlobalEnvironment.idUniqueHeadTrigger;
			objPlayerController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<PlayerController> ();
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
			objAudioController = GameObject.Find(GlobalEnvironment.idAudioController).GetComponent<AudioController> ();
		}

		void OnTriggerEnter2D(Collider2D otherobjs) {
			BotController objBotController = null;
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idBotTag)) {
				var getObj = gameObject.transform.parent.GetComponent <CharacterProperties> ();
				if (getObj) {
					var objProperties = otherobjs.gameObject.GetComponent <BotProperties> ();
					if (objProperties) {
						var objParentHead = objProperties.GetParentHead ();
						objBotController = objParentHead.GetComponent <BotController> ();
					}
					if (objBotController) {
						if (objBotController.IsPlayerFalling ()) {
							getObj.ReceiveImpactPlayer (otherobjs.transform, 1);
							if (objPlayerController) {
								objPlayerController.EnableKickHeadAnimation (true);
								objToolsGame.createDust (otherobjs.transform.position);
								objAudioController.PlaySounds (GlobalEnvironment.idKickHeadSound);
							}
						}
					}
				}
			}
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idPlatformTag)) {
				if (objPlayerController) {
						objPlayerController.StopJumpPlayer ();
				}
			}
		}

	}
}
