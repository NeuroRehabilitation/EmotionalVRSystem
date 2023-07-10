using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public List<int> Scenes;
    public int randomIndex;

    void Awake()
    {
        DontDestroyOnLoad(this);
        CreateList();
    }

    private void Start()
    {
        if (Scenes.Count > 0) { Shuffle(); }
        
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
        Scenes = Enumerable.Range(1, SceneManager.sceneCountInBuildSettings - 1).ToList();
    }
}
