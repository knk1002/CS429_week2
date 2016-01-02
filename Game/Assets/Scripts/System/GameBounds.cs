using System;

namespace Assets.Scripts.System
{
	public class GameBounds
	{
		int leftBound;
		int rightBound;
		int upperBound;
		int lowerBound;

		public GameBounds(int left, int right, int up, int down) {
			leftBound = left;
			rightBound = right;
			upperBound = up;
			lowerBound = down;
		}
	}
}

