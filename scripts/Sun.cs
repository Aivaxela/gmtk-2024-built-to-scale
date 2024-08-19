using Godot;

public partial class Sun : CharacterBody2D
{
    [Export] Area2D shipCheckArea;

    Vector2 velocity = new Vector2(70, 0);


    Ship ship;

    public override void _Ready()
    {
        ship = GetNode<Ship>("/root/main/ship");
        shipCheckArea.AreaEntered += OnAreaEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        velocity = new Vector2(50, 0);

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Process(double delta)
    {
        GlobalPosition = new Vector2(GlobalPosition.X, ship.GlobalPosition.Y);
    }

    private void OnAreaEntered(Area2D area)
    {
        ship?.PrepReset();
    }
}
