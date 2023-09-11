using System.Linq;
using UnityEngine;

namespace Framework.Shake
{
	public class ShakeBuilder
	{
		private const int MAX_COMMANDS = 16;
		
		private readonly ShakeCommand[] commands = new ShakeCommand [MAX_COMMANDS];

		private int activeCommandIndex;

		private void ClampByCap (ref Vector3 newShake)
		{
			var activeShakers = commands.Take(activeCommandIndex);

			var activeMagnitude = 0f;
			if (activeShakers.Any())
			{
				var totalShake = activeShakers.
					Select(x => x.CalculateActiveShake()).
					Aggregate((x, y) => x + y);

				activeMagnitude = totalShake.magnitude;
			}

			var remainingMagnitude = ShakeGlobalSettings.MaximumShakeMagnitude - activeMagnitude;
			remainingMagnitude = Mathf.Max(0, remainingMagnitude);

			newShake = Vector3.ClampMagnitude(newShake, remainingMagnitude);
		}

		public void AddCommand(ShakeCommand shakeCommand)
		{
			if (activeCommandIndex == MAX_COMMANDS)
			{
				//! Limit reached, ignoring the incoming command.
				return;
			}

			shakeCommand.Multiplier = 1;
			
			if (!shakeCommand.ignoreGlobalCap)
			{
				// !Gonna clamp it by max magnitude.
				var targetShakeVector = shakeCommand.strengthVector;
				var expectedMagnitude = targetShakeVector.magnitude;
				ClampByCap(ref targetShakeVector);
				var actualMagnitude = targetShakeVector.magnitude;

				var multiplier = actualMagnitude / Mathf.Max( Mathf.Epsilon,expectedMagnitude);
				
				shakeCommand.Multiplier = multiplier;
			}

			if (activeCommandIndex > 0)
			{
				shakeCommand.Progress = commands[activeCommandIndex - 1].Progress;
			}

			commands [activeCommandIndex++] = shakeCommand;
	    }

		public Vector3 Run ()
		{
			var dT = Time.deltaTime;

		    var power = Vector3.zero;

		    for (var i = activeCommandIndex-1; i >= 0; --i)
		    {
			    var result = commands[i].ProcessCommand(in dT, out var additionalPower);

			    power += additionalPower;

			    if (!result) continue;

			    commands[i] = null; //! Command is completed. Handover to GC.
			    activeCommandIndex--;

			    //! pull the rest to the index, so they wont be null until activeCommandIndex.
			    for (var a = i; a < MAX_COMMANDS-1; a++)
			    {
				    commands[a] = commands[a + 1];
			    }
		    }

		    return power;
		}
    }
}
