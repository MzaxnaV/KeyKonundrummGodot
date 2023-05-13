using Godot;
using System;
using System.Diagnostics;

public partial class Player : AnimatedSprite2D
{
	[Export] private NodePath _tileMapPath;
	[Export] private Vector2I _tilePos;
	
	private TileMap _tileMap;

	private Vector2I _size = new Vector2I(36, 36);

	public override void _Ready()
	{
		if (_tileMapPath != null && _tileMapPath.ToString() != "")
		{
			_tileMap = GetNode<TileMap>(_tileMapPath);
		}
		else
		{
			Debug.Fail("TileMapPath path not set, check inspector.");
		}

		Play();

		if (_tileMap.GetCellTileData(1, _tilePos).GetCustomData("id").As<int>() != 6)
		{
			Debug.Fail("Start Position not valid, check inspector and set tile pos.");
		}
		else
		{
			_tileMap.SetCell(1, _tilePos);
			Debug.Print(_tilePos.ToString());
			Position = _size * _tilePos;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		
	}
}
