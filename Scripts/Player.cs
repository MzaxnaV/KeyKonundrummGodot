using Godot;
using System.Diagnostics;

public enum key
{
	Up, Down, Left, Right
}

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
	private bool _keyUp;
	private bool _keyDown;
	private bool _keyLeft;
	private bool _keyRight;
	
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

		if (GetCellData(_tilePos.X, _tilePos.Y) != 6)
		{
			Debug.Fail("Start Position not valid, check inspector and set tile pos.");
		}
		else
		{
			_tileMap.SetCell(1, _tilePos);
			Debug.Print(_tilePos.ToString());
			Position = _size * _tilePos;
		}
		
		_keyUp = true;
		_keyDown = true;
		_keyLeft = true;
		_keyRight = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isMoving)
		{
			return;
		}
		
		var direction = new Vector2I(0, 0);
		
		if (_keyUp && Input.IsActionJustPressed("ui_up"))
		{
			SetTurn(ref direction, ref _keyUp, 0, -1);
		}
		if (_keyDown && Input.IsActionJustPressed("ui_down"))
		{
			SetTurn(ref direction, ref _keyDown, 0, 1);
		}
		if (_keyLeft && Input.IsActionJustPressed("ui_left"))
		{
			SetTurn(ref direction, ref _keyLeft, -1, 0);
		}
		if (_keyRight && Input.IsActionJustPressed("ui_right"))
		{
			SetTurn(ref direction, ref _keyRight, 1, 0);
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
	
	private int GetCellData(int posX, int posY)
	{
		var tileData = _tileMap.GetCellTileData(1, new Vector2I(posX, posY));
		return tileData != null ? tileData.GetCustomData("id").As<int>() : 0;
	}

	private void SetTurn(ref Vector2I direction, ref bool key, int x, int y)
	{
		switch (GetCellData(_tilePos.X, _tilePos.Y))
		{
			case 3:
				Debug.Print("Drop");
				key = false;
				
				GD.Print("Tile value before: " + GetCellData(_tilePos.X, _tilePos.Y));
				_tileMap.SetCell(1, _tilePos, 7, new Vector2I(1, 0), 5);
				GD.Print("Tile value after: " + GetCellData(_tilePos.X, _tilePos.Y));
				
				break;
			case 4:
				Debug.Print("Pick");
				// change the key to Drop
				key = true;
				break;
		}

		// check the cell data
		switch (GetCellData(_tilePos.X + x, _tilePos.Y + y))
		{
			case 1:
				break;
			case 2:
				switch (GetCellData(_tilePos.X + 2 * x, _tilePos.Y + 2 * y))
				{
					case 1:
						direction.Y += y;
						direction.X += x;
						break;
					/*
					case 3:
						direction.Y += y;
						direction.X += x;
						Debug.Print("Drop");
						key = false;
						// change the key to Pick
						break;
					case 4:
						direction.Y += y;
						direction.X += x;
						Debug.Print("Pick");
						// change the key to Drop
						key = true;
						break;
					*/
					default: 
						direction.Y += 2 * y;
						direction.X += 2 * x;
						break;
				}
				break;
			default:
				direction.Y += y;
				direction.X += x;
				break;
		}
	}
}
