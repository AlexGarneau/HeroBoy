using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverSpawnBoss : MonoBehaviour
{
	PlayerControl _player;
	
	float timer;
	float timerReset = 5f;

	void Start ()
	{
        GameObject[] list = GameObject.FindGameObjectsWithTag("Player");
        for (var i = list.Length - 1; i >= 0; i--)
        {
            PlayerControl player = list[i].GetComponent<PlayerControl>();
            if (player != null)
            {
                _player = player;
                break;
            }
        }
        timer = timerReset;
	}

	void Update ()
	{
		if (_player.playerHealth <= 0) {
			ActivateScreen ();
		}
	}

	void ActivateScreen ()
	{
		if (timer > 0) {
			timer -= 1 * Time.deltaTime;
			if (timer <= 0) {
				Application.LoadLevel (6);
				timer = 0;
			}
		}
	}
}
