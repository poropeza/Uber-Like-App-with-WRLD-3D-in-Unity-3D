using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RootObject
{
    [System.Serializable]
    public class Intersection
    {
        public int @out;// { get; set; }
        public List<bool> entry;// { get; set; }
        public List<int> bearings;// { get; set; }
        public List<double> location;// { get; set; }
        public int? @in;// { get; set; }
    }

    [System.Serializable]
    public class Maneuver
    {
        public int bearing_after;// { get; set; }
        public int bearing_before;// { get; set; }
        public List<double> location;// { get; set; }
        public string type;// { get; set; }
        public string modifier;// { get; set; }
    }

    [System.Serializable]
    public class Step
    {
        public List<Intersection> intersections { get; set; }
        public string driving_side;// { get; set; }
        public string geometry;// { get; set; }
        public string mode;// { get; set; }
        public Maneuver maneuver;// { get; set; }
        public double weight;// { get; set; }
        public double duration;// { get; set; }
        public string name;// { get; set; }
        public double distance;// { get; set; }
        public string @ref;// { get; set; }
    }

    [System.Serializable]
    public class Leg
    {
        public string summary;// { get; set; }
        public int weight;// { get; set; }
        public double duration;// { get; set; }
        public List<Step> steps;// { get; set; }
        public double distance;// { get; set; }
    }

    [System.Serializable]
    public class Route
    {
        public string geometry;// { get; set; }
        public List<Leg> legs;// { get; set; }
        public string weight_name;// { get; set; }
        public int weight;// { get; set; }
        public double duration;// { get; set; }
        public double distance;// { get; set; }
    }

    [System.Serializable]
    public class Waypoint
    {
        public string hint;// { get; set; }
        public string name;// { get; set; }
        public List<double> location;// { get; set; }
    }


    public List<Route> routes; //{ get; set; }
    public List<Waypoint> waypoints { get; set; }
    public string code { get; set; }

    public static RootObject CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<RootObject>(jsonString);
    }


    /* public string name;
     public int lives;
     public float health;

     public static PlayerInfo CreateFromJSON(string jsonString)
     {
         return JsonUtility.FromJson<PlayerInfo>(jsonString);
     }*/

}
