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

            AnalyticsManager.getConfig(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ParseConfig(Dictionary<string, object> parameters)
    {
        /*
         * Currently this function randomly chooses one of the sun configs
         * This morning we will make this driven by data received over the air
         * from our "model" service
         */
        int i = UnityEngine.Random.Range(1, 3);
        switch (i)
        {
            // TODO - flip this out for a query
            case 1:
                sun_config1();
                break;
            case 2:
                sun_config2();
                break;
            default:
                Debug.LogError("Something went wrong");
                break;
        }

    }

    void sun_config1()
    {
        // sunset
        // rotate the light
        Vector3 rotation = new Vector3(8, -180, 0);
        sun.transform.rotation = Quaternion.Euler(rotation);
        // change the color of the light
        Light lig = sun.GetComponent<Light>();
        lig.color = new Color(0.8f, 0.5803922f, 0.01960784f);
    }

    void sun_config2()
    {
        // daytime
        // rotate the light
        Vector3 rotation = new Vector3(50, 0, 0);
        sun.transform.rotation = Quaternion.Euler(rotation);
    }

    void manage_playerID()
    {
        // We can use the PlayerPrefs 
        if(PlayerPrefs.HasKey("PlayerID"))
        {
            // already set - we're good
        }
        else
        {

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
