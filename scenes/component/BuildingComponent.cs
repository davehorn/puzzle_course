using Godot;
using System;

namespace Game.Component;

public partial class BuildingComponent : Node2D {

	[Export] public int BuildableRadius { get; private set; }




	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		AddToGroup(nameof(BuildingComponent));

	}

	public Vector2I GetGridCellPosition() {
		var gridPosition = GlobalPosition / 64;
		gridPosition = gridPosition.Floor();
		//GD.Print(gridPosition);
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
		//return new Vector2I((int)GlobalPosition.x / 64, (int)GlobalPosition.y / 64);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
}
