using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MiniJSON;
using System;

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

    void Start()
    {
        this.connectionString = "http://" + host + ":" + port.ToString();
        AnalyticsManager.am = this;
    }

    public static void logSessionStart()
    {
        // when a new session is started, fire a new session event
        string req = am.connectionString + "/sessionStart";
        Dictionary<string, string> pairs = new Dictionary<string, string>();
        pairs.Add("Request", req);
        DateTime localDate = DateTime.UtcNow; // note - UTC to standard
        pairs.Add("startTime", localDate.ToString());
        am.StartCoroutine("_logEvent", pairs);
    }

    public static void logCustomEvent(Dictionary<string, string> data)
    {
        // fire a generic event
        string req = am.connectionString + "/counter";
        data.Add("Request", req);
        am.StartCoroutine("_logEvent", data);
    }

    public static void logNewPlayer()
    {
        Debug.Log("Registering new player");
        string req = am.connectionString + "/newPlayer";
        Dictionary<string, string> data = new Dictionary<string, string>();
        data.Add("Request", req);
        am.StartCoroutine("_logEvent", data);
    }

    IEnumerator _logEvent(Dictionary<string, string> keyValues)
    {
        if (isCDNAvailable)
        {
            string request = (string)keyValues["Request"];
            string pid = "Unknown";
            if (PlayerPrefs.HasKey("PlayerID"))
            {
                pid = PlayerPrefs.GetString("PlayerID");
            }
            keyValues.Add("PlayerID", pid);
            string data = MiniJSON.Json.Serialize(keyValues);
            UnityWebRequest www = UnityWebRequest.Put(request, data);
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");
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
        else
        {
            Debug.LogError("Ingestion service disabled or not reachable");
        }
    }
}
