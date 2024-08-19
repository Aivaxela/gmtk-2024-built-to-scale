using Godot;

public partial class Comet : CharacterBody2D
{
    [Export] Area2D gravityArea;
    [Export] Area2D shipCheckArea;
    [Export] Area2D spawnArea;
    [Export] Sprite2D sprite;
    [Export] Sprite2D selectionSprite;
    [Export] GpuParticles2D particles;

    Ship ship;
    float planetScale = 1;
    bool isSelected = false;
    Vector2 velocity = new Vector2(0, 0);

    public override void _Ready()
    {
        ship = GetNode<Ship>("/root/main/ship");

        gravityArea.MouseEntered += OnMouseEntered;
        gravityArea.MouseExited += OnMouseExited;
        shipCheckArea.AreaEntered += OnAreaEntered;
        spawnArea.AreaEntered += OnSpawnAreaEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = velocity;
        MoveAndSlide();

        if (Input.IsActionPressed("scale-up") && planetScale < 3 && isSelected) planetScale += 0.05f;
        if (Input.IsActionPressed("scale-down") && planetScale > 0.5 && isSelected) planetScale -= 0.05f;

        sprite.Scale = new Vector2(planetScale, planetScale);
        gravityArea.Scale = new Vector2(planetScale / 4f, planetScale / 4f);

        particles.RotationDegrees = Velocity.Angle();
    }


    private void OnMouseEntered()
    {
        selectionSprite.Visible = true;
        isSelected = true;
    }

    private void OnMouseExited()
    {
        selectionSprite.Visible = false;
        isSelected = false;
    }

    private void OnAreaEntered(Area2D area)
    {
        Ship ship = (Ship)area.GetParent();
        ship?.PrepReset();
    }

    private void OnSpawnAreaEntered(object _)
    {
        velocity = new Vector2(120, 60);
    }
}
