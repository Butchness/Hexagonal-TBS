using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MapSpawner : Node3D
{
	public PackedScene HexTileScene = (PackedScene)ResourceLoader.Load("res://scenes/tile.tscn"); // Reference to the hex tile scene

	public float TileSpacing = 1.01f; // Space between tiles

	[Export]
	public int GridSize = 3; //map radius

	private Dictionary<Tile.TileType, List<Tile.TileType>> adjacencyRules;
	private Dictionary<Tile.TileType, float> tileTypeProbabilities;

	public override void _Ready()
	{
		// Initialize adjacency rules and probabilities for the WFC algorithm
		InitializeAdjacencyRules();
		InitializeTileTypeProbabilities();

		// Check if the HexTileScene is loaded
		if (HexTileScene == null)
		{
			GD.PrintErr("HexTileScene is not assigned or loaded correctly.");
			return;
		}

		// Start the Wave Function Collapse map generation
		SpawnHexagonalGrid();
	}

	private void InitializeAdjacencyRules()
	{
		adjacencyRules = new Dictionary<Tile.TileType, List<Tile.TileType>>
		{
			{ Tile.TileType.Blank, new List<Tile.TileType> { Tile.TileType.Blank, Tile.TileType.ResourceWood, Tile.TileType.Water, Tile.TileType.Dirt } },
			{ Tile.TileType.ResourceWood, new List<Tile.TileType> { Tile.TileType.Blank, Tile.TileType.Mountain, Tile.TileType.ResourceWood } },
			{ Tile.TileType.Water, new List<Tile.TileType> { Tile.TileType.Blank } },
			{ Tile.TileType.Mountain, new List<Tile.TileType> { Tile.TileType.Mountain, Tile.TileType.ResourceWood } },
			{ Tile.TileType.Dirt, new List<Tile.TileType> { Tile.TileType.Blank, Tile.TileType.DirtLumber} },
			{ Tile.TileType.DirtLumber, new List<Tile.TileType> { Tile.TileType.Dirt, Tile.TileType.DirtLumber } }
		};
	}

	private void InitializeTileTypeProbabilities()
	{
		tileTypeProbabilities = new Dictionary<Tile.TileType, float>
		{
			{ Tile.TileType.Blank, 0.2f },
			{ Tile.TileType.ResourceWood, 0.15f },
			{ Tile.TileType.Water, 0.2f },
			{ Tile.TileType.Mountain, 0.15f },
			{ Tile.TileType.Dirt, 0.25f },
			{ Tile.TileType.DirtLumber, 0.10f }
		};
	}

	private void SpawnHexagonalGrid()
	{
		Dictionary<(int, int), List<Tile.TileType>> grid = new Dictionary<(int, int), List<Tile.TileType>>();

		// Initialize the grid with all possible types for each tile
		for (int q = -GridSize; q <= GridSize; q++)
		{
			int r1 = Math.Max(-GridSize, -q - GridSize);
			int r2 = Math.Min(GridSize, -q + GridSize);
			for (int r = r1; r <= r2; r++)
			{
				grid[(q, r)] = new List<Tile.TileType>((Tile.TileType[])Enum.GetValues(typeof(Tile.TileType)));
			}
		}

		// Collapse the grid starting from the center
		CollapseGrid(grid, (0, 0));

		// Instantiate tiles based on the collapsed grid
		foreach (var tile in grid)
		{
			SpawnTile(tile.Key.Item1, tile.Key.Item2, tile.Value[0]);
		}
	}

private void CollapseGrid(Dictionary<(int, int), List<Tile.TileType>> grid, (int q, int r) start)
{
	Queue<(int q, int r)> queue = new Queue<(int q, int r)>();
	queue.Enqueue(start);
	grid[start] = new List<Tile.TileType> { SelectTileTypeWithProbability(grid[start]) }; // Start with a tile chosen by probability

	while (queue.Count > 0)
	{
		var (q, r) = queue.Dequeue();
		Tile.TileType collapsedType = grid[(q, r)][0]; // Assume only one type left

		// Iterate over all neighbors of (q, r)
		var neighbors = GetNeighbors(q, r);
		foreach (var neighbor in neighbors)
		{
			if (!grid.ContainsKey(neighbor) || grid[neighbor].Count == 1) continue; // Skip if already collapsed

			// Update the list of possible types based on the current tile type and adjacency rules
			var filteredTypes = grid[neighbor].Where(type => adjacencyRules[collapsedType].Contains(type)).ToList();
			grid[neighbor] = filteredTypes;

			if (grid[neighbor].Count > 1)
			{
				grid[neighbor] = new List<Tile.TileType> { SelectTileTypeWithProbability(grid[neighbor]) };
			}

			if (grid[neighbor].Count == 1)
			{
				queue.Enqueue(neighbor);
			}
			else if (grid[neighbor].Count == 0)
			{
				GD.PrintErr($"No valid types found for tile at ({neighbor.q}, {neighbor.r}) after applying adjacency rules.");
			}
		}
	}
}

	private Tile.TileType SelectTileTypeWithProbability(List<Tile.TileType> possibleTypes)
	{
		float totalWeight = possibleTypes.Sum(type => tileTypeProbabilities[type]);
		float randomValue = (float)GD.RandRange(0, totalWeight);
		float cumulativeWeight = 0.0f;

		foreach (var type in possibleTypes)
		{
			cumulativeWeight += tileTypeProbabilities[type];
			if (randomValue < cumulativeWeight)
			{
				return type;
			}
		}

		return possibleTypes[0]; // Default return value in case of rounding errors
	}

	private List<(int q, int r)> GetNeighbors(int q, int r)
	{
		return new List<(int, int)>
		{
			(q + 1, r), (q - 1, r), (q, r + 1), (q, r - 1), (q + 1, r - 1), (q - 1, r + 1)
		};
	}

	private void SpawnTile(int q, int r, Tile.TileType type)
	{
		float tileWidth = 1.0f;  // Assuming the width of a tile is 1 unit (adjust as necessary)
		float tileHeight = 1.0f; // Assuming the height of a tile is 1 unit (adjust as necessary)

		// Convert axial coordinates (q, r) to pixel coordinates
		float posX = TileSpacing * (q + r / 2.0f) * tileWidth;
		float posZ = TileSpacing * r * (float)Math.Sqrt(3) / 2.0f * tileHeight;
		float posY = 0;

		// Instance the tile and set its position
		var hexTile = (Tile)HexTileScene.Instantiate();
		hexTile.Position = new Vector3(posX, posY, posZ);
		AddChild(hexTile);
		hexTile.SetTileType(type);
	}

	public override void _Process(double delta)
	{
	}
}
