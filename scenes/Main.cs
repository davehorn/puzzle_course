using Game.Manager;
using Godot;
using System;
using System.Collections.Generic;

namespace Game;

public partial class Main : Node {
	private GridManager gridManager;
	private Sprite2D cursor;
	private PackedScene buildingScene;
	private Button placeBuildingButton;
	private Vector2I? hoveredGridCell;
	// can also do = new() to instantiate the object


	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		buildingScene = GD.Load<PackedScene>("res://scenes//Building/Building.tscn");
		gridManager = GetNode<GridManager>("GridManager");
		cursor = GetNode<Sprite2D>("Cursor");
		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

		cursor.Visible = false;

		//placeBuildingButton.Pressed += () => PlaceBuildingAtMousePosition();
		placeBuildingButton.Pressed += OnButtonPressed;
		//placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));


		GD.Print("Hello, World!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		var gridPosition = gridManager.GetMouseGridCellPosition();
		cursor.GlobalPosition = gridPosition * 64;

		if (cursor.Visible && (!hoveredGridCell.HasValue || hoveredGridCell.Value != gridPosition)) {
			//highlightTileMapLayer.Clear();
			//highlightTileMapLayer.SetCellv(gridPosition, 0);
			hoveredGridCell = gridPosition;
			gridManager.HighlightBuildableTiles(); ;

		}
	}




	public override void _UnhandledInput(InputEvent evt) {
		if (hoveredGridCell.HasValue && evt.IsActionPressed("left_click") && gridManager.IsTilePositionValid(hoveredGridCell.Value)) {
			PlaceBuildingAtHoveredCellPosition();
			cursor.Visible = false;
		}
	}

	private void PlaceBuildingAtHoveredCellPosition() {
		if (!hoveredGridCell.HasValue) {
			return;
		}

		var building = buildingScene.Instantiate<Node2D>();
		AddChild(building);

		building.GlobalPosition = hoveredGridCell.Value * 64;
		gridManager.MarkTileAsOccupied(hoveredGridCell.Value);

		hoveredGridCell = null;
		gridManager.ClearHighlightedTiles();

	}

	private void OnButtonPressed() {
		GD.Print("Button pressed");
		cursor.Visible = true;
	}
}
