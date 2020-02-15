using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shakey : MonoBehaviour
{
    int way = 1;
    public float dist = 1;
    public Vector3 direction = Vector3.up;
    public float time = 1;
    public float elapsed = 0;
    public bool stunt = false;

    Vector3 dir;

    void Update()
    {
        if(elapsed < time)
        {
            if (way == 1 || !stunt)
            {
                transform.localPosition = transform.localPosition + dist * direction.normalized * way;
            }
            elapsed += Time.deltaTime;
        }
        else
        {
            way *= -1;
            elapsed = 0;
        }
    }
}
