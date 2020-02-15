using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LineRenderer drawCircle;
    public LineRenderer drawLine;
    public LineRenderer drawLilCircle;
    public GameObject allLineDrawers;
    public int circleVerts = 10;
    public float lineWidth = 0.1f;
    Vector3[] circlePoss;

    public Image blackscreen;

    public string thisLevel;
    public string nextLevel;

    public GameObject rotato;
    public GameObject spiritPrefab;
    public GameObject bigSpiritPrefab;

    public GameObject stuff;

    GameObject spirit1;
    GameObject spirit2;

    public float rotateSpeed = 0.2f;

    float startingTime = 20f;
    float timeLeft;

    float fadeElapsed = 0;
    float fadeInTime = 0.5f;
    public bool started = false;

    public AudioSource goodSound;
    public AudioSource badSound;

    void Start()
    {
        allLineDrawers.SetActive(false);
        timeLeft = startingTime;
        spirit1 = Instantiate(spiritPrefab, rotato.transform);
        spirit2 = Instantiate(spiritPrefab, rotato.transform);
        spirit2.transform.localScale = new Vector3(-1, 1, -1);

        float theta = 0;

        circlePoss = new Vector3[circleVerts];
        for (int i = 0; i < circleVerts; i++)
        {
            circlePoss[i] = new Vector3(Mathf.Sin(theta), 0, Mathf.Cos(theta));
            theta += (2 * Mathf.PI) / circleVerts;
        }

        Random.InitState((int)System.DateTime.UtcNow.Ticks);
        float min = 50, max = 150;
        float r = (Random.Range(-1, 1) > 0) ? Random.Range(min * 2f, max) : Random.Range(0, min * 2f);
        float b = (r < min * 2f) ? Random.Range(min * 2f, max) : Random.Range(0, min * 2f);
        float g = (r + b < 150) ? Random.Range(min * 2f, max) : Random.Range(0, min * 2f);
        VisualEffect temp = spirit1.GetComponentInChildren<VisualEffect>();


        temp.SetFloat("r", r);
        temp.SetFloat("g", g);
        temp.SetFloat("b", b);

        temp = spirit2.GetComponentInChildren<VisualEffect>();
        temp.SetFloat("r", max - r);
        temp.SetFloat("g", max - b);
        temp.SetFloat("b", max - g);


        drawCircle.positionCount = circleVerts;
        drawCircle.startWidth = lineWidth;
        drawCircle.endWidth = lineWidth;
        drawLine.positionCount = 2;
        drawLine.startWidth = lineWidth;
        drawLine.endWidth = lineWidth;


        int lilCount = 6; 
        Vector3[] lilPoss = new Vector3[lilCount];
        for (int i = 0; i < lilCount; i++)
        {
            lilPoss[i] = new Vector3(Mathf.Sin(theta), 0, Mathf.Cos(theta));
            theta += (2 * Mathf.PI) / lilCount;
        }
        drawLilCircle.positionCount = lilCount;
        drawLilCircle.SetPositions(lilPoss);
        drawLilCircle.startWidth = lineWidth/2;
        drawLilCircle.endWidth = lineWidth / 2;

        

    }

    // Update is called once per frame
    void Update()
    {
        

        if (!started)
        {
            blackscreen.color = new Color(0, 0, 0, (fadeInTime - fadeElapsed) / fadeInTime);
            fadeElapsed += Time.deltaTime;
            if(fadeElapsed > fadeInTime)
            {
                started = true;
                allLineDrawers.SetActive(true);
            }
        }
        else
        {
            blackscreen.color = new Color(0, 0, 0, 0);
            rotato.transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime, 0), Space.Self);

            //update circle
            float circleSize = spirit1.transform.GetChild(0).transform.position.magnitude; //lol lets hope the spirits never have a y coord
            Vector3[] temp = new Vector3[circlePoss.Length];
            for (int i = 0; i < circleVerts; i++)
            {
                temp[i] = circlePoss[i] * circleSize;
            }
            drawCircle.SetPositions(temp);

            //update line
            drawLine.SetPosition(0, spirit1.transform.GetChild(0).transform.position);
            drawLine.SetPosition(1, spirit2.transform.GetChild(0).transform.position);

            if ((timeLeft / startingTime) < 0.25)
            {
                lose();
            }
            spirit1.transform.GetChild(0).transform.localScale = Vector3.one * (timeLeft / startingTime);
            spirit2.transform.GetChild(0).transform.localScale = Vector3.one * (timeLeft / startingTime);
            timeLeft -= Time.deltaTime;

            if (Input.GetButton("nextLevel"))
            {
                StartCoroutine(LoadScene(nextLevel, 0));
            }

            if(Input.GetButton("prevLevel"))
            {
                if (int.Parse(thisLevel) > 0) {
                    StartCoroutine(LoadScene((int.Parse(thisLevel) - 1).ToString(), 0));
                }
            }

        }
        
    }

    public void win()
    {
        goodSound.Play();
        spirit1.SetActive(false);
        spirit2.SetActive(false);
        Instantiate(bigSpiritPrefab, stuff.transform);
        StartCoroutine(LoadScene(nextLevel, 3f));
        
    }

    public void lose()
    {
        badSound.Play();
        spirit1.SetActive(false);
        spirit2.SetActive(false);
        allLineDrawers.SetActive(false);
        StartCoroutine(LoadScene(thisLevel, 1.0f));
        
    }

    IEnumerator LoadScene(string name, float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            //fade black
            blackscreen.color = new Color(0, 0, 0, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(stuff);

        SceneManager.LoadScene(name);
    }
}
