using UnityEngine;
using System.Collections;

public class AbstractAssetController : MonoBehaviour
{
	// Spritesheet IDs
	public const int EXPLOSION_A_1 = 0;
	public const int EXPLOSION_A_2 = 1;
	public const int EXPLOSION_A_3 = 2;

	public const int MERMAID_A = 10;
	public const int MERMAID_B = 11;

	public const int BULLET_A = 20;
	public const int BOMB_A = 30;
	public const int BOMB_B = 31;

	// SFX:
	public const int SFX_EXPLOSION = 0;
	public const int SFX_MERMAID = 1;

	// Bullet
	private static WWW bulletWWW;
	protected static string bulletUrl = "/Sprites/Pirate Sheet.png";
	private static Rect bulletA = new Rect (1327, 238, 19, 17);

	// Mermaid
	private static WWW mermaidWWW;
	protected static string mermaidUrl = "/Sprites/Mermaid Cannon Sheet.png";
	private static Rect mermaidA = new Rect (380, 14, 155, 262);
	private static Rect mermaidB = new Rect (635, 15, 158, 263);
	
	// Explosion
	private static WWW explosionWWW;
	protected static string explosionUrl = "/Sprites/Pirate Captain.png";
	protected static Rect explosionA_1 = new Rect (915, 495, 90, 87);
	protected static Rect explosionA_2 = new Rect (1047, 434, 195, 206);
	protected static Rect explosionA_3 = new Rect (1316, 416, 195, 205);
	
	// Bomb
	private static Rect bombA = new Rect (587, 20, 74, 143);
	private static Rect bombB = new Rect (718, 22, 74, 143);

	// SFX
	private static WWW explosionSFXWWW;
	private static string explosionSFXUrl = "/Sounds/BB OGG/BB OGG/Pirate Captain Bomb.ogg";
	private static WWW mermaidWhistleSFXWWW;
	private static string mermaidWhistleSFXUrl = "/Sounds/BB OGG/BB OGG/Mermaids Falling.ogg";

	void Awake ()
	{
		loadSpritesheets ();
	}

	public void loadSpritesheets ()
	{
		string dataPath = "file://" + Application.dataPath;
		explosionWWW = new WWW (dataPath + explosionUrl);
		StartCoroutine (loadAsset (explosionWWW));

		mermaidWWW = new WWW (dataPath + mermaidUrl);
		StartCoroutine (loadAsset (mermaidWWW));

		bulletWWW = new WWW (dataPath + bulletUrl);
		StartCoroutine (loadAsset (bulletWWW));

		explosionSFXWWW = new WWW (dataPath + explosionSFXUrl);
		StartCoroutine (loadAsset (explosionSFXWWW));

		mermaidWhistleSFXWWW = new WWW (dataPath + mermaidWhistleSFXUrl);
		StartCoroutine (loadAsset (mermaidWhistleSFXWWW));
	}

	private IEnumerator loadAsset (WWW www)
	{
		yield return www;
	}

	public static void applyImage (SpriteRenderer renderer, int spriteId)
	{
		if (explosionWWW == null) {
			// Spritesheets not even loaded?
			Debug.Log ("Images not loaded! Add AbstractAssetController to GameMaster.");
			return;
		}

		WWW image;
		Rect rect;

		switch (spriteId) {
		case EXPLOSION_A_1:
			image = explosionWWW;
			rect = explosionA_1;
			break;
		case EXPLOSION_A_2:
			image = explosionWWW;
			rect = explosionA_2;
			break;
		case EXPLOSION_A_3:
			image = explosionWWW;
			rect = explosionA_3;
			break;
		case MERMAID_A:
			image = mermaidWWW;
			rect = mermaidA;
			break;
		case MERMAID_B:
			image = mermaidWWW;
			rect = mermaidB;
			break;
		case BULLET_A:
			image = bulletWWW;
			rect = bulletA;
			break;
		case BOMB_A:
			image = explosionWWW;
			rect = bombA;
			break;
		case BOMB_B:
			image = explosionWWW;
			rect = bombB;
			break;
		default:
			image = explosionWWW;
			rect = explosionA_1;
			break;
		}

		if (renderer.sprite) {
			// Remove the old texture first.
			Texture2D.DestroyImmediate (renderer.sprite.texture, true);
		}
		renderer.sprite = Sprite.Create (image.texture, rect, new Vector2 (0, 0), 100.0f);

		Vector3 center = renderer.sprite.bounds.center;
		renderer.transform.localPosition = new Vector3 (-center.x, -center.y, 0);

		BoxCollider2D coll = renderer.GetComponent<BoxCollider2D> ();
		if (coll) {
			coll.offset = new Vector2 (center.x, center.y);
			coll.size = new Vector2 (center.x * 2, center.y * 2);
		}
	}

	public static AudioClip getSFX (int sfxID)
	{
		switch (sfxID) {
		case SFX_EXPLOSION:
			return explosionSFXWWW.audioClip;
			break;
		case SFX_MERMAID:
			return mermaidWhistleSFXWWW.audioClip;
			break;
		}

		return null;
	}
}

