using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [Header("Scenes")]
    public List<int> Scenes;

    [Header("Selected Scene Index")]
    public int randomIndex;

    [Header("Number of Rounds")]
    public int NumberRounds = 2;
    public int currentRound = 1;


    void Awake()
    {
        DontDestroyOnLoad(this);
        CreateList();
    }

    private void Start()
    {
        if (Scenes.Count > 0) { Shuffle(); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    public void Shuffle()
    {
        System.Random random = new System.Random();
        randomIndex = random.Next(Scenes.Count);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(Scenes[randomIndex]);
        Scenes.RemoveAt(randomIndex);
    }

    public void CreateList()
    {
        Scenes = Enumerable.Range(1, SceneManager.sceneCountInBuildSettings - 3).ToList();
    }

    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
