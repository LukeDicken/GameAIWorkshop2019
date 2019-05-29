using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SceneManager : MonoBehaviour
{
    public bool ChangeConfig;
    public string PlayerID;
    public string SessionID;
    GameObject sun;
    // Start is called before the first frame update
    void Start()
    {
        manage_playerID();
        this.PlayerID = PlayerPrefs.GetString("PlayerID");
        this.new_sessionID();
        if (ChangeConfig)
        {
            sun = GameObject.FindWithTag("Sun");
            //int i = UnityEngine.Random.Range(1, 3);
            //switch (i)
            //{
            //    // TODO - flip this out for a query
            //    case 1:
            //        sun_config1();
            //        break;
            //    case 2:
            //        sun_config2();
            //        break;
            //    default:
            //        Debug.LogError("Something went wrong");
            //        break;
            //}
            AnalyticsManager.getConfig(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ParseConfig(Dictionary<string, object> parameters)
    {
        //Debug.Log("Start of sm callback");
        if(parameters.ContainsKey("configValue"))
        {
            int config =  (int)((long)parameters["configValue"]);
            switch (config)
            {
                case 0:
                    sun_config1();
                    break;
                case 1:
                    sun_config2();
                    break;
                default:
                    Debug.LogError("Response was not expected");
                    break;
            }
            //Debug.Log("Completed config routine");
        }
        else
        {
            Debug.LogError("Did not receive a well formed response from config service");
            sun_config2();
        }
    }

    void sun_config1()
    {
        // sunset
        Vector3 rotation = new Vector3(8, -180, 0);
        sun.transform.rotation = Quaternion.Euler(rotation);
        //Debug.Log("Config 1");
        Light lig = sun.GetComponent<Light>();
        lig.color = new Color(0.8f, 0.5803922f, 0.01960784f);
    }

    void sun_config2()
    {
        // daytime
        Vector3 rotation = new Vector3(50, 0, 0);
        sun.transform.rotation = Quaternion.Euler(rotation);
        //Debug.Log("Config 2");

    }

    void manage_playerID()
    {
        if(PlayerPrefs.HasKey("PlayerID"))
        {
            // already set - we're good
        }
        else
        {
            /*
             * In an ideal world, Identity would be handled outside server-side
             * Identity is genuinely one of the hardest things in Analytics,
             * since there are so many freaking edge cases around:
             *      Anonymous device usage
             *      Social network linkage
             *      Shared devices
             * Also be aware that tracking an identity can causes issues with
             * GDPR and similar systems.
             * 
             * For this exercise we are not tracking any identifiable info, so
             * we're safe
             */
            int rand = UnityEngine.Random.Range(0, 1000);
            string hash = Hash128.Compute(SystemInfo.deviceUniqueIdentifier + rand.ToString()).ToString();
            PlayerPrefs.SetString("PlayerID", hash);
            PlayerPrefs.Save();
            AnalyticsManager.logNewPlayer();
        }
    }

    void new_sessionID()
    {
        string hash = Hash128.Compute(DateTime.UtcNow.ToString()).ToString();
        PlayerPrefs.SetString("SessionID", hash);
        PlayerPrefs.Save();
        AnalyticsManager.logSessionStart();
    }
}
