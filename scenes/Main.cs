using Godot;
using System;

namespace Game;

public partial class Main : Node2D
{
	private Sprite2D sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Cursor");
		GD.Print("Hello, World!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var mousePosition = GetGlobalMousePosition();
		var gridPosition = mousePosition / 64;
		gridPosition = gridPosition.Floor();
		GD.Print(gridPosition);
		sprite.GlobalPosition = gridPosition * 64;
	}
}
