using System.IO;
using Godot;

public partial class EndingScene : Node2D
{
    [Export] Label epsSaved;
    [Export] Label mercTreasure;
    [Export] Label planetX;
    [Export] Label plutoVisit;
    [Export] Button replay;
    [Export] Button exit;

    Session session;


    public override void _Ready()
    {
        session = GetNode<Session>("/root/Session");

        if (session.warpShipSafe)
        {
            epsSaved.Text = $"Escape Pods saved: {session.podsSaved}/3";
            if (session.podsSaved == 3) epsSaved.Modulate = Colors.GreenYellow;
        }

        if (session.plutoVisited)
        {
            plutoVisit.Text = "Pluto visited: Yes";
            plutoVisit.Modulate = Colors.GreenYellow;
        }

        if (session.xFound)
        {
            planetX.Text = "Planet X Found: Yes";
            planetX.Modulate = Colors.GreenYellow;
        }

        if (session.mercTokenCollected)
        {
            mercTreasure.Text = "Mercury's Treasure found: Yes";
            mercTreasure.Modulate = Colors.GreenYellow;
        }

        replay.ButtonDown += OnReplayButtonDown;
        exit.ButtonDown += OnExitButtonDown;
    }

    private void OnReplayButtonDown()
    {
        GetTree().ChangeSceneToFile("res://scene/main.tscn");
    }

    private void OnExitButtonDown()
    {
        GetTree().Quit();
    }
}
