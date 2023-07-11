using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SAM : MonoBehaviour
{
    [Header("SAM Toggle Scales")]
    public ToggleGroup[] toggles;

    [Header("SAM Gameobjects")]
    public GameObject[] SAM_Items;

    [Header("Next Button")]
    public Button NextButton;

    public string[] answers = new string[4];

    private int currentToggle = 0;

    Toggle selected = null;

    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].allowSwitchOff = true;

            foreach (Toggle toggle in toggles[i].GetComponentsInChildren<Toggle>())
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

        Debug.Log(selected);
    }

    public void Submit()
    {
        //if (currentToggle >= 3)

        answers[currentToggle] = selected.name;
        SAM_Items[currentToggle].SetActive(false);
        currentToggle++;
        SAM_Items[currentToggle].SetActive(true);

    }
}
