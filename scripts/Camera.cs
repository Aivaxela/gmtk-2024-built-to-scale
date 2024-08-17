using Godot;

public partial class Camera : Camera2D
{
	[Export] CharacterBody2D ship;


	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		Vector2 newCameraPos = new(ship.GlobalPosition.X + 350, ship.GlobalPosition.Y);
		GlobalPosition = newCameraPos;
	}
}
