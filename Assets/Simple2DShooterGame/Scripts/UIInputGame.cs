// Detect screen events.

using UnityEngine.UI;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{	
	public class UIInputGame : MonoBehaviour 
	{
		private UIController objUIController;
		private Image borderMotionImage = null;
		private Image circleMotionImage = null;
		private Image borderShotsImage = null;
		private Image circleShotsImage = null;
		private float maxScreenWidth = 0.0f;
		private float maxScreenHeight = 0.0f;
		private float minHeight = 100.0f;
		private bool isMotionMoveMouse = false;
		private Vector2 positionMotion;
		private bool isShotsMoveMouse = false;
		private Vector2 positionShots;
		private float radiusBorderMotion = 50.0f;
		private float radiusBorderShots = 50.0f;
		private bool isEnableGame = false;
		private int idFingerMotion = -1;
		private int idFingerShots = -1;
		private PlayerController objPlayerController;

		void Awake () {
			objUIController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIController> ();
			objPlayerController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<PlayerController> ();
		}
		// Use this for initialization
		void Start () {
			maxScreenWidth = Screen.width / 2;
			maxScreenHeight = Screen.height - ((minHeight * Screen.height) / GlobalEnvironment.resolutionHeight);
		}
			
		// Update is called once per frame
		void Update () {
			bool isMouseDownButton = false;
			bool isMouseUpButton = false;
			bool isMouseMoveButton = false;
			bool isMouseDownShotsButton = false;
			bool isMouseUpShotsButton = false;
			bool isMouseMoveShotsButton = false;			
			if (Input.GetKeyUp (KeyCode.Escape) ) {
				if (objUIController) {
					objUIController.EscapeButton ();
				}
			}
			if (!isEnableGame) {
				return;
			}
			if (Input.touchCount == 0) {
				idFingerMotion = -1;
				idFingerShots = -1;
				if (Input.GetMouseButtonUp (0)) {
					isMouseUpButton = true;
				}
				if (Input.GetMouseButtonDown (0)) {
					isMouseDownButton = true;
				}
				if (Input.GetMouseButton (0)) {
					isMouseMoveButton = true;
				}
				touchMotionJoystick (Input.mousePosition, isMouseDownButton, isMouseUpButton, isMouseMoveButton);
				touchShotsJoystick (Input.mousePosition, isMouseDownButton, isMouseUpButton, isMouseMoveButton);
			} else {
				for ( var iLoop = 0 ; iLoop < Input.touchCount ; iLoop++ ) {
					Touch getTouchScreen = Input.GetTouch(iLoop);
					if (getTouchScreen.position.x < maxScreenWidth && getTouchScreen.position.y < maxScreenHeight) {
						if (idFingerMotion == -1) {
							idFingerMotion = getTouchScreen.fingerId;
						}
					}
					if (getTouchScreen.position.x >= maxScreenWidth && getTouchScreen.position.y < maxScreenHeight) {
						if (idFingerShots == -1) {
							idFingerShots = getTouchScreen.fingerId;
						}
					}						
					if (idFingerMotion == getTouchScreen.fingerId) {
						if (getTouchScreen.phase == TouchPhase.Began) {
							isMouseDownButton = true;
						}
						if (getTouchScreen.phase == TouchPhase.Ended) {
							isMouseUpButton = true;
						}
						if (getTouchScreen.phase == TouchPhase.Moved) {
							isMouseMoveButton = true;
						}
						if (getTouchScreen.phase == TouchPhase.Stationary) {
							isMouseMoveButton = true;
						}
						touchMotionJoystick (getTouchScreen.position, isMouseDownButton, isMouseUpButton, isMouseMoveButton);
					}
					if (idFingerShots == getTouchScreen.fingerId) {
						if (getTouchScreen.phase == TouchPhase.Began) {
							isMouseDownShotsButton = true;
						}
						if (getTouchScreen.phase == TouchPhase.Ended) {
							isMouseUpShotsButton = true;
						}
						if (getTouchScreen.phase == TouchPhase.Moved) {
							isMouseMoveShotsButton = true;
						}
						if (getTouchScreen.phase == TouchPhase.Stationary) {
							isMouseMoveShotsButton = true;
						}
						touchShotsJoystick (getTouchScreen.position, isMouseDownShotsButton, isMouseUpShotsButton, isMouseMoveShotsButton);
					}
				}
			}

		}

		public void SetJoysticks (GameObject objMenu) {
			setMotionJoystick (objMenu);
			setShotsJoystick (objMenu);
			ResetMotionJoystick ();
			ResetShotsJoystick ();
		}

		void setMotionJoystick (GameObject objMenu) {
			var objImages = objMenu.GetComponentsInChildren <Image> ();
			for (int iloop = 0; iloop < objImages.Length; iloop++) {
				string getNameImage = objImages [iloop].name;
				if (string.Equals(getNameImage, GlobalEnvironment.idBorderMotionImageLevelMenu)) {
					borderMotionImage = objImages [iloop];
				}
				if (string.Equals(getNameImage, GlobalEnvironment.idCircleMotionImageLevelMenu)) {
					circleMotionImage = objImages [iloop];
				}
			}
		}

		void touchMotionJoystick (Vector2 mousePositionScreen, bool downMouse, bool upMouse, bool moveMouse) {
			Vector2 positionMotionTmp;
			float triangleWidth,triangleRight, getTangente, getRadians;
			float xCirclePosition = 0.0f;
			float yCirclePosition = 0.0f;
			float outDegrees;
			if (upMouse) {
				isMotionMoveMouse = false;
				if (circleMotionImage) {
					circleMotionImage.transform.position = positionMotion;
				}
				ResetMotionJoystick ();
				sendEventTouchMotion (0, 0, 0);
			}
			if (mousePositionScreen.x < maxScreenWidth && mousePositionScreen.y < maxScreenHeight) {
				if (downMouse) {
					isMotionMoveMouse = true;
					if (borderMotionImage) {
						borderMotionImage.transform.position = mousePositionScreen;
					}
					if (circleMotionImage) {
						circleMotionImage.transform.position = mousePositionScreen;
					}
					positionMotion = mousePositionScreen;
					ShowJoysticks (GlobalEnvironment.idStartMotionJoystick);
				}
			}
			if (moveMouse && isMotionMoveMouse) {
				if (circleMotionImage) {
					positionMotionTmp = mousePositionScreen;
					triangleWidth  = mousePositionScreen.x - positionMotion.x;
					triangleRight = mousePositionScreen.y - positionMotion.y;
					// First quadrant
					if (triangleWidth >= 0 && triangleRight >= 0) {
						getTangente = triangleRight / triangleWidth;
						getRadians = Mathf.Atan (getTangente);
						xCirclePosition = radiusBorderMotion * Mathf.Cos (getRadians);
						yCirclePosition = radiusBorderMotion * Mathf.Sin (getRadians);
						if (mousePositionScreen.x > (positionMotion.x + xCirclePosition)) {
							positionMotionTmp.x = positionMotion.x + xCirclePosition;
						}
						if (mousePositionScreen.y > (positionMotion.y + yCirclePosition)) {
							positionMotionTmp.y = positionMotion.y + yCirclePosition;
						}
						circleMotionImage.transform.position = positionMotionTmp;
					}
					// Second quadrant
					if (triangleWidth < 0 && triangleRight >= 0) {
						getTangente = triangleRight / Mathf.Abs (triangleWidth);
						getRadians = Mathf.Atan (getTangente);
						outDegrees = (90.0f - (getRadians * Mathf.Rad2Deg)) + 90.0f;
						getRadians = outDegrees * Mathf.Deg2Rad;
						xCirclePosition = radiusBorderMotion * Mathf.Cos (getRadians);
						yCirclePosition = radiusBorderMotion * Mathf.Sin (getRadians);
						if (mousePositionScreen.x < (positionMotion.x + xCirclePosition)) {
							positionMotionTmp.x = positionMotion.x + xCirclePosition;
						}
						if (mousePositionScreen.y > (positionMotion.y + yCirclePosition)) {
							positionMotionTmp.y = positionMotion.y + yCirclePosition;
						}
						circleMotionImage.transform.position = positionMotionTmp;
					}
					// Third quadrant
					if (triangleWidth < 0 && triangleRight < 0) {
						getTangente = Mathf.Abs (triangleRight) / Mathf.Abs (triangleWidth);
						getRadians = Mathf.Atan (getTangente);
						outDegrees = (getRadians * Mathf.Rad2Deg) + 180.0f;
						getRadians = outDegrees * Mathf.Deg2Rad;
						xCirclePosition = radiusBorderMotion * Mathf.Cos (getRadians);
						yCirclePosition = radiusBorderMotion * Mathf.Sin (getRadians);
						if (mousePositionScreen.x < (positionMotion.x + xCirclePosition)) {
							positionMotionTmp.x = positionMotion.x + xCirclePosition;
						}
						if (mousePositionScreen.y < (positionMotion.y + yCirclePosition)) {
							positionMotionTmp.y = positionMotion.y + yCirclePosition;
						}
						circleMotionImage.transform.position = positionMotionTmp;
					}
					// Fourth quadrant
					if (triangleWidth >= 0 && triangleRight < 0) {
						getTangente = Mathf.Abs (triangleRight) / triangleWidth;
						getRadians = Mathf.Atan (getTangente);
						outDegrees = (90.0f - (getRadians * Mathf.Rad2Deg)) + 270.0f;
						getRadians = outDegrees * Mathf.Deg2Rad;
						xCirclePosition = radiusBorderMotion * Mathf.Cos (getRadians);
						yCirclePosition = radiusBorderMotion * Mathf.Sin (getRadians);
						if (mousePositionScreen.x > (positionMotion.x + xCirclePosition)) {
							positionMotionTmp.x = positionMotion.x + xCirclePosition;
						}
						if (mousePositionScreen.y < (positionMotion.y + yCirclePosition)) {
							positionMotionTmp.y = positionMotion.y + yCirclePosition;
						}
						circleMotionImage.transform.position = positionMotionTmp;
					}
					if (!System.Single.IsNaN (xCirclePosition) && !System.Single.IsNaN (yCirclePosition)) {
						sendEventTouchMotion (xCirclePosition, yCirclePosition, radiusBorderMotion);
					}
				}
			}
		}

		void ResetMotionJoystick () {
			float getWidthMotionImage, getWidthBorderImage;
			getWidthMotionImage = 0.0f;
			if (!circleMotionImage || !borderMotionImage) {
				return;
			}
			float proportionWidth = Screen.width / GlobalEnvironment.resolutionWidth;
			getWidthMotionImage = circleMotionImage.rectTransform.rect.width * proportionWidth;
			getWidthBorderImage = borderMotionImage.rectTransform.rect.width * proportionWidth;
			radiusBorderMotion = (getWidthBorderImage / 2) - (getWidthMotionImage / 2);
			Vector2 initPositionMotion = new Vector2((getWidthBorderImage / 2) + 50.0f, (getWidthBorderImage / 2) + 100.0f);
			circleMotionImage.transform.position = initPositionMotion;
			borderMotionImage.transform.position = initPositionMotion;
			ShowJoysticks (GlobalEnvironment.idStopMotionJoystick); 
		}

		void setShotsJoystick (GameObject objMenu) {
			var objImages = objMenu.GetComponentsInChildren <Image> ();
			for (int iloop = 0; iloop < objImages.Length; iloop++) {
				string getNameImage = objImages [iloop].name;
				if (string.Equals(getNameImage, GlobalEnvironment.idBorderShotsImageLevelMenu)) {
					borderShotsImage = objImages [iloop];
				}
				if (string.Equals(getNameImage, GlobalEnvironment.idCircleShotsImageLevelMenu)) {
					circleShotsImage = objImages [iloop];
				}
			}
		}

		void touchShotsJoystick (Vector2 mousePositionScreen, bool downMouse, bool upMouse, bool moveMouse) {
			Vector2 positionShotsTmp;
			float triangleWidth,triangleRight, getTangente, getRadians;
			float xCirclePosition = 0.0f;
			float yCirclePosition = 0.0f;
			float outDegrees;
			if (upMouse) {
				isShotsMoveMouse = false;
				if (circleShotsImage) {
					circleShotsImage.transform.position = positionShots;
				}
				ResetShotsJoystick ();
				sendEventTouchShots (0, 0, 0);
			}
			if (mousePositionScreen.x >= maxScreenWidth && mousePositionScreen.y < maxScreenHeight) {
				if (downMouse) {
					isShotsMoveMouse = true;
					if (borderShotsImage) {
						borderShotsImage.transform.position = mousePositionScreen;
					}
					if (circleShotsImage) {
						circleShotsImage.transform.position = mousePositionScreen;
					}
					positionShots = mousePositionScreen;
					ShowJoysticks (GlobalEnvironment.idStartShotsJoystick);
				}
			}
			if (moveMouse && isShotsMoveMouse) {
				if (circleShotsImage) {
					positionShotsTmp = mousePositionScreen;
					triangleWidth  = mousePositionScreen.x - positionShots.x;
					triangleRight = mousePositionScreen.y - positionShots.y;
					// First quadrant
					if (triangleWidth >= 0 && triangleRight >= 0) {
						getTangente = triangleRight / triangleWidth;
						getRadians = Mathf.Atan (getTangente);
						xCirclePosition = radiusBorderShots * Mathf.Cos (getRadians);
						yCirclePosition = radiusBorderShots * Mathf.Sin (getRadians);
						if (mousePositionScreen.x > (positionShots.x + xCirclePosition)) {
							positionShotsTmp.x = positionShots.x + xCirclePosition;
						}
						if (mousePositionScreen.y > (positionShots.y + yCirclePosition)) {
							positionShotsTmp.y = positionShots.y + yCirclePosition;
						}
						circleShotsImage.transform.position = positionShotsTmp;
					}
					// Second quadrant
					if (triangleWidth < 0 && triangleRight >= 0) {
						getTangente = triangleRight / Mathf.Abs (triangleWidth);
						getRadians = Mathf.Atan (getTangente);
						outDegrees = (90.0f - (getRadians * Mathf.Rad2Deg)) + 90.0f;
						getRadians = outDegrees * Mathf.Deg2Rad;
						xCirclePosition = radiusBorderShots * Mathf.Cos (getRadians);
						yCirclePosition = radiusBorderShots * Mathf.Sin (getRadians);
						if (mousePositionScreen.x < (positionShots.x + xCirclePosition)) {
							positionShotsTmp.x = positionShots.x + xCirclePosition;
						}
						if (mousePositionScreen.y > (positionShots.y + yCirclePosition)) {
							positionShotsTmp.y = positionShots.y + yCirclePosition;
						}
						circleShotsImage.transform.position = positionShotsTmp;
					}
					// Third quadrant
					if (triangleWidth < 0 && triangleRight < 0) {
						getTangente = Mathf.Abs (triangleRight) / Mathf.Abs (triangleWidth);
						getRadians = Mathf.Atan (getTangente);
						outDegrees = (getRadians * Mathf.Rad2Deg) + 180.0f;
						getRadians = outDegrees * Mathf.Deg2Rad;
						xCirclePosition = radiusBorderShots * Mathf.Cos (getRadians);
						yCirclePosition = radiusBorderShots * Mathf.Sin (getRadians);
						if (mousePositionScreen.x < (positionShots.x + xCirclePosition)) {
							positionShotsTmp.x = positionShots.x + xCirclePosition;
						}
						if (mousePositionScreen.y < (positionShots.y + yCirclePosition)) {
							positionShotsTmp.y = positionShots.y + yCirclePosition;
						}
						circleShotsImage.transform.position = positionShotsTmp;
					}
					// Fourth quadrant
					if (triangleWidth >= 0 && triangleRight < 0) {
						getTangente = Mathf.Abs (triangleRight) / triangleWidth;
						getRadians = Mathf.Atan (getTangente);
						outDegrees = (90.0f - (getRadians * Mathf.Rad2Deg)) + 270.0f;
						getRadians = outDegrees * Mathf.Deg2Rad;
						xCirclePosition = radiusBorderShots * Mathf.Cos (getRadians);
						yCirclePosition = radiusBorderShots * Mathf.Sin (getRadians);
						if (mousePositionScreen.x > (positionShots.x + xCirclePosition)) {
							positionShotsTmp.x = positionShots.x + xCirclePosition;
						}
						if (mousePositionScreen.y < (positionShots.y + yCirclePosition)) {
							positionShotsTmp.y = positionShots.y + yCirclePosition;
						}
						circleShotsImage.transform.position = positionShotsTmp;
					}
					if (!System.Single.IsNaN (xCirclePosition) && !System.Single.IsNaN (yCirclePosition)) {
						sendEventTouchShots (xCirclePosition, yCirclePosition, radiusBorderShots);
					}
				}
			}
		}

		void ResetShotsJoystick () {
			float getWidthShotsImage, getWidthBorderImage;
			getWidthShotsImage = 0.0f;
			if (!circleShotsImage || !borderShotsImage) {
				return;
			}
			float proportionWidth = Screen.width / GlobalEnvironment.resolutionWidth;
			getWidthShotsImage = circleShotsImage.rectTransform.rect.width * proportionWidth;
			getWidthBorderImage = borderShotsImage.rectTransform.rect.width * proportionWidth;
			radiusBorderShots = (getWidthBorderImage / 2) - (getWidthShotsImage / 2);
			Vector2 initPositionShots = new Vector2(Screen.width - ((getWidthBorderImage / 2) + 50.0f), (getWidthBorderImage / 2) + 100.0f);
			circleShotsImage.transform.position = initPositionShots;
			borderShotsImage.transform.position = initPositionShots;
			ShowJoysticks (GlobalEnvironment.idStopShotsJoystick);
		}

		// Showing the joysticks on the screen.
		void ShowJoysticks (int idAction) {
			Color colorBorderMotionImage;
			Color colorCircleMotionImage;
			Color colorBorderShotsImage;
			Color colorCircleShotsImage;
			switch (idAction) {
			case GlobalEnvironment.idStopMotionJoystick:
				colorBorderMotionImage = borderMotionImage.color;
				colorBorderMotionImage.a = 0.0f;
				borderMotionImage.color = colorBorderMotionImage;
				colorCircleMotionImage = circleMotionImage.color;
				colorCircleMotionImage.a = 0.3f;
				circleMotionImage.color = colorCircleMotionImage;
				break;
			case GlobalEnvironment.idStartMotionJoystick:
				colorBorderMotionImage = borderMotionImage.color;
				colorBorderMotionImage.a = 0.5f;
				borderMotionImage.color = colorBorderMotionImage;
				colorCircleMotionImage = circleMotionImage.color;
				colorCircleMotionImage.a = 0.5f;
				circleMotionImage.color = colorCircleMotionImage;
				break;
			case GlobalEnvironment.idStopShotsJoystick:
				colorBorderShotsImage = borderShotsImage.color;
				colorBorderShotsImage.a = 0.0f;
				borderShotsImage.color = colorBorderShotsImage;
				colorCircleShotsImage = circleShotsImage.color;
				colorCircleShotsImage.a = 0.3f;
				circleShotsImage.color = colorCircleShotsImage;
				break;
			case GlobalEnvironment.idStartShotsJoystick:
				colorBorderShotsImage = borderShotsImage.color;
				colorBorderShotsImage.a = 0.5f;
				borderShotsImage.color = colorBorderShotsImage;
				colorCircleShotsImage = circleShotsImage.color;
				colorCircleShotsImage.a = 0.5f;
				circleShotsImage.color = colorCircleShotsImage;
				break;
			default:
				break;
			}
		}

		// Send the events of the Joystick movement to the player.
		// float xPosition = X-axis offset, values - / +.
		// float yPosition = Y-axis offset, values - / +.
		// float maxDistance = Maximum distance of the x, y axes. 
		void sendEventTouchMotion (float xPosition, float yPosition, float maxDistance) {
			objPlayerController.sendEventTouchMotion (xPosition, yPosition, maxDistance);
		}

		// Send the events of the Joystick Shots to the player.
		// float xPosition = X-axis offset, values - / +.
		// float yPosition = Y-axis offset, values - / +.
		// float maxDistance = Maximum distance of the x, y axes. 
		void sendEventTouchShots (float xPosition, float yPosition, float maxDistance) {
			objPlayerController.sendEventTouchShots (xPosition, yPosition, maxDistance);
		}

		// Enable the game.
		public void EnableGame (bool bGame) {
			isEnableGame = bGame;
			if (isEnableGame) {
				ResetMotionJoystick ();
				ResetShotsJoystick ();
			}
		}
	}
}
