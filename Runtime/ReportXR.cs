using UnityEngine;


namespace SingularisVR.ReportXR
{
    public class ReportXR : MonoBehaviour
    {
        private static bool isInitialized = false;

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
            EnsureInitialized();

            InformXRManager.TrackEvent(name, meta); // Llama al método real del SDK base
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


            InformXRManager.TrackLevelStart(levelName, meta); // Llama al SDK base
            Debug.Log($"Nivel iniciado: {levelName}");
        }

        public static void EventLevelStart(string levelName, string metaString = "")
        {
            var meta = ParseMetaString(metaString);
            EventLevelStart(levelName, meta);
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
            EnsureInitialized();


        }

        public static void EventLevelComplete(string levelName, int score, string metaString = "")
        {
            var meta = ParseMetaString(metaString);
            EventLevelComplete(levelName, score, meta);
        }

    }

}


