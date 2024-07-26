using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
	private enum ChunkShape
	{
		None,
		TopRight,
		BottomRight,
		BottomLeft,
		TopLeft,
		Top,
		Right,
		Bottom,
		Left,
		Four
	}

	[Header(("Elements"))]
	[SerializeField] private Transform world;
	Chunk[,] grid;

	[Header("Settings")]
	[SerializeField] private int gridSize;
	[SerializeField] private int gridScale;

	[Header("Data")]
	private WorldData worldData;
	private string dataPath;
	private bool shouldSave;

	[Header("Chunk Meshes")]
	[SerializeField] private Mesh[] chunkShapes;


	private void Awake()
	{
		Chunk.onUnlock += ChunkUnlokedCallback;
		Chunk.onPriceChanged += ChunkPriceChangedCallback;
	}
	private void Start()
	{
		dataPath = Application.persistentDataPath + "/WorldData.txt";
		LoadWorld();
		Initialize();

		InvokeRepeating("TrySaveGame", 1, 1);
	}

	private void OnDestroy()
	{
		Chunk.onUnlock -= ChunkUnlokedCallback;
		Chunk.onPriceChanged -= ChunkPriceChangedCallback;
	}

	private void Initialize()
	{
		for (int i = 0; i < world.childCount; i++)
		{
			world.GetChild(i).GetComponent<Chunk>().Initialize(worldData.chunkPrices[i]);
		}

		InitializeGrid();

		UpdateChunkWalls();

		UpdateGridRenderers();
	}

	private void InitializeGrid()
	{
		grid = new Chunk[gridSize, gridSize];

		for (int i = 0; i < world.childCount; i++)
		{
			Chunk chunk = world.GetChild(i).GetComponent<Chunk>();

			Vector2Int chunkGridPosition = new Vector2Int((int)chunk.transform.position.x / gridScale,
														  (int)chunk.transform.position.z / gridScale);

			chunkGridPosition += new Vector2Int(gridScale / 2, gridSize / 2);

			grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
		}
	}

	private void UpdateChunkWalls()
	{
		// loop along the x axis
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			//loop along the z axis
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				Chunk chunk = grid[x, y];

				if (chunk == null)
				{
					continue;
				}

				Chunk topChunk = null;
				if (IsValiGridPosition(x, y + 1))
					topChunk = grid[x, y + 1];

				Chunk rigthChunk = null;
				if (IsValiGridPosition(x + 1, y))
					rigthChunk = grid[x + 1, y];

				Chunk bottomChunk = null;
				if (IsValiGridPosition(x, y - 1))
					bottomChunk = grid[x, y - 1];

				Chunk leftChunk = null;
				if (IsValiGridPosition(x - 1, y))
					leftChunk = grid[x - 1, y];

				int configuration = 0;

				if (topChunk != null && topChunk.IsUnlocked())
					configuration = configuration + 1;

				if (rigthChunk != null && rigthChunk.IsUnlocked())
					configuration = configuration + 2;

				if (bottomChunk != null && bottomChunk.IsUnlocked())
					configuration = configuration + 4;

				if (leftChunk != null && leftChunk.IsUnlocked())
					configuration = configuration + 8;

				//we know the configuration of the chunk
				chunk.UpdateWalls(configuration);

				SetChunkRenderer(chunk, configuration);
			}
		}
	}

	private void SetChunkRenderer(Chunk chunk, int configuration)
	{


		switch (configuration)
		{
			case 0:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.Four]);
					break;
				}

			case 1:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.Bottom]);					
					break;
				}

			case 2:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.Left]);
					break;
				}

			case 3:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomLeft]);
					break;
				}

			case 4:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.Top]);
					break;
				}

			case 5:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
					break;
				}

			case 6:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopLeft]);
					break;
				}

			case 7:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
					break;
				}

			case 8:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.Right]);
					break;
				}

			case 9:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.BottomRight]);
					break;
				}

			case 10:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
					break;
				}

			case 11:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
					break;
				}

			case 12:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.TopRight]);
					break;
				}

			case 13:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
					break;
				}

			case 14:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
					break;
				}

			case 15:
				{
					chunk.SetRenderer(chunkShapes[(int)ChunkShape.None]);
					break;
				}


		}
	}

	private void UpdateGridRenderers()
	{
		// loop along the x axis
		for (int x = 0; x < grid.GetLength(0); x++)
		{
			//loop along the z axis
			for (int y = 0; y < grid.GetLength(1); y++)
			{
				Chunk chunk = grid[x, y];

				if (chunk == null)
					continue;

				if (chunk.IsUnlocked())
					continue;

				Chunk topChunk = IsValiGridPosition(x, y + 1) ? grid[x, y + 1] : null;
				Chunk rightChunk = IsValiGridPosition(x + 1, y) ? grid[x + 1, y] : null;
				Chunk bottomChunk = IsValiGridPosition(x, y - 1) ? grid[x, y - 1] : null;
				Chunk leftChunk = IsValiGridPosition(x - 1, y) ? grid[x - 1, y] : null;

				if (topChunk != null && topChunk.IsUnlocked())
					chunk.DisplayLockedElements();
				else if(rightChunk != null && rightChunk.IsUnlocked())
					chunk.DisplayLockedElements();
				else if (bottomChunk != null && bottomChunk.IsUnlocked())
					chunk.DisplayLockedElements();
				else if (leftChunk != null && leftChunk.IsUnlocked())
					chunk.DisplayLockedElements();
			}
		}
	}

	private bool IsValiGridPosition(int x, int y)
	{
		if (x < 0 || x >= gridSize || y < 0 || y >= gridSize)
		{
			return false;
		}
		return true;
	}
	private void TrySaveGame()
	{
		if (shouldSave)
		{
			SaveWorld();
			shouldSave = false;
		}
	}

	private void ChunkUnlokedCallback()
	{
		UpdateChunkWalls();

		UpdateGridRenderers();

		SaveWorld();
	}

	private void ChunkPriceChangedCallback()
	{
		//Debug.Log("chunk price changed");
		shouldSave = true;
	}

	public void LoadWorld()
	{

		if (File.Exists(dataPath))
		{
			// Đọc dữ liệu từ tệp
			string data = File.ReadAllText(dataPath);
			worldData = JsonUtility.FromJson<WorldData>(data);
			if (worldData.chunkPrices.Count < world.childCount)
			{
				UpdateData();
			}
		}
		else
		{
			// Tạo mới dữ liệu nếu tệp không tồn tại
			worldData = new WorldData();

			for (int i = 0; i < world.childCount; i++)
			{
				int chunkInitialPrice = world.GetChild(i).GetComponent<Chunk>().GetInitialPrice();
				worldData.chunkPrices.Add(chunkInitialPrice);
			}

			SaveFileText(); // Lưu dữ liệu mới tạo vào tệp
		}
	}

	private void SaveFileText()
	{

		string worldDataString = JsonUtility.ToJson(worldData, true);

		using (FileStream fs = new FileStream(dataPath, FileMode.Create))
		{
			byte[] worldDataBytes = Encoding.UTF8.GetBytes(worldDataString);
			fs.Write(worldDataBytes);
		}

		Debug.Log("World data saved: " + worldDataString);
	}

	private void UpdateData()
	{
		int missingData = world.childCount - worldData.chunkPrices.Count;

		for (int i = 0; i < missingData; i++)
		{
			int chunkIndex = world.childCount - missingData + i;
			int chunkPrice = world.GetChild(chunkIndex).GetComponent<Chunk>().GetInitialPrice();
			worldData.chunkPrices.Add(chunkPrice);
		}
	}

	private void SaveWorld()
	{
		if (worldData.chunkPrices.Count != world.childCount)
		{
			worldData = new WorldData();
		}

		for (int i = 0; i < world.childCount; i++)
		{
			int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().GetCurrentPrice();

			if (worldData.chunkPrices.Count > i)
			{
				worldData.chunkPrices[i] = chunkCurrentPrice;
			}
			else
			{
				worldData.chunkPrices.Add(chunkCurrentPrice);
			}
		}

		string data = JsonUtility.ToJson(worldData, true);

		File.WriteAllText(dataPath, data);

		Debug.LogWarning("Data saved");
	}
}
