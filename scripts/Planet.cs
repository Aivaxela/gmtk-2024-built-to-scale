using Godot;

public partial class Planet : Node2D
{
    [Export] Area2D gravityArea;
    [Export] Area2D shipCheckArea;
    [Export] Sprite2D sprite;
    [Export] Sprite2D selectionSprite;
    [Export] AnimationPlayer animPlayer;
    [Export] Timer destroyTimer;
    [Export] PackedScene planetDestroyScene;

    Ship ship;
    float planetScale = 1;
    bool isSelected = false;

    public override void _Ready()
    {
        ship = GetNode<Ship>("/root/main/ship");

        gravityArea.MouseEntered += OnMouseEntered;
        gravityArea.MouseExited += OnMouseExited;
        shipCheckArea.AreaEntered += OnAreaEntered;

        if (destroyTimer != null) destroyTimer.Timeout += OnTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("scale-up") && planetScale < 3 && isSelected) planetScale += 0.05f;
        if (Input.IsActionPressed("scale-down") && planetScale > 0.5 && isSelected) planetScale -= 0.05f;

        sprite.Scale = new Vector2(planetScale, planetScale);
        gravityArea.Scale = new Vector2(planetScale / 4f, planetScale / 4f);
    }


    private void OnMouseEntered()
    {
        if (!destroyTimer.IsStopped()) return;
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
        if (area.GetParent() is Sun && destroyTimer != null)
        {
            animPlayer.Play("burn up");
            destroyTimer.Start();
            return;
        }
        ship?.Reset();
    }

    private void OnTimeout()
    {
        PlanetDebris planetDebris = (PlanetDebris)planetDestroyScene.Instantiate();
        planetDebris.GlobalPosition = GlobalPosition;
        RemoveChild(planetDebris);
        GetParent().AddChild(planetDebris);
        QueueFree();
    }
}
