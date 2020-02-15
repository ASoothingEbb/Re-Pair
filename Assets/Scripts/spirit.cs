using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spirit : MonoBehaviour
{
    public float towardSpeed = 0.2f;
    GameManager manager;

    public void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetButton("toward") && manager.started)
        {
            transform.localPosition = transform.localPosition + new Vector3(0, 0, -towardSpeed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bad"))
        {
            manager.lose();
        }else if (other.CompareTag("spirit"))
        {
            manager.win();
        }
    }
}
