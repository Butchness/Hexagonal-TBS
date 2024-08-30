using Godot;
using System.Collections.Generic;

public partial class Tile : Node3D
{
	private MeshInstance3D meshInstance;

	public enum TileType{
		Blank = 0,
		Building = 1,
		ResourceWood = 2,
		Water = 3,
		Castle = 4,
		Dock = 5,
		Market = 6
		// Add more types as needed
	}

	// Preload meshes for different tile types
	private readonly Dictionary<TileType, Mesh> tileMeshes = new Dictionary<TileType, Mesh>
	{
		{ TileType.Blank, GD.Load<Mesh>("res://meshes/tiles/OBJ format/grass.obj") },
		{ TileType.Building, GD.Load<Mesh>("res://meshes/tiles/OBJ format/building-cabin.obj") },
		{ TileType.ResourceWood, GD.Load<Mesh>("res://meshes/tiles/OBJ format/grass-forest.obj") },
		{ TileType.Water, GD.Load<Mesh>("res://meshes/tiles/OBJ format/water.obj") },
		{ TileType.Castle, GD.Load<Mesh>("res://meshes/tiles/OBJ format/building-castle.obj") },
		{ TileType.Dock, GD.Load<Mesh>("res://meshes/tiles/OBJ format/building-dock.obj") },
		{ TileType.Market, GD.Load<Mesh>("res://meshes/tiles/OBJ format/building-market.obj") },
	};

	[Export]
	public int TileTypeIndex
	{
		get => _tileTypeIndex;
		set
		{
			_tileTypeIndex = value;
			SetTileType((TileType)_tileTypeIndex);
		}
	}
	private int _tileTypeIndex;

	private TileType _currentTileType;

	public override void _Ready()
	{
		// Get the MeshInstance3D node
		meshInstance = GetNode<MeshInstance3D>("primaryTileModel");

		// Initialize the tile with the selected type
		SetTileType((TileType)_tileTypeIndex);
	}

	public void SetTileType(TileType type)
	{
		_currentTileType = type;

		// Debugging output
		GD.Print("Setting tile type: " + type.ToString());

		// Assign the appropriate mesh to the MeshInstance3D
		if (tileMeshes.ContainsKey(type))
		{
			Mesh mesh = tileMeshes[type];
			if (mesh != null)
			{
				meshInstance.Mesh = mesh;
				GD.Print("Mesh assigned successfully for tile type: " + type.ToString());
			}
			else
			{
				GD.PrintErr("Mesh is null for tile type: " + type.ToString());
			}
		}
		else
		{
			GD.PrintErr("Mesh not found for tile type: " + type.ToString());
		}
	}
}
