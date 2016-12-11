﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AsteroidCollision : MonoBehaviour
{
    public GameObject exitParticleSystem;
    public float FadeTimeIn = 0.2f;
    public float FadeTimeOut = 0.2f;
    public float voidRadius = 1f;
    GameObject TopLayer;
    GameObject InnerLayer;
    Eraser eraser;
    MeshRenderer rend;
    float opacity = 1.0f;
    public float rotationSpeed;

    void Start()
    {
        TopLayer = transform.GetChild(0).gameObject;
        rend = TopLayer.GetComponent<MeshRenderer>();
        InnerLayer = transform.GetChild(1).gameObject;
        eraser = InnerLayer.GetComponent<Eraser>();
        rotationSpeed = Random.Range(-rotationSpeed, rotationSpeed);

        //checking nearest asteroids
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, voidRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GameObject temp = hitColliders[i].gameObject;
            if (temp.name.Contains("Layer") && temp.transform.parent.gameObject != this.gameObject)
                Destroy(temp.transform.parent.gameObject);
            i++;
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + rotationSpeed * Time.deltaTime );
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        DOTween.To(value => opacity = value, 1, 0, FadeTimeIn).OnUpdate(() => { rend.material.SetFloat("_Opacity", opacity); });
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.name == "Player")
        eraser.Erase(col.gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Vector3 direction = col.transform.position - transform.position;
            Instantiate(exitParticleSystem, col.transform.position, Quaternion.LookRotation(direction));
            DOTween.To(value => opacity = value, 0, 1, FadeTimeOut).OnUpdate(() => { rend.material.SetFloat("_Opacity", opacity); });
        }
    }
}
