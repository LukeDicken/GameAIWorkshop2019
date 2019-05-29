using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAlerter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string colName = other.name; // pull the name of the other collider
        if(colName == "FPSController")
        {
            // the player did this
            // build the custom event data
            // fire a custom event
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("counterName", "Collision");
            parameters.Add("primaryParameter", this.name);
            AnalyticsManager.logCustomEvent(parameters);
        }
    }
}