using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShowAmount : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private int sliderValue;
    private int currentValue = 0;
    private TMP_Text amountText;

    private void Start()
    {
        amountText = gameObject.GetComponent<TMP_Text>();    
    }
    // Update is called once per frame
    void Update()
    {
        sliderValue = (int)slider.value;
        if (sliderValue != currentValue)
        {
            currentValue = sliderValue;
            amountText.text = currentValue.ToString();
        }
    }
}
