using Godot;

namespace GodotGameJamGame.Scripts;

public partial class ExitLevel : Node
{
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("exit"))
		{
			GetTree().Quit();
		}
	}
}