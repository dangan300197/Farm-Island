using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICropContainer : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;


	public void Configure(Sprite icon, int amount)
	{
		iconImage.sprite = icon;
        amountText.text = amount.ToString();
	}

    public void UpdateDisplay(int amount)
    {
        amountText.text = amount.ToString();
    }
}
