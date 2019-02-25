using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public Planet[] planets;
    public GameObject comet, asteroid;
    public Camera camera;
    float tempBase = 0;
    public ParticleSystem sunGlow;
    ParticleSystem.MainModule glow;
    float spawnRate, nextSpawnIncrease, spawnIncreaseRate = 10;
    float timer;
    float[][] planetSettings;
    List<GameObject> spaceRocks = new List<GameObject>();
    bool init = false;

    void Start()
    {
        planetSettings = new float[7][];
        for (int i = 0; i < planets.Length; i++)
        {
            planetSettings[i] = new float[3];
            planetSettings[i][0] = planets[i].water;
            planetSettings[i][1] = planets[i].nutrient;
            planetSettings[i][2] = planets[i].temp;
        }
        glow = sunGlow.main;
        instance = this;
        Reset();
        init = true;
    }

    void Update()
    {
        if (nextSpawnIncrease < Time.time)
        {
            nextSpawnIncrease = Time.time + spawnIncreaseRate;
            spawnRate *= 1.01f;
        }
        if (Random.value < spawnRate)
        {
            spaceRocks.Add(Instantiate(comet, new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0).normalized * 10f, Quaternion.identity));
        }
        if (Random.value < spawnRate)
        {
            spaceRocks.Add(Instantiate(asteroid, new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0).normalized * 10f, Quaternion.identity));
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Planet p = hit.transform.GetComponent<Planet>();
                if (p)
                {
                    p.Click();
                    return;
                }
                SpaceRock sr = hit.transform.GetComponent<SpaceRock>();
                if (sr)
                {
                    sr.Click();
                    return;
                }
                Clickable c = hit.transform.GetComponent<Clickable>();
                if (c)
                {
                    c.Click();
                    return;
                }
            }
        }
    }
    public void RaiseHeat()
    {
        if (tempBase < 0.3f)
        {
            foreach (Planet p in planets)
            {
                p.temp += 0.1f;
            }
            tempBase += 0.1f;
            glow.startSpeed = tempBase * 3 + 1;
        }
    }
    public void LowerHeat()
    {
        if (tempBase > -0.2f)
        {
            foreach (Planet p in planets)
            {
                p.temp -= 0.1f;
            }
            tempBase -= 0.1f;
            glow.startSpeed = tempBase * 3 + 1;
        }
    }
    void Reset()
    {
        foreach (GameObject go in spaceRocks)
        {
            if (go)
            {
                Destroy(go);
            }
        }
        spaceRocks = new List<GameObject>();
        nextSpawnIncrease = Time.time + spawnIncreaseRate;
        spawnRate = 0.01f;
        timer = Time.time;
        tempBase = 0;
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].water = planetSettings[i][0];
            planets[i].nutrient = planetSettings[i][1];
            planets[i].temp = planetSettings[i][2];
            planets[i].life = 0;
        }
        if (init)
        {
            glow.startSpeed = tempBase * 3 + 1;
        }
    }

    public void GameOver()
    {
        GetComponent<AudioSource>().Play();
        Reset();
    }
}
