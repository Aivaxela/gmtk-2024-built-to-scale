using Godot;

public partial class Planet : Node2D
{
    [Export] Area2D gravityArea;
    [Export] Sprite2D sprite;
    Ship ship;

    public override void _Ready()
    {
        ship = GetNode<Ship>("/root/main/ship");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("scale-up") && ship.planetPower < 5) ship.planetPower += 0.009f;
        if (Input.IsActionPressed("scale-down") && ship.planetPower > 1) ship.planetPower -= 0.009f;

        sprite.Scale = new Vector2(ship.planetPower, ship.planetPower);
        gravityArea.Scale = new Vector2(ship.planetPower / 4f, ship.planetPower / 4f);
    }
}
