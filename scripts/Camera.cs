using Godot;

public partial class Camera : Camera2D
{
	[Export] Ship ship;


	public override void _Process(double delta)
	{
		GlobalPosition = ship.GlobalPosition;
	}
}
