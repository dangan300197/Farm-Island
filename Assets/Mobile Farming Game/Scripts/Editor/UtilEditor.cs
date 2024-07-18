using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class UtilEditor 
{
    [MenuItem("Game/ClearData")]
    public static void ClearData()
    {
		DeleteWorldDataFile("WorldData.txt");
		DeleteWorldDataFile("InventoryData.txt");

	}

	public static void DeleteWorldDataFile(string fileName)
	{
		string filePath = Application.persistentDataPath + "/"+fileName;

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
