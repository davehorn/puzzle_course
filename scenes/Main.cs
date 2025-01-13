using Godot;
using System;
using System.Collections.Generic;

namespace Game;

public partial class Main : Node
{
	private Sprite2D cursor;
	private PackedScene buildingScene;
	private Button placeBuildingButton;
	private TileMapLayer highlightTileMapLayer;
	private Vector2? hoveredGridCell;
	private HashSet<Vector2> occupiedCells = new HashSet<Vector2>();
	// can also do = new() to instantiate the object


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buildingScene = GD.Load<PackedScene>("res://scenes//Building/Building.tscn");
		cursor = GetNode<Sprite2D>("Cursor");
		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");
		highlightTileMapLayer = GetNode<TileMapLayer>("HighlightTileMapLayer");

		cursor.Visible = false;

		//placeBuildingButton.Pressed += () => PlaceBuildingAtMousePosition();
		placeBuildingButton.Pressed += OnButtonPressed;
		//placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));


		GD.Print("Hello, World!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var gridPosition = GetMouseGridCellPosition();
		cursor.GlobalPosition = gridPosition * 64;

		if (cursor.Visible && (!hoveredGridCell.HasValue || hoveredGridCell.Value != gridPosition))
		{
			//highlightTileMapLayer.Clear();
			//highlightTileMapLayer.SetCellv(gridPosition, 0);
			hoveredGridCell = gridPosition;
			UpdateHighlightTileMapLayer();
		}
	}

	private Vector2 GetMouseGridCellPosition()
	{
		var mousePosition = highlightTileMapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 64;
		gridPosition = gridPosition.Floor();
		//GD.Print(gridPosition);
		return gridPosition;
	}

	public override void _UnhandledInput(InputEvent evt)
	{
		if (hoveredGridCell.HasValue && evt.IsActionPressed("left_click") && !occupiedCells.Contains(hoveredGridCell.Value))
		{
			PlaceBuildingAtHoveredCellPosition();
			cursor.Visible = false;
		}
	}

	private void PlaceBuildingAtHoveredCellPosition()
	{
		if (!hoveredGridCell.HasValue)
		{
			return;
		}

		var building = buildingScene.Instantiate<Node2D>();
		AddChild(building);

		building.GlobalPosition = hoveredGridCell.Value * 64;
		occupiedCells.Add(hoveredGridCell.Value);

		hoveredGridCell = null;
		UpdateHighlightTileMapLayer();

	}

	private void UpdateHighlightTileMapLayer()
	{
		highlightTileMapLayer.Clear();
		if (!hoveredGridCell.HasValue)
		{
			return;
		}

		GD.Print("current: " + hoveredGridCell);
		for (var x = hoveredGridCell.Value.X - 3; x <= hoveredGridCell.Value.X + 3; x++)
		{
			for (var y = hoveredGridCell.Value.Y - 3; y <= hoveredGridCell.Value.Y + 3; y++)
			{
				var newVector = new Vector2I((int)x, (int)y);
				GD.Print("painting highlight" + newVector);
				highlightTileMapLayer.SetCell(newVector, 0, Vector2I.Zero);

				//highlightTileMapLayer.SetCell(new Vector2I((int)x, (int)y), 0, Vector2I.Zero);
			}
		}

	}

	private void OnButtonPressed()
	{
		GD.Print("Button pressed");
		cursor.Visible = true;
	}
}
