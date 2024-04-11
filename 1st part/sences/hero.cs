using Godot;
using System;

public partial class hero : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -500.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity; 
		if (velocity.X > 1  || velocity.X < -1 )
		{
			GetNode<AnimatedSprite2D>("Sprite2D").Play("run");
		}
		else 
		{
			GetNode<AnimatedSprite2D>("Sprite2D").Play("idel");
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
            GetNode<AnimatedSprite2D>("Sprite2D").Play("jump");
			velocity.Y += gravity * (float)delta;
		}
		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			GetNode<AnimatedSprite2D>("Sprite2D").Play("run");
			velocity.X = direction.X * Speed;
			// Flip the sprite based on the direction
			if (direction.X < 0)
				GetNode<AnimatedSprite2D>("Sprite2D").Scale = new Vector2(Mathf.Abs(GetNode<AnimatedSprite2D>("Sprite2D").Scale.X) * -1, GetNode<AnimatedSprite2D>("Sprite2D").Scale.Y);
			else if (direction.X > 0)
				GetNode<AnimatedSprite2D>("Sprite2D").Scale = new Vector2(Mathf.Abs(GetNode<AnimatedSprite2D>("Sprite2D").Scale.X), GetNode<AnimatedSprite2D>("Sprite2D").Scale.Y);
		}
		else
		{
			GetNode<AnimatedSprite2D>("Sprite2D").Play("idle");
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
