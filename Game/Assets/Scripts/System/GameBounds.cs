using System;

namespace Assets.Scripts.System
{
	public class GameBounds
	{
		public float leftBound;
		public float rightBound;
		public float upperBound;
		public float lowerBound;

		public GameBounds(float left, float right, float up, float down) {
			leftBound = left;
			rightBound = right;
			upperBound = up;
			lowerBound = down;
		}
	}
}

