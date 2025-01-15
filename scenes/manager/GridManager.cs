using Game.Component;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Game.Manager;
// Provides data and state about the grid
public partial class GridManager : Node {

	private HashSet<Vector2I> occupiedCells = new HashSet<Vector2I>();

	[Export] private TileMapLayer highlightTileMapLayer;
	[Export] private TileMapLayer baseTerrainTileMapLayer;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	public bool IsTilePositionValid(Vector2I tilePosition) {
		//var tilePositionInt = new Vector2I((int)tilePosition.X, (int)tilePosition.Y);
		var customData = baseTerrainTileMapLayer.GetCellTileData(tilePosition);

		if (customData == null) {
			return false;
		}
		if (!(bool)customData.GetCustomData("buildable")) {
			return false;
		}


		return !occupiedCells.Contains(tilePosition);
	}

	public void MarkTileAsOccupied(Vector2I tilePosition) {
		occupiedCells.Add(tilePosition);
	}

	public void HighlightBuildableTiles() {
		//ClearHighlightedTiles();
		var buildingComponents = GetTree().GetNodesInGroup(nameof(BuildingComponent)).Cast<BuildingComponent>();
		foreach (var buildingComponent in buildingComponents) {
			//var buildingRadius = buildingComponent.BuildingRadius;
			//var buildingPosition = buildingComponent.GetGridPosition();
			HighlightValidTilesInRadius(buildingComponent.GetGridCellPosition(), buildingComponent.BuildableRadius);
		}
	}



	public void ClearHighlightedTiles() {
		highlightTileMapLayer.Clear();
	}

	public Vector2I GetMouseGridCellPosition() {
		var mousePosition = highlightTileMapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 64;
		gridPosition = gridPosition.Floor();
		//GD.Print(gridPosition);
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}

	private void HighlightValidTilesInRadius(Vector2I rootCell, int radius) {
		//ClearHighlightedTiles();

		//GD.Print("current: " + hoveredGridCell);
		for (var x = rootCell.X - radius; x <= rootCell.X + radius; x++) {
			for (var y = rootCell.Y - radius; y <= rootCell.Y + radius; y++) {
				var tilePosition = new Vector2I(x, y);
				if (!IsTilePositionValid(tilePosition)) {
					continue;
				}
				var newVector = new Vector2I((int)x, (int)y);
				//GD.Print("painting highlight" + newVector);
				highlightTileMapLayer.SetCell(tilePosition, 0, Vector2I.Zero);

				//highlightTileMapLayer.SetCell(new Vector2I((int)x, (int)y), 0, Vector2I.Zero);
			}
		}
	}
}
