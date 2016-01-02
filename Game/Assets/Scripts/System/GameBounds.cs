using System;

namespace Assets.Scripts.System
{
	public class GameBounds
	{
		public int leftBound;
		public int rightBound;
		public int upperBound;
		public int lowerBound;

		public GameBounds(int left, int right, int up, int down) {
			leftBound = left;
			rightBound = right;
			upperBound = up;
			lowerBound = down;
		}
	}
}

