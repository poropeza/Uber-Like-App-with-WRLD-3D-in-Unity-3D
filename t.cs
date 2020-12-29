using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class t : MonoBehaviour
{
    public Text label;
	// Update is called once per frame
	void Update ()
    {
        label.text = "LAT: "+GPS.Instance.lat.ToString()+"   LONG: "+GPS.Instance.lon.ToString();
	}
}
