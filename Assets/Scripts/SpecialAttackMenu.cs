using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialAttackMenu : MonoBehaviour {
    
    public Button menuFirePunch;    //1
    public Button menuMermCannon;   //2
    public Button menuMaceOfTrit;   //3
    public Button menuRARLaser;     //4
    public Button menuClownDrill;   //5

    PlayerControl _player;
    Animator _anim;
    public int menuSetting;
    GlobalControl _globalControl;

    // Use this for initialization
    void Start () {
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
        _anim = GetComponent<Animator>();
        GameObject gc = GameObject.Find("GlobalControl");
        if (gc != null) {
            _globalControl = gc.GetComponent<GlobalControl>();
        }
	}

    void Update()
    {
        _anim.SetInteger("MenuSetting", menuSetting);
    }

    public void SetFirePunch()
    {
        menuSetting = 1;
        _player.hasMermaidCannon = false;
        _player.hasMaceOfTrit = false;
        _player.hasRARLaser = false;
        _player.hasClownDrill = false;
    }

    public void SetMermaidCannon()
    {
        Debug.Log("Hit:" + _globalControl);
        if(_globalControl == null || _globalControl.mermCannonUnlocked == true)
        {
            menuSetting = 2;
            _player.hasMermaidCannon = true;
            _player.hasMaceOfTrit = false;
            _player.hasRARLaser = false;
            _player.hasClownDrill = false;
        }
    }

    public void SetMaceOfTrit()
    {
        if (_globalControl == null || _globalControl.maceOfTritUnlocked == true)
        {
            menuSetting = 3;
            _player.hasMermaidCannon = false;
            _player.hasMaceOfTrit = true;
            _player.hasRARLaser = false;
            _player.hasClownDrill = false;
        }
    }

    public void SetRARLaser()
    {
        if (_globalControl == null || _globalControl.rarLaserUnlocked == true)
        {
            menuSetting = 4;
            _player.hasMermaidCannon = false;
            _player.hasMaceOfTrit = false;
            _player.hasRARLaser = true;
            _player.hasClownDrill = false;
        }
    }

    public void SetClownDrill()
    {
        if (_globalControl == null || _globalControl.clownDrillUnlocked == true)
        {
            menuSetting = 5;
            _player.hasMermaidCannon = false;
            _player.hasMaceOfTrit = false;
            _player.hasRARLaser = false;
            _player.hasClownDrill = true;
        }
    }
}
