using Godot;

public partial class Boundaries : Area2D
{
    Ship ship;

    public override void _Ready()
    {
        ship = GetNode<Ship>("/root/main/ship");
    }

    public override void _Process(double delta)
    {

        if (Name == "x-boundaries")
            GlobalPosition = new Vector2(ship.GlobalPosition.X, GlobalPosition.Y);

        if (Name == "y-boundaries")
            GlobalPosition = new Vector2(GlobalPosition.X, ship.GlobalPosition.Y);


    }
}
