using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;
using System.Text;

public class PlayerToolSelector : MonoBehaviour
{
	public enum Tool { None, Sow, Water, Harvest, Reset }
	private Tool activeTool;

	[Header(" Elements ")]
	[SerializeField] private Image[] toolImages;

	[Header(" Settings ")]
	[SerializeField] private Color selectedToolColor;

	[Header(" Actions ")]
	public Action<Tool> onToolSelected;


	// Start is called before the first frame update
	void Start()
	{
		SelectTool(0);
	}

	public void SelectTool(int toolIndex)
	{
		activeTool = (Tool)toolIndex;
		Debug.Log(toolIndex);

		for (int i = 0; i < toolImages.Length; i++)
		{
			toolImages[i].color = i == toolIndex ? selectedToolColor : Color.white;
			
		}
		if (toolIndex == 4)
		{
			ResetGame();
		}
		onToolSelected?.Invoke(activeTool);
	}

	public bool CanSow()
	{
		return activeTool == Tool.Sow;
	}

	public bool CanWater()
	{
		return activeTool == Tool.Water;
	}

	public bool CanHarvest()
	{	
		return activeTool == Tool.Harvest;
	}
	public bool CanReset()
	{
		return activeTool == Tool.Reset;
	}

	public void ResetGame()
	{
		DeleteWorldDataFile("WorldData.txt");
		DeleteWorldDataFile("InventoryData.txt");

		// Reload the scene to restart the game
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}
	

	public void DeleteWorldDataFile(string fileName)
	{
		string filePath = Application.persistentDataPath + "/" + fileName;

		if (File.Exists(filePath))
		{
			File.Delete(filePath);
			Debug.Log("File deleted: " + filePath);
		}
		else
		{
			Debug.LogWarning("File not found: " + filePath);
		}
	}

}
