using UnityEngine;
using System.Collections;

public class MovementController2D : RaycastController
{

	float maxClimbAngle = 80f;
	float maxDescendAngle = 75f;
		
	public CollisionInfo _collisions;
		
	public override void Start ()
	{
		base.Start ();
	}
		
	public void Move (Vector3 _vel, bool standingOnPlatform = false)
	{
		UpdateRaycastOrigins ();
		_collisions.Reset ();
		_collisions.velocityOld = _vel;
			
		if (_vel.y < 0) {
			DescentSlope (ref _vel);
		}
		if (_vel.x != 0) {
			HorizontalCollisions (ref _vel);
		}
		if (_vel.y != 0) {
			VerticalCollisions (ref _vel);
		}
			
		transform.Translate (_vel);
			
		if (standingOnPlatform) {
			_collisions.below = true;
		}
	}
		
	void HorizontalCollisions (ref Vector3 _vel)
	{
		float directionX = Mathf.Sign (_vel.x);
		float rayLength = Mathf.Abs (_vel.x) + skinwidth;
		for (int i = 0; i < _horiRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (_horiSpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, colMask);
				
			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);
				
			if (hit) {
					
				if (hit.distance == 0) {
					continue;
				}
					
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
					
				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (_collisions.descendingSlope) {
						_collisions.descendingSlope = false;
						_vel = _collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != _collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance - skinwidth;
						_vel.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope (ref _vel, slopeAngle);
					_vel.x += distanceToSlopeStart * directionX;
				}
					
				if (!_collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					_vel.x = (hit.distance - skinwidth) * directionX;
					rayLength = hit.distance;
						
					if (_collisions.climbingSlope) {
						_vel.y = Mathf.Tan (_collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (_vel.x);
					}
						
					_collisions.left = directionX == -1;
					_collisions.right = directionX == 1;
				}
			}
		}
	}
		
	void VerticalCollisions (ref Vector3 _vel)
	{
		float directionY = Mathf.Sign (_vel.y);
		float rayLength = Mathf.Abs (_vel.y) + skinwidth;
		for (int i = 0; i < _vertRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (_vertSpacing * i + _vel.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, colMask);
				
			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);
				
			if (hit) {
				_vel.y = (hit.distance - skinwidth) * directionY;
				rayLength = hit.distance;
					
				if (_collisions.climbingSlope) {
					_vel.x = Mathf.Tan (_collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (_vel.x);
				}
					
				_collisions.below = directionY == -1;
				_collisions.above = directionY == 1;
			}
		}
			
		if (_collisions.climbingSlope) {
			float directionX = Mathf.Sign (_vel.x);
			rayLength = Mathf.Abs (_vel.x) + skinwidth;
			Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * _vel.y;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, colMask);
				
			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
				if (slopeAngle != _collisions.slopeAngle) {
					_vel.x = (hit.distance - skinwidth) * directionX;
					_collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}
		
	void ClimbSlope (ref Vector3 _vel, float slopeAngle)
	{
		float moveDistance = Mathf.Abs (_vel.x);
		float climbVelY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
		if (_vel.y <= climbVelY) {
			_vel.y = climbVelY;
			_vel.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (_vel.x);
			_collisions.below = true;
			_collisions.climbingSlope = true;
			_collisions.slopeAngle = slopeAngle;
		}
	}
		
	void DescentSlope (ref Vector3 _vel)
	{
		float directionX = Mathf.Sign (_vel.x);
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, colMask);
			
		if (hit) {
			float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign (hit.normal.x) == directionX) {
					if (hit.distance - skinwidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (_vel.x)) {
						float moveDistance = Mathf.Abs (_vel.x);
						float descendVelY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						_vel.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (_vel.x);
						_vel.y -= descendVelY;
							
						_collisions.slopeAngle = slopeAngle;
						_collisions.descendingSlope = true;
						_collisions.below = true;
					}
				}
			}
		}
	}
		
	/*void UpdateRaycastOrigins ()
	{
		Bounds bounds = _col.bounds;
		bounds.Expand (skinwidth * -2);
		
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.min.y);
	}
	
	void CalculateRaySpacing ()
	{
		Bounds bounds = _col.bounds;
		bounds.Expand (skinwidth * -2);
		
		_horiRayCount = Mathf.Clamp (_horiRayCount, 2, int.MaxValue);
		_vertRayCount = Mathf.Clamp (_vertRayCount, 2, int.MaxValue);
		
		_horiSpacing = bounds.size.y / (_horiRayCount - 1);
		_vertSpacing = bounds.size.x / (_vertRayCount - 1);
	}
	
	struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}*/
		
	public struct CollisionInfo
	{
		public bool above, below;
		public bool left, right;
			
		public bool climbingSlope;
		public bool descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld;
			
		public void Reset ()
		{
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;
				
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}
}


