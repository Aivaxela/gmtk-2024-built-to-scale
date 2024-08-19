using Godot;

public partial class Sun : CharacterBody2D
{
    [Export] Area2D shipCheckArea;
    [Export] Timer speedupTimer;

    Vector2 velocity = Vector2.Zero;
    float speed = 20;

    Ship ship;

    public override void _Ready()
    {
        ship = GetNode<Ship>("/root/main/ship");
        shipCheckArea.AreaEntered += OnAreaEntered;
        speedupTimer.Timeout += OnSpeedupTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        velocity = new Vector2(speed, 0);

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

    private void OnSpeedupTimerTimeout()
    {
        // speed += 5;

        GD.Print("sun speed: " + speed);
    }
}
