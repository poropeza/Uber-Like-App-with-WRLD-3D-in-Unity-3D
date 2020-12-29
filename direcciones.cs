using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Wrld.Space;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.Text;

public class direcciones : MonoBehaviour {

    void Start()
    {
        //hay que procesar la cadena json y transformar los \ en * u otro caracter para que no sean sentencias de escape y generen errores
        string s = "{\"routes\":[{\"geometry\":\"yuj|CflahN`@B_@tEdM`Ba@bE\",\"legs\":[{\"summary\":\"12th Street, Collins Avenue\",\"weight\":256.9,\"duration\":163.1,\"steps\":[{\"intersections\":[{\"out\":0,\"entry\":[true],\"bearings\":[188],\"location\":[-80.130116,25.782847]}],\"driving_side\":\"right\",\"geometry\":\"yuj|CflahN`@B\",\"mode\":\"driving\",\"maneuver\":{\"bearing_after\":188,\"bearing_before\":0,\"location\":[-80.130116,25.782847],\"type\":\"depart\"},\"weight\":22.1,\"duration\":8.5,\"name\":\"Ocean Drive\",\"distance\":19.3},{\"intersections\":[{\"out\":2,\"in\":0,\"entry\":[false,true,true],\"bearings\":[15,195,285],\"location\":[-80.130143,25.782675]}],\"driving_side\":\"right\",\"geometry\":\"wtj|CjlahNOjBOhB\",\"mode\":\"driving\",\"maneuver\":{\"bearing_after\":278,\"bearing_before\":187,\"location\":[-80.130143,25.782675],\"modifier\":\"right\",\"type\":\"turn\"},\"weight\":105.1,\"duration\":61.5,\"name\":\"12th Street\",\"distance\":108.6},{\"intersections\":[{\"out\":2,\"in\":1,\"entry\":[true,false,true,true],\"bearings\":[15,105,195,285],\"location\":[-80.131213,25.782835]},{\"out\":2,\"in\":0,\"entry\":[false,true,true,true],\"bearings\":[15,105,195,285],\"location\":[-80.131431,25.781783]}],\"driving_side\":\"right\",\"geometry\":\"wuj|C`sahNfDb@j@FpFt@\",\"mode\":\"driving\",\"maneuver\":{\"bearing_after\":189,\"bearing_before\":278,\"location\":[-80.131213,25.782835],\"modifier\":\"left\",\"type\":\"turn\"},\"ref\":\"FL A1A\",\"weight\":97.9,\"duration\":61.400000000000006,\"name\":\"Collins Avenue\",\"distance\":256.6},{\"intersections\":[{\"out\":3,\"in\":0,\"entry\":[false,true,true,true],\"bearings\":[15,105,195,285],\"location\":[-80.131701,25.78057]}],\"driving_side\":\"right\",\"geometry\":\"qgj|CbvahNSjBMvA\",\"mode\":\"driving\",\"maneuver\":{\"bearing_after\":279,\"bearing_before\":191,\"location\":[-80.131701,25.78057],\"modifier\":\"right\",\"type\":\"turn\"},\"weight\":31.8,\"duration\":31.7,\"name\":\"10th Street\",\"distance\":99.6},{\"intersections\":[{\"in\":0,\"entry\":[true],\"bearings\":[101],\"location\":[-80.132677,25.780742]}],\"driving_side\":\"right\",\"geometry\":\"shj|Cf|ahN\",\"mode\":\"driving\",\"maneuver\":{\"bearing_after\":0,\"bearing_before\":281,\"location\":[-80.132677,25.780742],\"modifier\":\"left\",\"type\":\"arrive\"},\"weight\":0,\"duration\":0,\"name\":\"10th Street\",\"distance\":0}],\"distance\":484.2}],\"weight_name\":\"routability\",\"weight\":256.9,\"duration\":163.1,\"distance\":484.2}],\"waypoints\":[{\"hint\":\"aZLYgGyS2IBFAAAAjAEAAAAAAAAAAAAAGPeZQYq28EIAAAAAAAAAAEUAAACMAQAAAAAAAAAAAABCNwAAvE85-z9qiQHDTzn7PmqJAQAAfwDuc9Fw\",\"name\":\"Ocean Drive\",\"location\":[-80.130116,25.782847]},{\"hint\":\"zYx1hMqAAoUgAAAAyQAAAAAAAAAAAAAApcUCQT-hMkIAAAAAAAAAACAAAADJAAAAAAAAAAAAAABCNwAAu0U5-wZiiQGvRTn7z2GJAQAAXwbuc9Fw\",\"name\":\"10th Street\",\"location\":[-80.132677,25.780742]}],\"code\":\"Ok\"}";
        RootObject p = RootObject.CreateFromJSON(s);

        string dir = "";

        for (int i = 0; i < p.routes[0].legs[0].steps.Count; i++)
        {
            dir = "";

            if (p.routes[0].legs[0].steps[i].maneuver.bearing_before < 90 || p.routes[0].legs[0].steps[i].maneuver.bearing_before > 270)
            {
                dir = dir + "N";
            }
            else if (p.routes[0].legs[0].steps[i].maneuver.bearing_before > 90 || p.routes[0].legs[0].steps[i].maneuver.bearing_before <270)
            {
                dir = dir + "S";
            }

            if (p.routes[0].legs[0].steps[i].maneuver.bearing_before!=0 && p.routes[0].legs[0].steps[i].maneuver.bearing_before != 180 && p.routes[0].legs[0].steps[i].maneuver.bearing_before != 90 && p.routes[0].legs[0].steps[i].maneuver.bearing_before != 270)
            {
                dir = dir + p.routes[0].legs[0].steps[i].maneuver.bearing_before+ "°";
            }
            

            if (p.routes[0].legs[0].steps[i].maneuver.bearing_before > 0 &&  p.routes[0].legs[0].steps[i].maneuver.bearing_before < 180)
            {
                dir = dir + "E";
            }
            else if (p.routes[0].legs[0].steps[i].maneuver.bearing_before > 180 && p.routes[0].legs[0].steps[i].maneuver.bearing_before < 360)
            {
                dir = dir + "W";
            }

            /*
            if (p.routes[0].legs[0].steps[i].maneuver.type!=null)
            {
                Debug.Log(p.routes[0].legs[0].steps[i].maneuver.type+" "+ p.routes[0].legs[0].steps[i].maneuver.modifier + " " + dir + " on " + p.routes[0].legs[0].steps[i].name +"("+ p.routes[0].legs[0].steps[i].distance+"m and "+ p.routes[0].legs[0].steps[i].duration+"s)");
            }
            else
            {
                Debug.Log(p.routes[0].legs[0].steps[i].maneuver.modifier + " " + dir + " on " + p.routes[0].legs[0].steps[i].name + "(" + p.routes[0].legs[0].steps[i].distance + "m and " + p.routes[0].legs[0].steps[i].duration + "s)");
            }*/

            
        }

        
        //Debug.Log(p.routes[0].weight);
    }


    // Use this for initialization
    public void EmpezarBusqueda ()
    {
       obtenerUbicacionDestino();
    }

    private void obtenerUbicacionDestino()
    {

    	var lat_ini = PlayerPrefs.GetFloat("lat_ini");
        var lon_ini = PlayerPrefs.GetFloat("lon_ini");

        //las coordenadas van volteadas => lon,lat->lat,lon
        var entrada = GameObject.Find("destino").GetComponent<Text>().text;
        //string url= "http://geocoder.api.here.com/6.2/geocode.json?app_id=XLSu8zWz3rMXCPGz6ZTG&app_code=hxY7PXuR6MjXq6ghVpMK1g&searchtext=" + entrada;
        //string url = "http://places.cit.api.here.com/places/v1/discover/search?at=25.778373,-80.132164&q="+entrada+"&app_id=XLSu8zWz3rMXCPGz6ZTG&app_code=hxY7PXuR6MjXq6ghVpMK1g";
        string url = "http://places.cit.api.here.com/places/v1/discover/search?at="+lat_ini+","+lat_ini+"&q="+entrada+"&app_id=XLSu8zWz3rMXCPGz6ZTG&app_code=hxY7PXuR6MjXq6ghVpMK1g";
        //at se debe de sustituir por la posicion gps actual de la persona

        WebClient myWebClient = new WebClient();
        byte[] myDataBuffer = myWebClient.DownloadData(url);
        string download = Encoding.ASCII.GetString(myDataBuffer);

        //Debug.Log(download);


        var inicio = download.IndexOf("position") + 11;

        string lat = "";
        string longi = "";

        for (int i = inicio; i < download.Length; i++)
        {
            if (download[i] == ',')
            {
                break;
            }
            else
            {
                lat = lat + download[i];
            }

        }
        var es_lon = 0;

        for (int i = inicio; i < download.Length; i++)
        {
            if (download[i] == ']')
            {
                break;
            }
            else
            {
                if (download[i] == ',')
                {
                    es_lon = 1;
                }

                if (es_lon == 1 && download[i] != ',')
                {
                    longi = longi + download[i];
                }
            }
          
            

        }


        //Debug.Log("Posición: " + lat + "," + longi);

        PlayerPrefs.SetString("lat_final", lat);
        PlayerPrefs.SetString("lon_final", longi);


        SceneManager.LoadScene("UnityWorldSpace");

        /*

        var inicio = download.IndexOf("Latitude")+10;

        string lat = "";
        string longi = "";

         for (int i = inicio; i < download.Length; i++)
         {
            if (download[i] == ',')
            {
                break;
            }
            else
            {
                lat = lat + download[i];         
            }

        }

        

        var inicio1 = download.IndexOf("Longitude") +11;


        for (int i = inicio1; i < download.Length; i++)
        {
            if (download[i] == '}')
            {
                break;
            }
            else
            {
                longi = longi + download[i];
            }

        }

        //Debug.Log("Posición: " + lat + "," + longi);

        PlayerPrefs.SetString("lat", lat);
        PlayerPrefs.SetString("lon", longi);


        SceneManager.LoadScene("UnityWorldSpace");
        */
    }


}
