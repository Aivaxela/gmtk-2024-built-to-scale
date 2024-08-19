using Godot;

public partial class Planet : Node2D
{
    [Export] Area2D gravityArea;
    [Export] Area2D shipCheckArea;
    [Export] Sprite2D sprite;
    [Export] Sprite2D selectionSprite;
    [Export] AnimationPlayer animPlayer;
    [Export] Timer destroyTimer;
    [Export] GpuParticles2D planetDebris;
    [Export] int maxScale = 3;

    Ship ship;
    EscapePod ep1;
    EscapePod ep2;
    EscapePod ep3;

    float planetScale = 1;
    bool isSelected = false;

    public override void _Ready()
    {
        planetScale = sprite.Scale.X;

        ship = GetNode<Ship>("/root/main/ship");
        ep1 = GetNode<EscapePod>("/root/main/escape-pods/escape-pod1");
        ep2 = GetNode<EscapePod>("/root/main/escape-pods/escape-pod2");
        ep3 = GetNode<EscapePod>("/root/main/escape-pods/escape-pod3");

        gravityArea.MouseEntered += OnMouseEntered;
        gravityArea.MouseExited += OnMouseExited;
        shipCheckArea.AreaEntered += OnAreaEntered;

        if (destroyTimer != null) destroyTimer.Timeout += OnTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("scale-up") && planetScale < maxScale && isSelected) planetScale += 0.05f;
        if (Input.IsActionPressed("scale-down") && planetScale > 0.5 && isSelected) planetScale -= 0.05f;
        if (Input.IsActionJustPressed("interact") && Name == "planet-earth")
        {
            if (ship.planetNear.Name == "planet-earth" && ship.podReady) LaunchPodFromEarth();
        }
        if (Input.IsActionJustPressed("interact") && Name == "planet-mars")
        {
            if (ship.planetNear.Name == "planet-mars" && ship.warpShipReady) LaunchWSFromMars();
        }

        sprite.Scale = new Vector2(planetScale, planetScale);
        gravityArea.Scale = new Vector2(planetScale / 4f, planetScale / 4f);
    }


    private void LaunchPodFromEarth()
    {
        switch (ship.podCounter)
        {
            case 1:
                ep1.launched = true;
                ep1.Visible = true;
                ep1.velocity = new Vector2(30, 0);
                ship.podCounter++;
                ship.podReady = false;
                ship.podBoardingTimer.Start();
                return;
            case 2:
                ep2.launched = true;
                ep2.Visible = true;
                ep2.velocity = new Vector2(30, 10);
                ship.podCounter++;
                ship.podReady = false;
                ship.podBoardingTimer.Start();
                return;
            case 3:
                ep3.launched = true;
                ep3.Visible = true;
                ep3.velocity = new Vector2(25, -13);
                ship.podCounter++;
                ship.podReady = false;
                ship.podBoardingTimer.Start();
                return;
            default:
                break;
        }
    }

    private void LaunchWSFromMars()
    {
        ship.LaunchWarpShip();
        ship.warpShipReady = false;
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
            shipCheckArea.QueueFree();
            return;
        }
        if (area.GetParent() is EscapePod)
        {
            EscapePod escapePod = (EscapePod)area.GetParent();
            escapePod.QueueFree();
            if (Name == "planet-mars")
            {
                ship.podsArrived++;
                return;
            }
        }
        ship?.PrepReset();
    }

    private void OnTimeout()
    {
        planetDebris.Emitting = true;
        Timer debrisTimer = (Timer)planetDebris.GetNode("Timer");
        debrisTimer.Start();
        RemoveChild(planetDebris);
        GetParent().AddChild(planetDebris);
        planetDebris.GlobalPosition = GlobalPosition;

        QueueFree();
    }
}
