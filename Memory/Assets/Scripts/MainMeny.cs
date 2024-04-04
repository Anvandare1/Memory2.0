using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMeny : MonoBehaviour
{

    public void Spela()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Spelet har avslutats");
    }




 
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
