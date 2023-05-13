using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
	[Export] private NodePath _tileMapPath;
	[Export] private NodePath _animatedSprite2DPath;
	
	private TileMap _tileMap;

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

		GetChild<AnimatedSprite2D>(0).Play();
	}

	public override void _PhysicsProcess(double delta)
	{
	}
}
