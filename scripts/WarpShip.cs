using Godot;

public partial class WarpShip : CharacterBody2D
{
    [Export] float speed = 20;
    Vector2 velocity;
    Vector2 direction;


    public override void _Ready()
    {
        velocity = new Vector2(0, 0);
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
}
