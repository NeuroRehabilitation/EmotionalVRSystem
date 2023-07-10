using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VAS : MonoBehaviour
{
    [Header("VAS Scales")]
    public ToggleGroup AngerToggles;
    public ToggleGroup FearToggles;
    public ToggleGroup JoyToggles;
    public ToggleGroup SadToggles;

    private string AngerRating;
    private string FearRating;
    private string JoyRating;
    private string SadRating;

    // Start is called before the first frame update
    void Start()
    {

    }
}
