using Godot;

public partial class Camera : Camera2D
{
	[Export] Ship ship;
	[Export] Label boostingLabel;


	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		GlobalPosition = ship.GlobalPosition;

		boostingLabel.Text = ship.boosting ? "BOOSTING" : "";
	}
}
