using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResetGameButton : MonoBehaviour
{
	public Button resetButton;
	public TextMeshProUGUI buttonText;

	private string inventoryDataPath;
	private string worldDataPath;

	void Start()
	{
		inventoryDataPath = Application.persistentDataPath + "/InventoryData.txt";
		worldDataPath = Application.persistentDataPath + "/WorldData.txt";

		// Gán sự kiện click cho nút
		if (resetButton != null)
		{
			resetButton.onClick.AddListener(ResetGame);
		}

		if (buttonText != null)
		{
			buttonText.text = "Reset Game";
		}
	}

	public void ResetGame()
	{
		Debug.Log("Click");
		if (File.Exists(inventoryDataPath))
		{
			File.Delete(inventoryDataPath);
			Debug.Log("Deleted: " + inventoryDataPath);
		}
		else
		{
			Debug.Log("File not found: " + inventoryDataPath);
		}

		if (File.Exists(worldDataPath))
		{
			File.Delete(worldDataPath);
			Debug.Log("Deleted: " + worldDataPath);
		}
		else
		{
			Debug.Log("File not found: " + worldDataPath);
		}

		// Reload the scene to restart the game
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}
}
