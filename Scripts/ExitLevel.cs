using Godot;

namespace GodotGameJamGame.Scripts;

public partial class ExitLevel : Node
{
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("reset"))
		{
			GetTree().ReloadCurrentScene();
		}
	}

	public override void _Ready()
	{
		var music = GetChild<AudioStreamPlayer>(1);
		
		music.Play();
	}
}
