using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace SingularisVR.ReportXR {

    [System.Serializable]
    public class MoodleUser {
        public string sitename;
        public string username;
        public string firstname;
        public string lastname;
        public string fullname;
        public string lang;
        public int userid;
        public string userpictureurl;
        public string release;
        public string version;
        public string userprivateaccesskey;
    }

    [System.Serializable]
    public class MoodleSco {
        public int id;
        public string title;
    }

    [System.Serializable]
    public class MoodleScorm {    
        public int id;
        public string name;
        public int attemptCount;
        public MoodleSco[] scoes;
    }

    [System.Serializable]
    public class MoodleLoginSuccess {
        public string token;
        public MoodleUser user;
        public MoodleScorm[] scorms;
    }



    [System.Serializable]
    public class  MoodleErrorDetails {
        public string error;
        public string errorcode;
        public string stacktrace;
        public string debuginfo;
        public string reproductionlink;
    }

    [System.Serializable]
    public class MoodleLoginError {
        public string error;
        public MoodleErrorDetails details;
    }


    public class ReportXR {
        private static bool isInitialized = false;
        private static AndroidJavaClass unityClass;
        private static AndroidJavaObject unityActivity;
        private static AndroidJavaObject rXRLib;


        public static MoodleLoginSuccess loginSuccess;
        public static MoodleLoginError loginError;
        private static string loginResult;


        public delegate void Callback(string result);
        public static event Callback OnLoginSuccess;
        public static event Callback OnLoginError;

        public static event Callback OnEventSent;



        /// <summary>
        /// Inicializa el SDK de informXR, envolviendo la inicialización en tu propio SDK.
        /// </summary>
        public static void Initialize(string appId, string orgId, string authSecret) {
            if (isInitialized) {
                Debug.LogWarning("iXR SDK ya fue inicializado.");
                return;
            }

            // Aquí usas las llamadas reales al SDK base de informXR


            isInitialized = true;
            Debug.Log("iXR SDK personalizado inicializado.");
        }

        // --------------------------
        // EVENT
        // --------------------------
        public static void Event(string name) {
            Event(name, null);
        }



        public static void Event(string name, Dictionary<string, string> meta = null) {
            //EnsureInitialized();

            //InformXRManager.TrackEvent(name, meta); // Llama al método real del SDK base
            Debug.Log($"Evento enviado: {name}");
        }

        // --------------------------
        // LEVEL START
        // --------------------------
        public static void EventLevelStart(string levelName) {
            EventLevelStart(levelName, (Dictionary<string, string>)null);
        }

        public static void EventLevelStart(string levelName, Dictionary<string, string> meta = null) {
            //InformXRManager.TrackLevelStart(levelName, meta); // Llama al SDK base
            Debug.Log($"Nivel iniciado: {levelName}");
        }

        public static void EventLevelStart(string levelName, string metaString = "") {
            //var meta = ParseMetaString(metaString);
            //EventLevelStart(levelName, meta);
        }

        // --------------------------
        // LEVEL COMPLETE
        // --------------------------
        public static void EventLevelComplete(string levelName, int score) {
            EventLevelComplete(levelName, score, (Dictionary<string, string>)null);
        }

        public static void EventLevelComplete(string levelName, int score, Dictionary<string, string> meta = null) {
            //EnsureInitialized();
        }

        public static void EventLevelComplete(string levelName, int score, string metaString = "") {
            //var meta = ParseMetaString(metaString);
            //EventLevelComplete(levelName, score, meta);
        }





        public static void CallLogin(string username, string password, string scormName) {

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass pluginClass = new AndroidJavaClass("com.singularisvr.reportxr.plugin.rXRLib");

            UnityCallbackProxy callback = new UnityCallbackProxy(result => {
                Debug.Log($"[ReportXR] Login result: {result}");
                loginResult = result;

                if (result.Contains("token")) {
                    loginSuccess = JsonUtility.FromJson<MoodleLoginSuccess>(result);
                    OnLoginSuccess?.Invoke(result);
                }
                else {
                    loginError = JsonUtility.FromJson<MoodleLoginError>(result);
                    OnLoginError?.Invoke(result);
                }
            });

            pluginClass.CallStatic("login", currentActivity, username, password, scormName, callback);
        }

        public static void CallEvent(string token, int scoId, int attempt, string eventName, string eventValue) {

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass pluginClass = new AndroidJavaClass("com.singularisvr.reportxr.plugin.rXRLib");

            UnityCallbackProxy callback = new UnityCallbackProxy(result => {
                Debug.Log($"[ReportXR] Event result: {result}");
                OnEventSent?.Invoke(result);
            });

            pluginClass.CallStatic("sendEvent", currentActivity, token, scoId, attempt, eventName, eventValue, callback);
        }

        public static void CallEvent(int userId, int attempt, string eventName, string eventValue) {

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass pluginClass = new AndroidJavaClass("com.singularisvr.reportxr.plugin.rXRLib");

            UnityCallbackProxy callback = new UnityCallbackProxy(result => {
                Debug.Log($"[ReportXR] Event result: {result}");
                OnEventSent?.Invoke(result);
            });

            pluginClass.CallStatic("sendSingularisEvent", currentActivity, userId, attempt, eventName, eventValue, callback);
        }

    }




}




