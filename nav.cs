using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class nav : MonoBehaviour
{

	public void IniciarRecorrido()
    {
        SceneManager.LoadScene("destino");
    }

    public void IniciarRecorrido1()
    {
        Destroy(GameObject.Find("Main Camera"));
        SceneManager.LoadScene("destino");
    }

    public void Salir()
    {
        Application.Quit();


       //string url = "http://10.0.0.5/?T";
        //WWW www = new WWW(url);
        //StartCoroutine(WaitForRequest(www));

    }

    public void AcercaNosotros()
    {
        SceneManager.LoadScene("Nosotros");
    }

    public void menuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
