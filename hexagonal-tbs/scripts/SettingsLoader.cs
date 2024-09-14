using Godot;
using System;

struct ConfigSettings
{
	public bool mute_on_unfocus;
	public int master_vol;
}

public partial class SettingsLoader : Node3D
{
	private ConfigSettings settings;
	private AudioStreamPlayer testSound; // Assuming you have an AudioStreamPlayer node

	public override void _Ready()
	{
		// Initialize the AudioStreamPlayer node (assuming it’s a child node of this node)
		//testSound = GetNode<AudioStreamPlayer>("testSound"); // Update the path as needed
		
		var configFile = new ConfigFile();
		Error err = configFile.Load("res://settings.cfg");
		
		if (err != Error.Ok)
		{
			GD.Print("Failed to load settings file: " + err);
			settings.mute_on_unfocus = false; // Default value
			settings.master_vol = 50;         // Default value
		}
		else
		{
			// Load values from the config file
			settings.mute_on_unfocus = (bool)configFile.GetValue("audio", "mute_on_unfocus", false);
			settings.master_vol = (int)configFile.GetValue("audio", "master_volume", 20);
		}

		// Set the master volume (convert percentage to decibels)
		float dbVolume = (settings.master_vol > 0) ? 20f * Mathf.Log(settings.master_vol / 100f) : -80f;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), dbVolume);
		
		CallDeferred(nameof(LoadMapScene));
	}
	
	public void LoadMapScene()
	{
		//Load the new scene for instancing
		var mapScene = (PackedScene)ResourceLoader.Load("res://scenes/level1Map.tscn");
		
		if(mapScene != null)
		{	
			//instance pointer to current scene
			Node lastScene = GetTree().CurrentScene;
			
			//Instance the new scene
			GetTree().ChangeSceneToPacked(mapScene);
			
			//close previous scene
			//lastScene.QueueFree();
		}
		else{
			GD.PrintErr("Unable to load scene");
		}
	}

	public override void _Process(double delta)
	{
	}
}
