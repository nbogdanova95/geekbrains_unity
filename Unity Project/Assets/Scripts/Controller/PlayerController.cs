﻿namespace Geekbrains
{
	public class PlayerController : BaseController, IOnUpdate
	{
		private readonly IMotor _motor;

		public PlayerController(IMotor motor)
		{
			_motor = motor;
		}

		public void OnUpdate()
		{
			_motor.Move();
		}
	}
}