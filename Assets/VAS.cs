using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VAS : MonoBehaviour
{
    [Header("VAS Toggle Scales")]
    public ToggleGroup[] toggles;

    [Header("VAS Gameobjects")]
    public GameObject[] VAS_Items;

    [Header("Next Button")]
    public Button NextButton;

    public string[] answers = new string[4];

    private int currentToggle = 0;

    Toggle selected = null;

    private Manager Manager;


    void Start()
    {
        Manager = FindObjectOfType<Manager>();

        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].allowSwitchOff = true;

            foreach(Toggle toggle in toggles[i].GetComponentsInChildren<Toggle>()) 
            {
                toggle.onValueChanged.AddListener(OnToggleChanged);
            }
        }

        NextButton.interactable = false;
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn) 
        {
            selected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
            NextButton.interactable = true;
        }
        else
            NextButton.interactable = false;
    }

    public void Submit()
    {
        if(currentToggle < VAS_Items.Length - 1)
        {
            answers[currentToggle] = selected.name;
            VAS_Items[currentToggle].SetActive(false);
            currentToggle++;
            VAS_Items[currentToggle].SetActive(true);
        }
        else
        {
            answers[currentToggle] = selected.name;
            Manager.VAS_answers = answers;
            Manager.WriteData();
            Manager.ChangeScene();
        }
    }
}
