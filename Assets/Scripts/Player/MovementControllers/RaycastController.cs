using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{

	public LayerMask colMask;
	
	public const float skinwidth = 0.015f;
	public int _horiRayCount = 4;
	public int _vertRayCount = 4;

	[HideInInspector]
	public float
		_horiSpacing;
	[HideInInspector]
	public float
		_vertSpacing;
	[HideInInspector]
	public BoxCollider2D
		_col;

	public RaycastOrigins raycastOrigins;
	
	public virtual void Start ()
	{
		_col = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}


	public void UpdateRaycastOrigins ()
	{
		Bounds bounds = _col.bounds;
		bounds.Expand (skinwidth * -2);
		
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.min.y);
	}
	
	public void CalculateRaySpacing ()
	{
		Bounds bounds = _col.bounds;
		bounds.Expand (skinwidth * -2);
		
		_horiRayCount = Mathf.Clamp (_horiRayCount, 2, int.MaxValue);
		_vertRayCount = Mathf.Clamp (_vertRayCount, 2, int.MaxValue);
		
		_horiSpacing = bounds.size.y / (_horiRayCount - 1);
		_vertSpacing = bounds.size.x / (_vertRayCount - 1);
	}
	
	public struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
