// Google Mobile ads.
// Banner, interstitial and rewarded video.

using System;
using UnityEngine;
#if ADMOB_ADS_ASSETS
using GoogleMobileAds.Api;
#endif

namespace Simple2DShooter.Scripts 
{
    public class GoogleAdsGame : MonoBehaviour
    {
		#if ADMOB_ADS_ASSETS
		private InterstitialAd interstitial=null;
		#endif
		private bool  enableAdmobAds = false;
		[Header("Google Ads")]		
		public string AdmobBannerId = null; // Banner id.
		public string AdmobInterstialId = null; // Interstial id.
		[Tooltip("Counter to enable Interstitial ad.")]		
		public int countInterstial = 1;
		private int countinterstialtmp = 1;
		public string AdmobRewardedId = null; // Rewarded video id.
		[Tooltip("Counter to enable video ad.")]
		public int adsVideoCounter = 1; // Counter to enable video ad. - Contador para habilitar el ad video.
		int adsVideoCounterTmp = 0;
		[Tooltip("Maximum number of ad videos in a game match.")]
		public int maxAdVideos = 10; // Maximum number of ad videos in a game match. - Cantidad maxima de videos de publicidad que se mostrara en una partida.
		int maxAdVideosCount = 0;			
		bool AdmobTestDevice = false; 
		string AdmobDeviceId = null;
		#if ADMOB_ADS_ASSETS
		bool brewardvideo = false;
		#endif
		private float timeprotection = 5.0f;
		private float timerest = 0.0f;
		float timeEnableButton = 0.0f;
		float timeDisableButton = 0.0f;
		private ToolsGame objToolsGame;
		private UIShopController objUIShopController;
		// Check if the ad video is loaded. true = yes, the ad video is loaded - false = no, the video ad is not loaded.
		// Determina el estado de la carga del video ads, true = si cargo el video ads, false = no cargo el video ads.
		bool rewardVideoIsLoaded = false; 			
		#if ADMOB_ADS_ASSETS
		private BannerView bannerView = null;
		private RewardedAd rewardedAd = null;		
		#endif
		bool enableAdVideo = false;

		void Awake () {
			objToolsGame = GameObject.Find(GlobalEnvironment.idGameController).GetComponent<ToolsGame> ();
			objUIShopController = GameObject.Find(GlobalEnvironment.idUIController).GetComponent<UIShopController> ();		
		}

		// Use this for initialization
		void Start () {
			#if ADMOB_ADS_ASSETS
			enableAdmobAds = true;
			#endif				
			timerest = timeprotection;
			timeEnableButton = timeprotection;
			timeDisableButton = timeprotection;
			maxAdVideosCount = 0;
			adsVideoCounterTmp = 0;
			enableAdVideo = false;
			InitAdMob ();
			CreateBannerAdmob ();
			CreateInterstialAdmob();
			CreateRewardVideoAdAdmob ();
		}

		void OnGUI () {
			#if ADMOB_ADS_ASSETS
			if (brewardvideo == true) {
				timerest -= 1.0f;
				if (timerest < 0) {
					brewardvideo = false;
					if (objUIShopController != null) {
						objUIShopController.BuyCoinsFromVideo ();
					}
				}
			}
			#endif
		}

		void InitAdMob () {
			if (!enableAdmobAds) {
				return;
			}
			#if ADMOB_ADS_ASSETS
			MobileAds.Initialize(initStatus => { });
			#endif

		}

		// Create banner in the top position
		void CreateBannerAdmob () {
			if (!enableAdmobAds) {
				return;
			}			
			if (AdmobBannerId == null) {
				return;
			}
			if (string.IsNullOrEmpty (AdmobBannerId)) {
				return;
			}
			#if ADMOB_ADS_ASSETS && (UNITY_ANDROID || UNITY_IOS)
				AdRequest request = null;
				// Create a banner at the bottom of the screen.
				bannerView = new BannerView(AdmobBannerId, AdSize.Banner, AdPosition.Bottom);
				if (checkTestDevice () == false) {
					// Create an empty ad request.
					request = new AdRequest.Builder().Build();
				} else {
					// Device test
					// request = new AdRequest.Builder().AddTestDevice(AdmobDeviceId).Build();
				}
				// Load the banner with the request.
				if (request != null) {
					bannerView.LoadAd(request);
				}
			#endif
		}

		public void EnableBannerAdmob (bool enableBanner) {
			if (!enableAdmobAds) {
				return;
			}			
			if (AdmobBannerId == null) {
				return;
			}
			if (string.IsNullOrEmpty (AdmobBannerId)) {
				return;
			}
			#if ADMOB_ADS_ASSETS			
			if (enableBanner) {
				if (bannerView != null) {
					bannerView.Show ();
				}
			} else {
				if (bannerView != null) {
					bannerView.Hide ();
				}				
			}
			#endif
		}

		// Create interstitial
		void CreateInterstialAdmob () {
			if (!enableAdmobAds) {
				return;
			}			
			if (AdmobInterstialId == null) {
				return;
			}
			if (string.IsNullOrEmpty (AdmobInterstialId)) {
				return;
			}
			#if ADMOB_ADS_ASSETS && (UNITY_ANDROID || UNITY_IOS)
				AdRequest request = null;
				// Initialize an InterstitialAd.
				interstitial = new InterstitialAd(AdmobInterstialId);				
				interstitial.OnAdClosed += interstitialClosed;
				if (checkTestDevice () == false) {
					// Create an empty ad request.
					request = new AdRequest.Builder().Build();
				} else {
					// Device test
					// request = new AdRequest.Builder().AddTestDevice(AdmobDeviceId).Build();
				}
				// Load the interstitial with the request.
				if (request != null) {
					interstitial.LoadAd(request);
				}
			#endif
		}

		// Show the interstitial
		public void ShowInterstialAdmob () {
			if (!enableAdmobAds) {
				return;
			}			
			if (AdmobInterstialId == null) {
				return;
			}
			if (string.IsNullOrEmpty (AdmobInterstialId)) {
				return;
			}			
			if (countinterstialtmp >= countInterstial) {
				countinterstialtmp = 1;
				#if ADMOB_ADS_ASSETS && (UNITY_ANDROID || UNITY_IOS)
					if (interstitial != null) {
						if (interstitial.IsLoaded()) {
							interstitial.Show();
						}
					}
				#endif
			} else {
				countinterstialtmp++;
			}
		}

		// Close and reload the interstitial
		public void interstitialClosed (object sender, EventArgs args) {
			#if ADMOB_ADS_ASSETS && (UNITY_ANDROID || UNITY_IOS)
				AdRequest request = null;
				if (checkTestDevice () == false) {
					request = new AdRequest.Builder().Build();
				} else {
					// Device test
					// request = new AdRequest.Builder().AddTestDevice(AdmobDeviceId).Build();
				}
				// Load the interstitial with the request.
				if (request != null) {
					interstitial.LoadAd(request);
				}
			#endif
		}

		bool checkTestDevice () {
			if (AdmobTestDevice == false) {
				return false;
			}
			if (AdmobDeviceId == null) {
				return false;
			}
			if (string.IsNullOrEmpty (AdmobDeviceId)) {
				return false;
			}
			return true;
		}

		void CreateRewardVideoAdAdmob () {
			#if ADMOB_ADS_ASSETS
			if (!enableAdmobAds) {
				return;
			}			
			if (AdmobRewardedId == null) {
				return;
			}
			if (string.IsNullOrEmpty (AdmobRewardedId)) {
				return;
			}
			if (rewardedAd != null) {
				if (rewardedAd.IsLoaded ()) {
					return;
				}
			}
			rewardVideoIsLoaded = false;
        	rewardedAd = new RewardedAd(AdmobRewardedId);
        	// Called when an ad request has successfully loaded.
        	rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        	// Called when an ad request failed to load.
        	rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        	// Called when an ad is shown.
        	rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        	// Called when an ad request failed to show.
        	rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        	// Called when the user should be rewarded for interacting with the ad.
        	rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        	// Called when the ad is closed.
        	rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        	// Create an empty ad request.
        	AdRequest request = new AdRequest.Builder().Build();
        	// Load the rewarded ad with the request.
        	rewardedAd.LoadAd(request);
			#endif
		}

		#if ADMOB_ADS_ASSETS
		public void HandleRewardedAdLoaded(object sender, EventArgs args) {
			timeEnableButton = timeprotection;
			rewardVideoIsLoaded = true;
		}

		
		public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
			timeDisableButton = timeprotection;			
			rewardVideoIsLoaded = false;
		}
		
		public void HandleRewardedAdOpening(object sender, EventArgs args) {

		}

		public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args) {

		}

		public void HandleRewardedAdClosed(object sender, EventArgs args) {
			enableAdVideo = false;
			CreateRewardVideoAdAdmob ();
		}

		public void HandleUserEarnedReward(object sender, Reward args) {
			string type = args.Type;
			double amount = args.Amount;
			timerest = timeprotection;
			brewardvideo = true;
		}
		#endif

		public void ShowRewardVideoAdAdmob () {
			if (!enableAdmobAds) {
				return;
			}			
			if (AdmobRewardedId == null) {
				return;
			}
			if (string.IsNullOrEmpty (AdmobRewardedId)) {
				return;
			}
			#if ADMOB_ADS_ASSETS
			if (rewardedAd.IsLoaded()) {
				rewardedAd.Show();
			}
			#endif
		}

		// Enable video ad.
		public void VideoAdActivation () {
			if (rewardVideoIsLoaded && maxAdVideosCount < maxAdVideos && !enableAdVideo) {
				if (adsVideoCounterTmp == 0) {
					enableAdVideo = true;
					maxAdVideosCount++;
				}
				adsVideoCounterTmp++;
				if (adsVideoCounterTmp > adsVideoCounter) {
					adsVideoCounterTmp = 0;
				}
			}
		}

		// Check that reward video is available.
		// Return: true = yes the video ad is available, false = the ad video is not available. 
		public bool CheckRewardVideoIsAvailable () {
			bool status = false;
			if (maxAdVideosCount < maxAdVideos) {
				return enableAdVideo;
			}
			return status;
		}
    }
}