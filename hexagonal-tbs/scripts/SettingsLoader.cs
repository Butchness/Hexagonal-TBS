using Godot;
using System;
using static Godot.GD;

public partial class SettingsLoader : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var config = new ConfigFile();
		Error err = config.Load("res://settings.cfg");
		
		if(err != Error.Ok){
			Print(err);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
