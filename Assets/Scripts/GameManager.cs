using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private List<GameObject> instancesOfPrefab;

    private bool isChestSpawned;

    public GameObject coin;

    public int score;

    public Vector2 spawnPosition;

    public GameObject chest;
    private void Start()
    {
        spawnPosition = new Vector2(0f, -0.25f);
        isChestSpawned = false;

        score = 0;

        CountPrefabs();
    }
    public void IncreaseScore()
    {
        score += 50;
    }
    private void Update()
    {
        CountPrefabs();

        if (instancesOfPrefab.Count <= 0 && !isChestSpawned)
        {
            Instantiate(chest, spawnPosition, Quaternion.identity);
            isChestSpawned = true;
        }
    }

    public void CountPrefabs() {

        // Encuentra todos los objetos instanciados en la escena
        GameObject[] instances = GameObject.FindObjectsOfType<GameObject>();

        // Filtra las instancias para encontrar aquellas que tengan el prefab asociado
        instancesOfPrefab = new List<GameObject>();
        foreach (GameObject instance in instances)
        {
            if (instance.name == coin.name && instance != coin)
            {
                instancesOfPrefab.Add(instance);
            }
        }
    }

    public void GameOver()
    {
        // Reboot the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
