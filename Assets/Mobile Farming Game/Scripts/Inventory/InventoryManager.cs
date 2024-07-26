/*
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
	private Inventory inventory;
	private InventoryDisplay inventoryDisplay;
	private string dataPath;

	// Start is called before the first frame update
	void Start()
	{
		dataPath = Application.persistentDataPath + "/InventoryData.txt";

		LoadInventory();
		ConfigureInventoryDisplay();

		CropTile.onCropHarvested += CropHarvestedCallback;

		AppleTree.onAppleHarvested += CropHarvestedCallback;
	}

	private void OnDestroy()
	{
		CropTile.onCropHarvested -= CropHarvestedCallback;

		AppleTree.onAppleHarvested -= CropHarvestedCallback;
	}

	private void ConfigureInventoryDisplay()
	{
		inventoryDisplay = GetComponent<InventoryDisplay>();
		inventoryDisplay.Configure(inventory);
	}

	private void CropHarvestedCallback(CropType cropType)
	{
		// Update our inventory
		inventory.CropHarvestedCallback(cropType);

		inventoryDisplay.UpdateDisplay(inventory);

		SaveInventory();
	}

	[NaughtyAttributes.Button]
	public void ClearInventory()
	{
		inventory.Clear();
		inventoryDisplay.UpdateDisplay(inventory);

		SaveInventory();
	}

	public Inventory GetInventory()
	{
		return inventory;
	}

	private void LoadInventory()
	{
		string data = "";


		if (File.Exists(dataPath))
		{
			data = File.ReadAllText(dataPath);
			inventory = JsonUtility.FromJson<Inventory>(data);

			if (inventory == null)
				inventory = new Inventory();
		}
		else
		{
			File.Create(dataPath);
			inventory = new Inventory();
		}
	}

	private void SaveInventory()
	{
		string data = JsonUtility.ToJson(inventory, true);
		File.WriteAllText(dataPath, data);
	}
}
*/


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
	private Inventory inventory;
	private InventoryDisplay inventoryDisplay;
	private string dataPath;

	// Start is called before the first frame update
	void Start()
	{
		dataPath = Application.persistentDataPath + "/InventoryData.txt";

		LoadInventory();
		ConfigureInventoryDisplay();

		CropTile.onCropHarvested += CropHarvestedCallback;
		AppleTree.onAppleHarvested += CropHarvestedCallback;
	}

	private void OnDestroy()
	{
		CropTile.onCropHarvested -= CropHarvestedCallback;
		AppleTree.onAppleHarvested -= CropHarvestedCallback;
	}

	private void ConfigureInventoryDisplay()
	{
		inventoryDisplay = GetComponent<InventoryDisplay>();
		inventoryDisplay.Configure(inventory);
	}

	private void CropHarvestedCallback(CropType cropType)
	{
		// Update our inventory
		inventory.CropHarvestedCallback(cropType);
		inventoryDisplay.UpdateDisplay(inventory);
		SaveInventory();
	}

	[NaughtyAttributes.Button]
	public void ClearInventory()
	{
		inventory.Clear();
		inventoryDisplay.UpdateDisplay(inventory);
		SaveInventory();
	}

	public Inventory GetInventory()
	{
		return inventory;
	}

	private void LoadInventory()
	{
		if (File.Exists(dataPath))
		{
			try
			{
				using (StreamReader reader = new StreamReader(dataPath))
				{
					string data = reader.ReadToEnd();
					inventory = JsonUtility.FromJson<Inventory>(data);
					if (inventory == null)
						inventory = new Inventory();
				}
			}
			catch (IOException ex)
			{
				Debug.LogError("IOException while reading file: " + ex.Message);
				inventory = new Inventory();
			}
		}
		else
		{
			inventory = new Inventory();
		}
	}

	private void SaveInventory()
	{
		try
		{
			using (StreamWriter writer = new StreamWriter(dataPath, false))
			{
				string data = JsonUtility.ToJson(inventory, true);
				writer.Write(data);
			}
		}
		catch (IOException ex)
		{
			Debug.LogError("IOException while writing to file: " + ex.Message);
		}
	}
}
