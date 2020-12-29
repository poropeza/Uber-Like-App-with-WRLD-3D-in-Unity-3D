using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Wrld.Space;
using Wrld;


public class OSRM : MonoBehaviour {

    List<Location> trayectoria;
    public GameObject[] ruta_objetos;
    LineRenderer linea;
    GameObject cliente;
    GameObject marcador;
    GameObject wrldi;

    void Start()
    {
        var lat_ini = PlayerPrefs.GetFloat("lat_ini");
        var lon_ini = PlayerPrefs.GetFloat("lon_ini");


        //(double)lat_ini;
        //(double)lon_ini;
        //muevo la cámara a cierta coordenada
        //var startLocation = LatLong.FromDegrees(18.92163, -99.20459);
        var startLocation = LatLong.FromDegrees(lat_ini, lon_ini);
        Api.Instance.CameraApi.MoveTo(startLocation, distanceFromInterest: 800, headingDegrees: 0, tiltDegrees: 50);

        cliente = GameObject.Find("cliente");
        marcador = GameObject.Find("nodo");
        wrldi = GameObject.Find("WrldMap");

        StartCoroutine(ruta());

    }

    void Update()
    {
        if (GameObject.Find("WrldMap").activeInHierarchy)
        {
            for (int i = 0; i < ruta_objetos.Length; i++)
            {
                //Debug.DrawLine(ruta_objetos[i].transform.position, ruta_objetos[i + 1].transform.position, Color.red);
                linea.SetPosition(i, ruta_objetos[i].transform.position);
            }
        }

        

        

    }



    public class Leg
    {
        public string summary { get; set; }
        public double weight { get; set; }
        public double duration { get; set; }
        public List<object> steps { get; set; }
        public double distance { get; set; }
    }

    public class Route
    {
        public string geometry { get; set; }
        public List<Leg> legs { get; set; }
        public string weight_name { get; set; }
        public double weight { get; set; }
        public double duration { get; set; }
        public double distance { get; set; }
    }

    public class Waypoint
    {
        public string hint { get; set; }
        public string name { get; set; }
        public List<double> location { get; set; }
    }


    public class RootObject
    {

        public List<Route> routes { get; set; }
        public List<Waypoint> waypoints { get; set; }
        public string code { get; set; }
    }

    [System.Serializable]
    public class DataReader
    {
        public RootObject[] events;
    }

    IEnumerator ruta()
    {
        //las coordenadas van volteadas => lon,lat->lat,lon
        var lat_entrada=PlayerPrefs.GetString("lat_final");
        var lon_entrada = PlayerPrefs.GetString("lon_final");

        var lat_ini = PlayerPrefs.GetFloat("lat_ini");
        var lon_ini = PlayerPrefs.GetFloat("lon_ini");

        Debug.Log("lat:" + lat_ini + "    lon: " + lon_ini);

        //lat_ini = 25.776417f;
        //lon_ini = -80.131531f;
        //lon_ini = -99.20459,18.92163;
        UnityWebRequest www = UnityWebRequest.Get("https://router.project-osrm.org/route/v1/driving/"+ lon_ini +","+lat_ini+";"+ lon_entrada + ","+ lat_entrada);//?steps=true me trae los modificadores paso a paso para llegar al destino
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text

            //Debug.Log(www.downloadHandler.text);
            // DataReader root = JsonUtility.FromJson<DataReader>(www.downloadHandler.text);
            //Debug.Log(root.events[0].routes[0].geometry);
            var inicio = www.downloadHandler.text.IndexOf("geometry") + 11;

            string ruta_codificada = "";

            for (int i = inicio; i < www.downloadHandler.text.Length; i++)
            {
                if (www.downloadHandler.text[i] == '"')
                {
                    break;
                }
                else
                {
                    ruta_codificada = ruta_codificada + www.downloadHandler.text[i];
                }
            }

            //Debug.Log(ruta_codificada);

            trayectoria = DecodePolylinePoints(ruta_codificada);

            
            var xz = 1;

            ruta_objetos = new GameObject[trayectoria.Count];
            var xi = 0;

            linea = GetComponent<LineRenderer>();


            foreach (var coordenada in trayectoria)
            {
                //Debug.Log("[" + coordenada.lon + "," + coordenada.lat + "]");//para la coordenada real, necesito intercambiar los valores de lon y lat

                /*Vector3 xyz_vector1 = new Vector3();
               xyz_vector1 =  Quaternion.AngleAxis((float)coordenada.lon, -Vector3.up) * Quaternion.AngleAxis((float)coordenada.lat, -Vector3.right) * new Vector3(0, 0, 1);
                xyz_vector1.Set(xyz_vector1.x, 1f, xyz_vector1.z);*/

                GameObject nodo = (GameObject)Instantiate(marcador, transform.position, transform.rotation);
                nodo.transform.localScale = new Vector3(5f, 5f, 5f);

                MeshRenderer mesh = nodo.GetComponent<MeshRenderer>();
                mesh.enabled = false;

                nodo.GetComponent<GeographicTransform>().SetPosition(new LatLong(coordenada.lat,coordenada.lon));
                nodo.GetComponent<GeographicTransform>().SetElevation(12);

                

                ruta_objetos[xi] = nodo;
                xi = xi + 1;

                if (xz==1)//es el punto inicial donde debe empezar el coche
                {
                    WrldMap wp= wrldi.GetComponent<WrldMap>();
                    wp.m_latitudeDegrees = coordenada.lat;
                    wp.m_longitudeDegrees = coordenada.lon;
                    cliente.GetComponent<GeographicTransform>().SetPosition(new LatLong(coordenada.lat, coordenada.lon));
                    cliente.GetComponent<GeographicTransform>().SetElevation(2);

                    xz = xz + 1;
                }

                //StartCoroutine(espera());             
            }

            linea.positionCount = ruta_objetos.Length;


        }
    }



    IEnumerator espera()
    {    
        yield return new WaitForSeconds(1);        
    }

    public class Location
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    
     private List<Location> DecodePolylinePoints(string encodedPoints) 
            {
                if (encodedPoints == null || encodedPoints == "") return null;
                List<Location> poly = new List<Location>();
                char[] polylinechars = encodedPoints.ToCharArray();
                int index = 0;

                int currentLat = 0;
                int currentLng = 0;
                int next5bits;
                int sum;
                int shifter;
        
                try
                {
                    while (index < polylinechars.Length)
                    {
                        // calculate next latitude
                        sum = 0;
                        shifter = 0;
                        do
                        {
                            next5bits = (int)polylinechars[index++] - 63;
                            sum |= (next5bits & 31) << shifter;
                            shifter += 5;
                        } while (next5bits >= 32 && index < polylinechars.Length);

                        if (index >= polylinechars.Length)
                            break;

                        currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                        //calculate next longitude
                        sum = 0;
                        shifter = 0;
                        do
                        {
                            next5bits = (int)polylinechars[index++] - 63;
                            sum |= (next5bits & 31) << shifter;
                            shifter += 5;
                        } while (next5bits >= 32 && index < polylinechars.Length);

                        if (index >= polylinechars.Length && next5bits >= 32)
                            break;

                        currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                        Location p = new Location();
                        p.lat = (double)currentLat / 100000.0;
                        p.lon = (double)currentLng/ 100000.0;
                        poly.Add(p);
                    } 
                }
                catch (System.Exception ex)
                {
                    // logo it
                }
                return poly;
           }
     



}



