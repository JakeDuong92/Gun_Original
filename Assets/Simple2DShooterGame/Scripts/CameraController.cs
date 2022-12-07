// Camera movement and collisions.

using System.Collections;
using UnityEngine;

namespace Simple2DShooter.Scripts 
{
	public class CameraController : MonoBehaviour 
	{
		private PlayerController objPlayerController = null;
		private bool isStopCamera = false;
		private Vector2 positionCollisionPlayer = Vector2.zero;
		private ToolsGame objToolsGame;
		private Vector3 velocityCamera = Vector3.zero;
		public float speedSmoothCamera = 0.1f;
		private bool isShakeCamera = false;
		public float durationShake = 0.2f;
		public bool enableShake = true;
		private float constShake = 0.2f;
		private float timeShake = 0.06f;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> (); 
			objPlayerController = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<PlayerController> ();
		}

		// Use this for initialization
		void Start () {
			float widthCamera = objToolsGame.GetWidthCamera ();
			var getBoxColliders = GetComponents <BoxCollider2D>();
			getBoxColliders [0].offset = new Vector2 (-widthCamera / 2, getBoxColliders [0].offset.y);
			getBoxColliders [1].offset = new Vector2 (widthCamera / 2, getBoxColliders [1].offset.y);
			constShake = objToolsGame.GetDistancePlayer (durationShake);
		}
		
		// Update is called once per frame
		void Update () {
			if (isShakeCamera) {
				return;
			}	
			if (objPlayerController) {
				Vector2 getPositionPlayer = objPlayerController.GetPositionPlayer ();
				if (!isStopCamera) {
					Vector3 positionPlayerTmp = new Vector3 (getPositionPlayer.x, 0.0f, transform.position.z);
					transform.position = Vector3.SmoothDamp(transform.position, positionPlayerTmp, ref velocityCamera, speedSmoothCamera);
				} else {
					if (positionCollisionPlayer.x < 0) {
						if (getPositionPlayer.x > positionCollisionPlayer.x) {
							isStopCamera = false;
						}
					} else {
						if (getPositionPlayer.x < positionCollisionPlayer.x) {
							isStopCamera = false;
						}
					}
				}
			}
		}

		// Detect camera collisions on walls.
		void OnTriggerEnter2D(Collider2D otherobjs) {
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idCameraImpactTag)) {
				isStopCamera = true;
				if (objPlayerController) {
					positionCollisionPlayer = objPlayerController.GetPositionPlayer ();
				}
			}
		}

		void OnTriggerExit2D(Collider2D otherobjs) {
			if (string.Equals (otherobjs.gameObject.tag, GlobalEnvironment.idCameraImpactTag)) {
				isStopCamera = false;
			}
		}

		public void ShakeCamera () {
			if (isShakeCamera || !enableShake) {
				return;
			}			
			StartCoroutine ("CoroutineShakeCamera");
		}

		IEnumerator CoroutineShakeCamera () {
			Vector3 posInitCamera = transform.position;
			Vector3 posCamera = posInitCamera;
			isShakeCamera = true;
			posCamera.x -= constShake;
			transform.position = posCamera;
			yield return new WaitForSeconds (timeShake);
			transform.position = posInitCamera;
			yield return new WaitForSeconds (timeShake);
			posCamera = posInitCamera;
			posCamera.x += constShake;
			transform.position = posCamera;
			yield return new WaitForSeconds (timeShake);
			transform.position = posInitCamera;
			isShakeCamera = false;
		}		

	}
}
