using Godot;
using System;
using System.Diagnostics;

public partial class Player : AnimatedSprite2D
{
	[Export] private NodePath _tileMapPath;
	[Export] private NodePath _barPath;
	[Export] private Vector2I _tilePos;
	
	[Signal] public delegate bool UiEventHandler();
	
	private TileMap _tileMap;
	private Sprite2D _bar;
	private bool _isMoving;
	private Timer _timer;
	private readonly Vector2I _size = new (36, 36);

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
		
		if (_barPath != null && _barPath.ToString() != "")
		{
			_bar = GetNode<Sprite2D>(_barPath);
		}
		else
		{
			Debug.Fail("BarSprite path not set, check inspector.");
		}

		_timer = GetChild<Timer>(0);
		_timer.Timeout += OnTimerTimeout;

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
		if (_isMoving)
		{
			return;
		}

		var direction = new Vector2I(0, 0);

		if (Input.IsActionPressed("ui_up"))
		{
			direction.Y -= 1;
		}
		if (Input.IsActionPressed("ui_down"))
		{
			direction.Y += 1;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			direction.X -= 1;
		}
		if (Input.IsActionPressed("ui_right"))
		{
			direction.X += 1;
		}

		if (direction != new Vector2I(0, 0))
		{
			_tilePos += direction;
			Position = _size * _tilePos;
			_isMoving = true;
			
			_timer.Start();
			// EmitSignal(nameof(UiEventHandler));
		}
	}

	private void OnTimerTimeout()
	{
		_isMoving = false;
	}
}
