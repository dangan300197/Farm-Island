using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CashManager : MonoBehaviour
{
	public static CashManager instance;
	[Header("Settings")]
	private int coins;


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		ResetCoins(); // Reset coins về 400 khi bắt đầu game dùng để mở các ô đất
		LoadData();
		UpdateCoinContainers();
	}

	public void AddCoins(int amount)
	{
		coins += amount;
		UpdateCoinContainers();
		Debug.Log("Coins updated: " + coins);

		SaveData();
	}

	private void UpdateCoinContainers()
	{
		GameObject[] coinContainers = GameObject.FindGameObjectsWithTag("CoinAmount");

		foreach (GameObject coinContainer in coinContainers)
		{
			//coinContainer.GetComponent<TextMeshProUGUI>().text = coins.ToString();
			TextMeshProUGUI textComponent = coinContainer.GetComponent<TextMeshProUGUI>();
			if (textComponent != null)
			{
				textComponent.text = coins.ToString();
			}
			else
			{
				Debug.LogError("Coin container does not have a TextMeshProUGUI component");
			}
		}
	}

	private void ResetCoins()
	{
		coins = 400;
		SaveData();
	}
	public void UseCoin(int amount)
	{
		AddCoins(-amount);
	}

	public int GetCoins()
	{
		return coins;
	}

	[NaughtyAttributes.Button]
	private void Add500Coins()
	{
		AddCoins(500);
	}

	private void LoadData()
	{
		coins = PlayerPrefs.GetInt("Coins");
	}

	private void SaveData()
	{
		PlayerPrefs.SetInt("Coins", coins);
	}

}
