extends Node

func _ready():
	var bgColor = Color(0.1, 0.1, 0.1)
	RenderingServer.set_default_clear_color(bgColor)
