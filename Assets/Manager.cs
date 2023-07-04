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
        Scenes = Enumerable.Range(1,SceneManager.sceneCountInBuildSettings-1).ToList();
    }

    private void Start()
    {
        Shuffle();
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
}
