using UnityEngine;
using System.Collections;

public class LevelBoundary : MonoBehaviour
{
    public static int TYPE_RECTANGLE = 0;
    public static int TYPE_CIRCLE = 1;

    // Rectangle vars.
	public static float topWidth = 0;
	public static float bottomWidth = 0;
	public static float height = 0;
	public static float bottom = 0;
	public static float left = 0;

    // Circle vars.
    public static Vector2 circleCenter = Vector2.zero;
    public static float circleRadius = 0;

    public static int type = TYPE_RECTANGLE;
	
	public static Vector3 adjustPositionToBoundary (Vector3 position)
	{
        float xVal = 0;
        float yVal = 0;
        if (type == TYPE_RECTANGLE) {
            // Start with y position. X position is different based on how it's adjusted.
            yVal = Mathf.Max(bottom, Mathf.Min(bottom + height, position.y));

            // Get the fraction position between top and bottom from 0 to 1.
            float ratio = (yVal - bottom) / height;

            // Get the difference in width between the top and bottom widths.
            float widthDiff = Mathf.Abs(topWidth - bottomWidth) * (1f - ratio);

            // Get the left side offset.
            float leftOffset = left + Mathf.Abs(topWidth - bottomWidth) * ratio * .5f;

            // Get the boundary width at said ratio.
            float width = widthDiff + Mathf.Min(topWidth, bottomWidth);

            // Finally, we can get the x value!
            xVal = Mathf.Max(leftOffset, Mathf.Min(leftOffset + width, position.x));
        } else if (type == TYPE_CIRCLE) {
            // Circle stage. Make sure position is within the radius. Simple.
            var pos = new Vector2(position.x, position.y);
            var dist = Vector2.Distance(circleCenter, pos);
            if (dist <= circleRadius) {
                // He's just fine.
                xVal = position.x;
                yVal = position.y;
            } else {
                // Too far! Gotta move him in to the edge of the circle.
                Vector2 direction = new Vector2(pos.x - circleCenter.x, pos.y - circleCenter.y);
                direction.Normalize();
                xVal = direction.x * circleRadius + circleCenter.x;
                yVal = direction.y * circleRadius + circleCenter.y;
            }
        }

        return new Vector3(xVal, yVal, position.z);
	}
}


