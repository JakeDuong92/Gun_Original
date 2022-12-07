// Create the background sprites.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class SetupBackground : MonoBehaviour 
	{
		private ToolsGame objToolsGame;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
		}

		// Use this for initialization
		void Start () {
			BuildBackground ();
		}

		void BuildBackground () {
			GameObject objDesignLevel = null;
			var getObjsDesign = GameObject.FindGameObjectsWithTag (GlobalEnvironment.idDesignLevelTag);
			foreach (GameObject objDesign in getObjsDesign) {
				objDesignLevel = objDesign; 
			}
			if (!objDesignLevel) {
				return;
			}
			SpriteRenderer getDesignRenderer = objDesignLevel.GetComponent <SpriteRenderer>();
			float widthPattern = getDesignRenderer.sprite.bounds.size.x;
			float heightPattern = getDesignRenderer.sprite.bounds.size.y;
			var getObjsPlatform = GameObject.FindGameObjectsWithTag (GlobalEnvironment.idBackgroundTag);
			foreach (GameObject objPlatform in getObjsPlatform) {
				SpriteRenderer getRenderer = objPlatform.GetComponent <SpriteRenderer>();
				float xBoundsSprite = getRenderer.sprite.bounds.size.x;
				float yBoundsSprite = getRenderer.sprite.bounds.size.y;
				float widthSprite = getRenderer.sprite.rect.width;
				float heightSprite = getRenderer.sprite.rect.height;
				Vector3 positionSprite = objPlatform.gameObject.transform.position;
				Vector3 scaleSprite = objPlatform.gameObject.transform.localScale;
				float widthCurrent = (xBoundsSprite * scaleSprite.x *GlobalEnvironment.resolutionWidth) / widthPattern;
				float heightCurrent = (yBoundsSprite * scaleSprite.y * GlobalEnvironment.resolutionHeight) / heightPattern;
				objToolsGame.SetScaleSprite (objPlatform, widthCurrent, heightCurrent);
				float widthDesignPattern = widthPattern / 2;
				float widthScreenPattern = GlobalEnvironment.resolutionWidth / 2;
				if (positionSprite.x < 0) {
					widthScreenPattern *= -1.0f; 
					widthDesignPattern *= -1.0f;
				}
				float heightDesignPattern = heightPattern / 2;
				float heightScreenPattern = GlobalEnvironment.resolutionHeight / 2;
				if (positionSprite.y < 0) {
					heightScreenPattern *= -1.0f; 
					heightDesignPattern *= -1.0f;
				}
				float xPositionCurrent = (positionSprite.x * widthScreenPattern) / widthDesignPattern;
				float yPositionCurrent = (positionSprite.y * heightScreenPattern) / heightDesignPattern;
				objToolsGame.SetPositionSprite (objPlatform, xPositionCurrent, yPositionCurrent);
			}
		}

	}
}
