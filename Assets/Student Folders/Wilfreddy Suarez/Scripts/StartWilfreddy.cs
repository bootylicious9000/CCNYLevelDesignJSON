using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWilfreddy : MonoBehaviour
{

    public GameObject Song;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        Song.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            Time.timeScale = 1;
            Song.SetActive(true);
        }
    }
}
