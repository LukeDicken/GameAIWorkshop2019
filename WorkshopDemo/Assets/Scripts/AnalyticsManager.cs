using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnalyticsManager : MonoBehaviour
{
    /// <summary>
    /// The AnalyticsManager runs all things Analytics
    /// Note that this class inherits MonoBehaviour - in an ideal world
    /// this wouldn't be the case (there are performance implications),
    /// but we want to leverage Unity Coroutines to fire off the web calls
    /// asynchronously.
    /// </summary>

    // static class variable
    private static AnalyticsManager am; // singleton holder

    private string connectionString;

    // inspector fields
    public string host;
    public int port;
    public bool isCDNAvailable;

    public static AnalyticsManager retrieveAM()
    {
        // singleton pattern
        // this saves us having to have an ephemeral GameObject and
        // searching the scene by name/tag when we want to reference the AM
        if (AnalyticsManager.am == null)
        {
            AnalyticsManager.am = new AnalyticsManager();
        }
        return AnalyticsManager.am;
    }


    void Start()
    {
        this.connectionString = "http://" + host + ":" + port.ToString();
        AnalyticsManager.am = this;
    }

    public static void logSessionStart()
    {
        string req = am.connectionString + "/status";
        Dictionary<string, object> pairs = new Dictionary<string, object>();
        pairs.Add("Request", req);

        am.StartCoroutine("_logEvent", pairs);
    }

    //IEnumerator _logSessionStart(string request)
    //{


    //}

    IEnumerator _logEvent(Dictionary<string, object> keyValues)
    {
        string request = (string)keyValues["Request"];
        UnityWebRequest www = UnityWebRequest.Get(request);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            isCDNAvailable = false;
            yield break;
        }
        string result = www.downloadHandler.text;
        Debug.Log(result);
    }
}
