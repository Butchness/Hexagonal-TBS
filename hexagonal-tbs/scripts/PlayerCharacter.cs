using Godot;
using System;

public partial class PlayerCharacter : Node3D
{
	private Node3D _pivot;
	private double itterator = 0;
	
	public override void _Ready()
	{
		_pivot = GetNode<Node3D>("pivot");
	}
	
	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventKey eventKey){
			if(eventKey.Pressed && eventKey.Keycode == Key.Escape){
				GetTree().Quit(); //if the escape key is pressed, close the game
			}
			else if(eventKey.Pressed && eventKey.Keycode == "ui_forward"){
				
			}
		}
		//parse user inputs
	}
	
	public override void _Process(double delta)
	{
	}
}
