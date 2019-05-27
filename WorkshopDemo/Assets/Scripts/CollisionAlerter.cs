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
            // fire an event
            //AnalyticsManager.logCustomEvent();
        }
    }
}
