using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private Manager Manager;

    void Start()
    {
        Manager = FindObjectOfType<Manager>();

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
    }

    public void Submit()
    {
        if (currentToggle >= 1)
        {
            Manager.SAM_answers = answers;
            SceneManager.LoadScene("VAS");
        }
            
        if(currentToggle < SAM_Items.Length-1) 
        {
            answers[currentToggle] = selected.name;
            SAM_Items[currentToggle].SetActive(false);
            currentToggle++;
            SAM_Items[currentToggle].SetActive(true);
        }
    }
}
