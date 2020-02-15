using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateContinuously : MonoBehaviour
{
    public float offset = 0f;
    public float speed = 10;



    float myTime;
    private void Start()
    {
        myTime = 0f;


    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, offset + myTime * speed, 0);
        myTime += Time.deltaTime;
    }
}
