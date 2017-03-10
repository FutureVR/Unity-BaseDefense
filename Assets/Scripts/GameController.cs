using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour {

    bool walking = true;

    public GameObject fpsCont;
    public GameObject orthView;

    public GameObject enemy;
    public GameObject terrain;
    Vector3 terrainScale;
    Person person;

    public float initialEnemyCount;

    List<WoodPile> woodPiles = new List<WoodPile>();
    List<MetalPile> metalPiles = new List<MetalPile>();
    public int numOfWoodPiles = 10;
    public int numOfMetalPiles = 10;
    public float pileDistMultiplier = 3;
    public float minResourceAmt = 50;
    public float maxResourceAmt = 100;

    public GameObject woodPile;
    public GameObject metalPile;

    void Start ()
    {
        person = fpsCont.GetComponentInChildren<Person>();
        terrainScale = terrain.transform.localScale * 5;

        if (walking == false) Time.timeScale = 0;
        //setOrtho(false);
        //setFPS(true);
        fpsCont.SetActive(walking);
        orthView.SetActive(!walking);

        spawnPiles();
        spawnEnemies();
	}

    void Update()
    {
        if (Input.GetButtonDown("SwitchCam")) switch_cam_on_input();

        if (Time.time % 10 == 0  &&  Time.timeScale != 0) spawnObjectRandomly(enemy);
    }

    void spawnEnemies()
    {
        for (int i = 0; i < initialEnemyCount; i++) spawnObjectRandomly(enemy);
    }

    //TODO: wood and metal should be spawned differently
    void spawnPiles()
    {
        for (int i = 0; i < numOfWoodPiles; i++)
        {
            GameObject newGO = spawnObjectRandomly(woodPile);
            newGO.GetComponent<ResourcePile>().resourceAmt = Random.Range(minResourceAmt, maxResourceAmt);
        }
        for (int i = 0; i < numOfMetalPiles; i++)
        {
            GameObject newGO = spawnObjectRandomly(metalPile);
            newGO.GetComponent<ResourcePile>().resourceAmt = Random.Range(minResourceAmt, maxResourceAmt);
        }
    }

    GameObject spawnObjectRandomly(GameObject go)
    {
        float x_pos = Random.Range(-terrainScale.x, terrainScale.x);
        float z_pos = Random.Range(-terrainScale.x, terrainScale.x);
        Vector3 spawnPos = new Vector3(x_pos, fpsCont.transform.localScale.y / 2, z_pos);

        GameObject newGO = Instantiate(go);
        newGO.transform.position = spawnPos;

        return newGO;
    }



    void switch_cam_on_input()
    {
        //Switch from walking to overview
        if (walking == true)
        {
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //setOrtho(true);
            //setFPS(false);

            walking = false;

            fpsCont.SetActive(false);
            orthView.SetActive(true);
        }
        //Switch from overview to walking 
        else
        {
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //setOrtho(false);
            //setFPS(true);

            walking = true;

            fpsCont.SetActive(true);
            orthView.SetActive(false);
        }
    }

    void setOrtho(bool b)
    {
        orthView.GetComponent<Camera>().enabled = b;
        orthView.GetComponent<PlaceBlocks>().enabled = b;
        orthView.GetComponent<AudioListener>().enabled = b;
    }

    void setFPS(bool b)
    {
        fpsCont.GetComponentInChildren<Camera>().enabled = b;
        fpsCont.GetComponentInChildren<AudioListener>().enabled = b;
        fpsCont.GetComponent<FirstPersonController>().enabled = b;
    }
}
