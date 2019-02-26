using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    public MeshRenderer clouds, ocean, land;
    public Material red, yellow, blue, green, white, black;
    public Image lifeTrack, tempTrack, waterTrack, nutrientTrack;
    public float temp, water, nutrient, life;
    public GameObject lifeWarning;
    float checkRate = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        CheckState();
        InvokeRepeating("CheckState", Random.value, checkRate);
    }

    void CheckState()
    {
        clouds.gameObject.SetActive(life > 0.5f);
        float lifeAdd = 0;
        if (temp > 0.25f && temp < 0.75f)
        {
            if (water > 0.5f)
            {
                clouds.material = white;
                ocean.material = blue;
            }
            else
            {
                clouds.material = yellow;
                ocean.material = yellow;
            }
            if (nutrient > 0.5f)
            {
                land.material = green;
            }
            else
            {
                land.material = yellow;
            }
            lifeAdd += water * nutrient / 5;
        }
        else if (temp > 0.5f)
        {
            if (water - 0.02f * checkRate < 0)
            {
                water = 0;
            }
            else
            {
                water -= 0.02f * checkRate;
            }
            if (water > 0.5f)
            {
                clouds.material = white;
                ocean.material = yellow;
            }
            else
            {
                lifeAdd -= 0.01f;
                clouds.material = red;
                ocean.material = red;
            }
            if (nutrient > 0.5f)
            {
                land.material = yellow;
            }
            else
            {
                lifeAdd -= 0.01f;
                land.material = red;
            }
        }
        else
        {
            if (water > 0.5f)
            {
                clouds.material = white;
                ocean.material = white;
            }
            else
            {
                lifeAdd -= 0.01f;
                clouds.gameObject.SetActive(false);
                ocean.material = black;
            }
            if (nutrient > 0.5f)
            {
                land.material = yellow;
            }
            else
            {
                lifeAdd -= 0.01f;
                land.material = black;
            }
        }
        if (life + lifeAdd * checkRate < 0)
        {
            life = 0;
        }
        else
        {
            if (life < 0.75f && life + lifeAdd * checkRate > 0.75f)
            {
                lifeWarning.SetActive(false);
                lifeWarning.SetActive(true);
            }
            life += lifeAdd * checkRate;
        }
        lifeTrack.fillAmount = life;
        tempTrack.fillAmount = temp;
        waterTrack.fillAmount = water;
        nutrientTrack.fillAmount = nutrient;
        if (life >= 1)
        {
            Manager.instance.GameOver();
        }
    }

    public void Click()
    {
        if (nutrient >= 0.01f)
        {
            nutrient -= 0.01f;
        }
    }
}
