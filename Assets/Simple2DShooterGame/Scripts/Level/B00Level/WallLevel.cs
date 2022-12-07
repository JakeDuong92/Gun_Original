// Detection collisions on stage walls.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
    public class WallLevel : MonoBehaviour
    {
		void OnTriggerEnter2D (Collider2D otherobjs) {
			PlayerController objPlayerController = null;
            BotController objBotController = null;
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idPlayerTag)) {
				var objProperties = otherobjs.gameObject.GetComponent <CharacterProperties> ();
				if (objProperties) {
					var objParentHead = objProperties.GetParentHead ();
					objPlayerController = objParentHead.GetComponent <PlayerController> ();
				}
				if (objPlayerController) {
                    objPlayerController.EnableGame (false);
                    objPlayerController.DeathPlayer ();
				}
			}
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idBotTag)) {
				var objProperties = otherobjs.gameObject.GetComponent <BotProperties> ();
				if (objProperties) {
					var objParentHead = objProperties.GetParentHead ();
					objBotController = objParentHead.GetComponent <BotController> ();
				}
				if (objBotController) {
                    objBotController.EnableGame (false);
                    objBotController.DeathPlayer ();
				}
			}
		}
		
    }
}
