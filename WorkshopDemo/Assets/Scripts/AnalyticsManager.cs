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
    }

    public static void logNewPlayer()
    {
        // register a new player

        // create a dictionary or parameters
        // set up the correct endpoint
        // start the coroutine
    }

    public static void logCustomEvent(Dictionary<string, string> data)
    {
        // fire a generic event

        // get the dictionary as a passed parameter
        // add the correct endpoint
        // start the coroutine
    }

    public static void getConfig(SceneManager sm)
    {
        // start the _getConfig coroutine
    }

    public IEnumerator _getConfig(SceneManager sm)
    {
        if (isCDNAvailable)
        {
            yield return new WaitForEndOfFrame(); // Placeholder for compiler happiness
            // build the GET request object
            // REMEMBER - GETs pass parameters in their request string
            // Createe a UnityWebRequest
            // Send it and yield while it completes
            // use MiniJSON to create a Dictionary object from the UnityWebRequest's downloadHandler field
            // hand off to sm.ParseConfig
        }
    }

    IEnumerator _logEvent(Dictionary<string, string> keyValues)
    {
        if (isCDNAvailable)
        {
            // retrieve the Request URL
            // get the PlayerID from PlayerPrefs and add it to the Dictionoary
            // retrieve the SessionID and add it to the dictionary
            // use MiniJSON to serialize the dictionary into a JSON string
            // Code provided will fire that as a UnityWebRequest


            string request = ""; // placeholder - make sure this is the correct endpoint
            string data = ""; // placeholder - use MiniJSON to populate this
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
