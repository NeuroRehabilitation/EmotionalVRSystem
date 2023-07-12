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

    public CSV CSV_writer;
    public string[] SAM_answers;
    public string[] VAS_answers;
    public string[] DataToSave;

    void Awake()
    {
        DontDestroyOnLoad(this);
        CreateList();

        CSV_writer = GetComponent<CSV>();
    }

    private void Start()
    {
        if (Scenes.Count > 0) { Shuffle(); }

        CSV_writer.AddData("Scene", "Valence", "Arousal", "Anger", "Fear", "Joy", "Sad");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || currentRound > NumberRounds)
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

    public void WriteData()
    {
        DataToSave = SAM_answers.Concat(VAS_answers).ToArray();
        CSV_writer.AddData(DataToSave);
    }

    public void ChangeScene()
    {
        if (Scenes.Count > 0)
        {
            Shuffle();
            LoadScene();
        }
        else
        {
            currentRound++;
            CreateList();
            Shuffle();
            LoadScene();
        }
    }

    public void Quit()
    {
        CSV_writer.Save("test.csv");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
