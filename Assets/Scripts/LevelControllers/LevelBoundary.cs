using UnityEngine;
using System.Collections;

public class LevelBoundary : MonoBehaviour
{
	public static float topWidth = 0;
	public static float bottomWidth = 0;
	public static float height = 0;
	public static float bottom = 0;
	public static float left = 0;
	
	public static Vector3 adjustPositionToBoundary (Vector3 position)
	{
		// Start with y position. X position is different based on how it's adjusted.
		float yVal = Mathf.Max (bottom, Mathf.Min (bottom + height, position.y));

		// Get the fraction position between top and bottom from 0 to 1.
		float ratio = (yVal - bottom) / height;

		// Get the difference in width between the top and bottom widths.
		float widthDiff = Mathf.Abs (topWidth - bottomWidth) * (1f - ratio);

		// Get the left side offset.
		float leftOffset = left + Mathf.Abs (topWidth - bottomWidth) * ratio * .5f;

		// Get the boundary width at said ratio.
		float width = widthDiff + Mathf.Min (topWidth, bottomWidth);

		// Finally, we can get the x value!
		float xVal = Mathf.Max (leftOffset, Mathf.Min (leftOffset + width, position.x));
		return new Vector3 (xVal, yVal, position.z);
	}
}


