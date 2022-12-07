// Methods for creating effects, scaling and position of sprites, etc.

using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class ToolsGame : MonoBehaviour 
	{
		private float widthCamera = 0.0f;
		private float heightCamera = 0.0f;
		public GameObject levelObjects = null;
		public GameObject dustPrefab = null;
		public GameObject bloodPrefab = null;
		public float widthBlood = 10.0f;
		public float heightBlood = 10.0f;

		void Awake () {
			heightCamera = Camera.main.orthographicSize * 2.0f;
			widthCamera = (heightCamera / Screen.height) * Screen.width;
		}

		// Create sprite 2d.
		public GameObject CreateSprite (Sprite objSprite, float xPosition, float yPosition, float width, float height) {
			if (!objSprite) {
				return null;
			}
			int randomNum = Random.Range (0, 100);
			string getNewName = objSprite.name + "-" + randomNum.ToString ()+ "-"+"Sprite";
			GameObject new2DSprite = new GameObject(getNewName);
			if (levelObjects) {
				new2DSprite.transform.SetParent (levelObjects.transform);
			}
			SpriteRenderer rendererSprite = new2DSprite.AddComponent<SpriteRenderer>();
			rendererSprite.sprite = objSprite;
			SetScaleSprite (new2DSprite, width,  height);
			SetPositionSprite (new2DSprite, xPosition, yPosition);
			return new2DSprite;
		}

		// Scale a 2d sprite.
		public void SetScaleSprite (GameObject objSprite, float widthVSprite, float heightVSprite) {
			if (!objSprite || widthVSprite < 0 || heightVSprite < 0) {
				return;
			}
			float widthSprite = (widthVSprite * Screen.width) / GlobalEnvironment.resolutionWidth;
			float heightSprite = (heightVSprite * Screen.height) / GlobalEnvironment.resolutionHeight;
			SpriteRenderer getRenderer = objSprite.GetComponent <SpriteRenderer>();
			if (getRenderer) {
				objSprite.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				float heightNewCamera = (heightCamera * heightSprite) / Screen.height;
				float widthNewCamera = (widthCamera * widthSprite) / Screen.width;
				float yBounds = getRenderer.sprite.bounds.size.y;
				float xBounds = getRenderer.sprite.bounds.size.x;
				Vector3 scaleSprite = objSprite.transform.localScale;
				scaleSprite.y = heightNewCamera / yBounds;
				objSprite.transform.localScale = scaleSprite;
				scaleSprite = objSprite.transform.localScale;
				scaleSprite.x = widthNewCamera / xBounds;
				objSprite.transform.localScale = scaleSprite;
			}
		}

		// Position of the sprite on the screen and camera.
		public void SetPositionSprite (GameObject objSprite, float xPositionTmp, float yPositionTmp) {
			if (!objSprite) {
				return;
			}
			float xPosition = (xPositionTmp * Screen.width) / GlobalEnvironment.resolutionWidth;
			float yPosition = (yPositionTmp * Screen.height) / GlobalEnvironment.resolutionHeight;
			float heightNewCamera = (heightCamera * yPosition) / Screen.height;
			float widthNewCamera = (widthCamera * xPosition) / Screen.width;
			objSprite.transform.position = new Vector2 (widthNewCamera, heightNewCamera);
		}

		// Rotate the sprite.
		// GameObject objSprite: Sprite to rotate.
		// float rotationDegrees: Angle rotation.
		// bool resetRotation: true = Rotation (0,0,0)
		public void SetRotateSprite (GameObject objSprite, float rotationDegrees, bool resetRotation) {
			if (!objSprite) {
				return;
			}
			if (float.IsNaN (rotationDegrees)) {
				return;
			}
			if (resetRotation) {
				objSprite.transform.rotation = Quaternion.identity;
			} else {
				objSprite.transform.rotation = Quaternion.Euler (Vector3.forward * rotationDegrees);
			}
		}

		// Move the sprite to its new position in the world.
		// GameObject objSprite: The sprite origin.
		// GameObject objDesignLevel: Design pattern object. 
		public void SetPositionWorldPointSprite (GameObject objSprite, GameObject objDesignLevel) {
			if (!objSprite) {
				return;
			}
			SpriteRenderer getDesignRenderer = objDesignLevel.GetComponent <SpriteRenderer>();
			float widthPattern = getDesignRenderer.sprite.bounds.size.x;
			float heightPattern = getDesignRenderer.sprite.bounds.size.y;
			SpriteRenderer getRenderer = objSprite.GetComponent <SpriteRenderer>();
			float xBoundsSprite = getRenderer.sprite.bounds.size.x;
			float yBoundsSprite = getRenderer.sprite.bounds.size.y;
			float widthSprite = getRenderer.sprite.rect.width;
			float heightSprite = getRenderer.sprite.rect.height;
			Vector3 positionSprite = objSprite.gameObject.transform.position;
			Vector3 scaleSprite = objSprite.gameObject.transform.localScale;
			float widthCurrent = (xBoundsSprite * scaleSprite.x *GlobalEnvironment.resolutionWidth) / widthPattern;
			float heightCurrent = (yBoundsSprite * scaleSprite.y * GlobalEnvironment.resolutionHeight) / heightPattern;
			SetScaleSprite (objSprite, widthCurrent, heightCurrent);
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
			SetPositionSprite (objSprite, xPositionCurrent, yPositionCurrent);
		}

		// Set layer y sorting layer
		public void SetLayerSprite (GameObject objSprite, string idLayer, int sortingLayer) {
			if (!objSprite) {
				return;
			}
			SpriteRenderer getRenderer = objSprite.GetComponent <SpriteRenderer>();
			if (getRenderer) {
				getRenderer.sortingLayerName = idLayer;
				getRenderer.sortingOrder = sortingLayer;
			}
		}

		public void SetParentSprite (GameObject objSprite) {
			if (levelObjects) {
				objSprite.transform.SetParent (levelObjects.transform);
			}
		}

		public void SetTag (GameObject objValue, string tagValue) {
			if (objValue) {
				objValue.tag = tagValue;
			}
		}

		public void createDust (Vector3 positionPlayer) {
			if (!dustPrefab) {
				return;
			}
			GameObject getObj = Instantiate (dustPrefab, positionPlayer, dustPrefab.transform.rotation) as GameObject;
			if (getObj) {
				SetParentSprite (getObj);
				var getParticle = getObj.gameObject.GetComponent<ParticleSystem>();
				if (getParticle) {
					getParticle.Play ();
					Destroy (getParticle.gameObject , 3);
				}
				Destroy (getObj, 4);
			}
		}
			
		public bool getEnableRandom () {
			bool retValue = false;
			int avalue = 0;
			int bvalue = 0;
			int cvalue = 2;
			avalue = Random.Range (bvalue, cvalue);
			if (avalue == 1) {
				retValue = true;
			}
			return retValue;
		}

		public float GetWidthCamera () {
			return widthCamera;
		}

		// Check the object is visible in the camera.
		public bool IsVisibleInCamera (GameObject objPlayer) {
			bool retValue = false;
			Vector3 positionPlayer = Camera.main.WorldToViewportPoint (objPlayer.transform.position);
			retValue = positionPlayer.z > 0 && positionPlayer.x > 0 && positionPlayer.x < 1 && positionPlayer.y > 0 && positionPlayer.y < 1;
			return retValue;
		}

		// Creates blood by impact of bullets. 
		public void CreateBloodCharacters (Transform transformImpact) {
			if (bloodPrefab == null) {
				return;
			}
			int maxBlood = Random.Range (2, 6);
			for (int aloop = 0; aloop < maxBlood; aloop++) {
				float xForce = Random.Range (-60.0f, 60.0f);
				float yForce = Random.Range (-60.0f, 60.0f);
				float widthMinBlood = Random.Range (widthBlood / 2, widthBlood);
				float heightMinBlood = Random.Range (heightBlood / 2, heightBlood);
				GameObject getObj = Instantiate (bloodPrefab, transformImpact.position, bloodPrefab.transform.rotation) as GameObject;
				if (getObj) {
					SetScaleSprite (getObj, widthMinBlood, heightMinBlood);
					Rigidbody2D rigidbodyBullet = getObj.GetComponent<Rigidbody2D> ();
					if (rigidbodyBullet) {
						rigidbodyBullet.AddRelativeForce (new Vector2 (xForce, yForce)); 
					}
					SetParentSprite (getObj);
					Destroy (getObj, 1.0f);
				}
			}
		}

		// Returns the height the player jumped.
		public float GetHeightJumpPlayer (float jump) {
			float relationshipScreen = GlobalEnvironment.resolutionWidth / GlobalEnvironment.resolutionHeight;
			float relationshipScreenCurrent = (float)Screen.width / (float)Screen.height;
			float heightJumpTmp = (jump * relationshipScreenCurrent) / relationshipScreen;
			float heightJump = heightJumpTmp + (heightJumpTmp / 8.0f);
			return heightJump;
		}

		public float GetDistancePlayer (float distance) {
			float relationshipScreen = GlobalEnvironment.resolutionWidth / GlobalEnvironment.resolutionHeight;
			float relationshipScreenCurrent = (float)Screen.width / (float)Screen.height;
			return (distance * relationshipScreenCurrent) / relationshipScreen;
		}		

	}
}
