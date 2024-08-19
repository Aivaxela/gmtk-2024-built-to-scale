using Godot;

public partial class WarpShip : CharacterBody2D
{
    [Export] float speed = 20;
    [Export] Area2D nepCheckArea;
    [Export] Area2D sunCheckArea;
    public Vector2 velocity;
    Vector2 direction;
    Session session;


    public override void _Ready()
    {
        velocity = new Vector2(0, 0);

        session = GetNode<Session>("/root/Session");

        sunCheckArea.AreaEntered += OnSunEntered;
        nepCheckArea.AreaEntered += OnNepEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateDirection();
        CalculateVelocity();
    }


    private void UpdateDirection()
    {
        direction = velocity;
        speed = Mathf.Lerp(speed, 100f, 0.01f);
    }

    private void CalculateVelocity()
    {
        velocity = velocity.Slerp(direction, 0.007f);
        velocity = velocity.Normalized() * speed;

        Velocity = velocity;
        MoveAndSlide();
        Rotation = velocity.Angle();
    }

    private void OnNepEntered(object _)
    {
        session.warpShipSafe = true;
        QueueFree();
    }

    private void OnSunEntered(object _)
    {
        QueueFree();
    }
}
