using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public bool active = false;
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
    int panel = 0;
    public GameObject[] introPanels;
    public GameObject back, next, start;
    public TMPro.TextMeshPro tempText, scoreText, gameOverText;
    public Color wrongTemp, rightTemp;

    void Start()
    {
        SetPanel(panel);
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
        if (active)
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
        }
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
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
            else
            {
                Clickable c = hit.transform.GetComponent<Clickable>();
                if (c)
                {
                    c.Hover();
                    return;
                }
            }
        }
    }
    public void RaiseHeat()
    {
        if (tempBase < 0.25f)
        {
            foreach (Planet p in planets)
            {
                p.temp += 0.1f;
            }
            tempBase += 0.1f;
            glow.startSpeed = tempBase * 3 + 1;
            tempText.text = Mathf.Floor(2500 + tempBase * 1000).ToString() + "K";
        }
    }
    public void LowerHeat()
    {
        if (tempBase > -0.15f)
        {
            foreach (Planet p in planets)
            {
                p.temp -= 0.1f;
            }
            tempBase -= 0.1f;
            glow.startSpeed = tempBase * 3 + 1;
            tempText.text = Mathf.Floor(2500 + tempBase * 1000).ToString() + "K";
        }
    }

    public void StartGame()
    {
        foreach (GameObject go in introPanels)
        {
            go.SetActive(false);
        }
        start.SetActive(false);
        next.SetActive(false);
        back.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        panel = 0;
        active = true;
        Reset();
    }
    public void Next()
    {
        panel++;
        SetPanel(panel);
    }
    public void Back()
    {
        panel--;
        SetPanel(panel);
    }
    void SetPanel(int i)
    {
        foreach (GameObject go in introPanels)
        {
            go.SetActive(false);
        }
        back.SetActive(i > 0);
        next.SetActive(i != 3);
        if (i < 0)
        {
            gameOverText.gameObject.SetActive(true);
            scoreText.gameObject.SetActive(true);
            scoreText.text = Mathf.Floor(Time.time - timer).ToString() + " seconds";
        }
        else
        {
            gameOverText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            introPanels[i].SetActive(true);
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
        panel = -1;
        GetComponent<AudioSource>().Play();
        start.SetActive(true);
        SetPanel(panel);
        active = false;
    }
}
