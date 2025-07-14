using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;


namespace SingularisVR.ReportXR
{





    [System.Serializable]
    public class MoodleUser
    {
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
    public class MoodleResponse
    {
        public string token;
        public MoodleUser user;
    }






    public class ReportXR 
    {
        private static bool isInitialized = false;
        private static AndroidJavaClass unityClass;
        private static AndroidJavaObject unityActivity;
        private static AndroidJavaObject rXRLib;
        public const string userName = "user_vr";
        public const string password = "Az12345678-";
		public const string scormName = "Abra 2004";


        /// <summary>
        /// Inicializa el SDK de informXR, envolviendo la inicialización en tu propio SDK.
        /// </summary>
        public static void Initialize(string appId, string orgId, string authSecret)
        {
            if (isInitialized)
            {
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
        public static void Event(string name)
        {
            Event(name, null);
        }

      

        public static void Event(string name, Dictionary<string, string> meta = null)
        {
            //EnsureInitialized();

            //InformXRManager.TrackEvent(name, meta); // Llama al método real del SDK base
            Debug.Log($"Evento enviado: {name}");
        }

        // --------------------------
        // LEVEL START
        // --------------------------
        public static void EventLevelStart(string levelName)
        {
            EventLevelStart(levelName, (Dictionary<string, string>)null);
        }

        public static void EventLevelStart(string levelName, Dictionary<string, string> meta = null)
        {


            //InformXRManager.TrackLevelStart(levelName, meta); // Llama al SDK base
            Debug.Log($"Nivel iniciado: {levelName}");
        }

        public static void EventLevelStart(string levelName, string metaString = "")
        {
            //var meta = ParseMetaString(metaString);
            //EventLevelStart(levelName, meta);
        }

        // --------------------------
        // LEVEL COMPLETE
        // --------------------------
        public static void EventLevelComplete(string levelName, int score)
        {
            EventLevelComplete(levelName, score, (Dictionary<string, string>)null);
        }

        public static void EventLevelComplete(string levelName, int score, Dictionary<string, string> meta = null)
        {
            //EnsureInitialized();


        }

        public static void EventLevelComplete(string levelName, int score, string metaString = "")
        {
            //var meta = ParseMetaString(metaString);
            //EventLevelComplete(levelName, score, meta);
        }
       



        public static void CallAddEvent()
        {

           

            unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

            rXRLib = new AndroidJavaObject("com.singularisvr.reportxr.plugin.rXRLib");

            try
            {
                string version = rXRLib.CallStatic<string>("getVersion");
                Debug.Log("ReportXR Version: " + version);
            }
            catch (AndroidJavaException e)
            {
                Debug.LogError("Failed: string getVersion(): " + e.Message);
            }         

            try
            {
                bool success = rXRLib.CallStatic<bool>("sendEvent", unityActivity, 1, 1, 1, "unity", "sample");
                Debug.Log("sendEvent success: " + success);
            }
            catch (AndroidJavaException e)
            {
                Debug.LogError("Failed: bool sendEvent(Activity activity, int eventType, int eventSubType, int eventValue, String eventSource, String eventData): " + e.Message);
            }


        }


        public static void TestLogin()
        {
            CallLogin(userName, password, scormName);
        }




        public static void CallLogin(string username, string password, string scormName)
        {

          AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
          AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

         AndroidJavaClass pluginClass = new AndroidJavaClass("com.singularisvr.reportxr.plugin.rXRLib"); // Reemplazá con el nombre real

         UnityCallbackProxy callback = new UnityCallbackProxy(result => {
         Debug.Log("Login result: " + result);

             
             CallEvent(result);

         });
        


         pluginClass.CallStatic("login", currentActivity, username, password, scormName, callback);
        }

        private static void CallEvent(string result)
        {

            MoodleResponse response = JsonUtility.FromJson<MoodleResponse>(result);
            Debug.Log("Token: " + response.token);
            Debug.Log("Usuario: " + response.user.username);
            Debug.Log("UserId" + response.user.userid);


            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass pluginClass = new AndroidJavaClass("com.singularisvr.reportxr.plugin.rXRLib"); // Reemplazá con el nombre real

            UnityCallbackProxy callback = new UnityCallbackProxy(result => {

                Debug.Log("Succes Sending Event" + result);

            });



            pluginClass.CallStatic("sendEvent", currentActivity, 12, response.user.userid, 2, "test", "test2", callback);









        }

    }

 


}




