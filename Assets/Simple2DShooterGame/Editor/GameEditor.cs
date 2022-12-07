// Create menu called "Game Setup".
// Enable Google ads.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Simple2DShooter.Scripts 
{
	public class GameEditor : EditorWindow
	{
		static string titleWindow = "Game Setup";
		string titleSetup = "Set Up Advertising";
		string enableAdmobStr = "Enable Google Ads";
		static bool enableAdmob = false;
		static bool lockAdmob = false;
		// string enableUnityAdsStr = "Enable Unity Ads";
		// bool enableUnityAds = false;
		// bool lockUnityAds = false;
		string titleAbout = "About";
		static readonly string [] admobSymbol = new string[] {
			"ADMOB_ADS_ASSETS"
		};
		static readonly string [] unitySymbol = new string[] {
			"UNITY_ADS_ASSETS"
		};


		// Add menu "Game Setup".
		[MenuItem("Window/Game Setup")]
		static void Init () {
			GameEditor window = (GameEditor)EditorWindow.GetWindow(typeof(GameEditor), false, titleWindow);
			window.Show();
		}

		void OnEnable () {
			CheckSymbols ();
		}

		void OnGUI () {
			GUILayout.Label(titleSetup, EditorStyles.boldLabel);
			enableAdmob = EditorGUILayout.Toggle(enableAdmobStr, enableAdmob);
			if (enableAdmob && !lockAdmob) {
				lockAdmob = true;
				AddAdmobSymbol ();
			}
			if (!enableAdmob && lockAdmob) {
				lockAdmob = false;
				RemoveAdmobSymbol ();
			}
			/**
			enableUnityAds = EditorGUILayout.Toggle(enableUnityAdsStr, enableUnityAds);
			if (enableUnityAds && !lockUnityAds) {
				lockUnityAds = true;
				AddUnityAdsSymbol ();
			}
			if (!enableUnityAds && lockUnityAds) {
				lockUnityAds = false;
				RemoveUnityAdsSymbol ();
			}
			**/
			GUILayout.Label(" ", EditorStyles.label);
			GUILayout.Label(titleAbout, EditorStyles.boldLabel);
			GUILayout.Label("Developed by Luis Revilla", EditorStyles.label);			
			GUILayout.Label("Email support: luisrevcoding@gmail.com", EditorStyles.label);
			GUILayout.Label("Website: www.luiscoding.com", EditorStyles.label);					
		}

		// Add the ADMOB_ADS_ASSETS symbol in PlayerSettings. 
		static void AddAdmobSymbol () {
			string retSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
			List <string> settingSymbol = retSymbols.Split ( ';' ).ToList ();
			settingSymbol.AddRange (admobSymbol.Except (settingSymbol));
			PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, string.Join (";", settingSymbol.ToArray ()));
		}

		// Remove the ADMOB_ADS_ASSETS symbol in PlayerSettings.
		static void RemoveAdmobSymbol () {
			string retSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
			retSymbols = RemoveStringSymbols (retSymbols, admobSymbol [0]);
			List <string> settingSymbol = retSymbols.Split (';').ToList ();
			PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, string.Join ("", settingSymbol.ToArray ()));
		}

		// Add the UNITY_ADS_ASSETS symbol in PlayerSettings.
		static void AddUnityAdsSymbol () {
			string retSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
			List <string> settingSymbol = retSymbols.Split ( ';' ).ToList ();
			settingSymbol.AddRange (unitySymbol.Except (settingSymbol));
			PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, string.Join (";", settingSymbol.ToArray ()));
		}

		// Remove the UNITY_ADS_ASSETS symbol in PlayerSettings.
		static void RemoveUnityAdsSymbol () {
			string retSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
			retSymbols = RemoveStringSymbols (retSymbols, unitySymbol [0]);
			List <string> settingSymbol = retSymbols.Split (';').ToList ();
			PlayerSettings.SetScriptingDefineSymbolsForGroup (EditorUserBuildSettings.selectedBuildTargetGroup, string.Join ("", settingSymbol.ToArray ()));
		}

		// Delete a string.
		static string RemoveStringSymbols (string origStr, string removeStr) {
			if (origStr == null || removeStr == null) {
				return "";
			}		 
			if (origStr.Length == 0 || removeStr.Length == 0) {
				return "";
			}
			string newStr = origStr.Replace (removeStr, "");
			if (newStr.StartsWith(";")) {
				newStr = newStr.Substring(1);
			}
			if (newStr.EndsWith (";")) {
				newStr = newStr.Substring(0, newStr.Length - 1);
			}		
			return newStr;
		}

		// Check a string.
		static bool CheckStringSymbols (string origStr, string checkStr) {
			if (origStr == null || checkStr == null) {
				return false;
			}	
			if (origStr.Length == 0 || checkStr.Length == 0) {
				return false;
			}
			return origStr.Contains (checkStr);
		}

		static void CheckSymbols () {
			string retSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
			enableAdmob = false;
			if (CheckStringSymbols (retSymbols, admobSymbol [0])) {
				enableAdmob = true;
			}
			/**
			enableUnityAds = false;
			if (CheckStringSymbols (retSymbols, unitySymbol [0])) {
				enableUnityAds = true;
			}
			**/			
		}

	}
}