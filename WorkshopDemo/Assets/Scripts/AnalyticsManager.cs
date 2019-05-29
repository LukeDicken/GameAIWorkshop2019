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


        // create a dictionary of parameters
        // set up the correct endpoint in the dictionary
        // add a time stamp to the event
        // add the passed SessionID
        // start the coroutine
        Dictionary<string, string> data = new Dictionary<string, string>();
        string req = am.connectionString + "/sessionStart";
        data.Add("Request", req);
        DateTime localDate = DateTime.UtcNow; // note - UTC to standard
        data.Add("startTime", localDate.ToString());
        am.StartCoroutine("_logEvent", data);
    }

    public static void logNewPlayer()
    {
        // register a new player

        // create a dictionary or parameters
        // set up the correct endpoint
        // start the coroutine
        Dictionary<string, string> data = new Dictionary<string, string>();
        string req = am.connectionString + "/newPlayer";
        data.Add("Request", req);
        am.StartCoroutine("_logEvent", data);
    }

    public static void logCustomEvent(Dictionary<string, string> data)
    {
        // fire a generic event

        // get the dictionary as a passed parameter
        // add the correct endpoint
        // start the coroutine
        string req = am.connectionString + "/counter";
        data.Add("Request", req);
        am.StartCoroutine("_logEvent", data);
    }

    public static void getConfig(SceneManager sm)
    {
        am.StartCoroutine("_getConfig", sm);
    }

    public IEnumerator _getConfig(SceneManager sm)
    {
        if (isCDNAvailable)
        {
            string request = this.connectionString + "/playerType?PlayerID="+PlayerPrefs.GetString("PlayerID");
            UnityWebRequest www = UnityWebRequest.Get(request);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                isCDNAvailable = false;
                yield break;
            }
            Dictionary<string, object> resp = (Dictionary<string, object>)MiniJSON.Json.Deserialize(www.downloadHandler.text);
            sm.ParseConfig(resp);
        }
    }

    IEnumerator _logEvent(Dictionary<string, string> keyValues)
    {
        if (isCDNAvailable)
        {
            string request = (string)keyValues["Request"];
            keyValues.Remove("Request");
            string pid = "Unknown";
            if (PlayerPrefs.HasKey("PlayerID"))
            {
                pid = PlayerPrefs.GetString("PlayerID");
            }
            string sessionID = "-";
            if (PlayerPrefs.HasKey("SessionID"))
            {
                sessionID = PlayerPrefs.GetString("SessionID");
            }
            keyValues.Add("PlayerID", pid);
            keyValues.Add("SessionID", sessionID);
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
        }
        else
        {
            Debug.LogError("Ingestion service disabled or not reachable");
        }
    }
}
