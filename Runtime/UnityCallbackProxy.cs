using System;
using UnityEngine;

namespace SingularisVR.ReportXR {
    public class UnityCallbackProxy : AndroidJavaProxy {

        public Action<string> onResult;


        public UnityCallbackProxy(Action<string> onResult) : base("com.singularisvr.reportxr.plugin.UnityCallback") {
            this.onResult = onResult;
        }


        public void sendResult(string result) {
            if (onResult != null)
                onResult(result);
            else
                Debug.LogWarning("UnityCallbackProxy: onResult is null, no callback to invoke.");
        }
    }

}
