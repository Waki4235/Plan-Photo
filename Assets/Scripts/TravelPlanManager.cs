using UnityEngine;
using System.Collections.Generic;

public class TravelPlanManager : MonoBehaviour
{
    [System.Serializable]
    public class PlannedLocation
    {
        public string name;
        public string gridId;
    }

    public List<PlannedLocation> plannedLocations = new List<PlannedLocation>();

    public void AddLocation(string name, string gridId)
    {
        plannedLocations.Add(new PlannedLocation
        {
            name = name,
            gridId = gridId
        });

        Debug.Log($"[Plan Added] {name} ({gridId})");
    }
}
