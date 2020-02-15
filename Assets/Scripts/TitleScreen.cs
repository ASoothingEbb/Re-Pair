using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    bool noInput = true;
    float elapsed = 0;
    public Image blackscreen;
    public GameObject music;
    public void Update()
    {
        if (noInput)
        {
            if (Input.anyKey)
            {
                noInput = false;
            }
        }
        else
        {
            if(elapsed < 1)
            {
                elapsed += Time.deltaTime;
                blackscreen.color = new Color(0, 0, 0, elapsed);
            }
            else
            {
                DontDestroyOnLoad(music);
                music.GetComponent<AudioSource>().Play();
                SceneManager.LoadScene("0");
            }
        }
    }
}
