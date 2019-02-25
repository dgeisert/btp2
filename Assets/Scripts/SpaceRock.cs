using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRock : MonoBehaviour
{
    public GameObject deathParticles;
    public bool is_comet = true;
    public float speed = 100;
    public Rigidbody rb;
    float step = 0.1f;

    void Update()
    {
        Vector3 move = Vector3.zero;
        foreach (Planet p in Manager.instance.planets)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            move += (p.transform.position - transform.position)
            * p.transform.localScale.x / dist / dist;
        }
        rb.AddForce(move * speed);
        transform.LookAt(transform.position + rb.velocity);
    }
    void OnCollisionEnter(Collision col)
    {
        Planet p = col.collider.GetComponent<Planet>();
        if (p != null)
        {
            if (!is_comet)
            {
                if (p.nutrient + step > 1)
                {
                    p.nutrient = 1;
                }
                else
                {
                    p.nutrient += step;
                }
            }
            if (!is_comet)
            {
                if (p.water + step > 1)
                {
                    p.water = 1;
                }
                else
                {
                    p.water += step;
                }
            }
            DestroyThis();
        }
        Destroy(gameObject);
    }

    public void Click()
    {
        DestroyThis();
    }

    void DestroyThis()
    {
        deathParticles.transform.SetParent(null);
        deathParticles.SetActive(true);
        Destroy(deathParticles, 5);
        Destroy(gameObject);
    }
}
