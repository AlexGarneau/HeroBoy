using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MovementController2D))]
public class PlayerControl : AbstractClass
{
    public const string PLAYER_ANIM_DODGE_START = "PlayerDodge";

    public enum PlayerStates
    {
        immobile,
        mobile,
        attacking,
        dodging,
        stunned,
        dying,
        shoulderdash,
        clownDrill
    }
    ;
    public PlayerStates state;

    public int playerHealth;
	public float moveSpeed = 3f;
    public bool earnKills = true;

    public bool tempInvuln;
    public float tempInvulnTimer = 0f;
    public float stunTime = 1f;

    public float dodgeForce = 5f;
    public float dodgeDecline = 0.5f;

	public GameObject mermaidBomb;
	public GameObject mermanBomb;

	Vector3 _vel;
	float velocityXSmoothing = 1f;
	float velocityYSmoothing = 1f;

	[HideInInspector]
	public bool
		facingLeft;
	[HideInInspector]
	public bool
		facingRight;
	[HideInInspector]
	bool
		isMoving;

	bool inputA = false;
	//LIGHT
	bool inputB = false;
	//MEDIUM
	bool inputC = false;
	//HEAVY
	bool inputD = false;
	//SPECIAL
	bool inputDHold = false;
    //SPECIAL HOLD (FOR CHARGE)
    bool inputDodge = false;

    float _doubleTapTimeRight;
    float _doubleTapTimeLeft;

	Animator _anim;
	MovementController2D _controller;
	ChargeBarScript _chargeBar;

	public PlayerDamageCollider dc;
    public BoxCollider2D box;
	public Collider2D healthPickup;
    public Collider2D chargePartPickup;
    public Collider2D chargeFullPickup;

    public GameObject hitSplat;
    public GameObject hitDust;
    public Transform hitDustSpawn;

    //STANDARD MOVES LOCK
    public bool canAttack;
    public GameObject stunIcon;

    //SPECIAL MOVES INVENTORY
	public bool hasMermaidCannon = false;
    public bool hasMaceOfTrit = false;
    public bool hasRARLaser = false;
    public bool hasClownDrill = false;
    private int mermaidCount = 15;

    //ROCK THROW
    public GameObject rockNormal;
	public GameObject rockCharge;
    public bool throwRight = false;
	
	// Combo stuff
	private const int COMBO_LIGHT = 1;
	private const int COMBO_MED = 2;
	private const int COMBO_HEAVY = 3;
	private int[] comboOrder = new int[3];
	private bool inCombo = false;

    protected float xForce = 0;
    protected float yForce = 0;
    protected float friction = 0.5f;

    protected float dodgeSpeed = 0;
    protected float dodgeSpeedMax = 10;
    protected Vector2 dodgeDirection;

    protected float clownDrillVelocity = 0f;
    protected float clownDrillAccelPerSec = 12f;

    bool gamePaused = false;

    new void Start ()
	{
		_controller = GetComponent<MovementController2D> ();
        box = GetComponent<BoxCollider2D>();
		dc = GetComponentInChildren<PlayerDamageCollider> (true);
        
        _chargeBar = GameObject.Find("InGameUI").GetComponentInChildren<ChargeBarScript>();

        Debug.Log("******** NEW PLAYER - HAVE INSTANCE? " + GlobalControl.instance);

		if (GlobalControl.instance) {
            Debug.Log("******** YES! " + GlobalControl.instance.playerCP);
            _chargeBar.chargePercentage = GlobalControl.instance.playerCP;

            if (GlobalControl.instance.playerHP > 0) {
                playerHealth = GlobalControl.instance.playerHP;
            } else {
                playerHealth = 100;
            }
            GlobalControl.instance.resetPlayerStats();
        } else {
            _chargeBar.chargePercentage = 0;
            playerHealth = 100;
        }

		_anim = GetComponent<Animator> ();
		PlayerAbstractBehaviour[] pabs = _anim.GetBehaviours<PlayerAbstractBehaviour> ();
		for (var i = pabs.Length - 1; i >= 0; i--) {
			pabs [i].player = this;
		}

		facingLeft = false;
		facingRight = true;

		dc.gameObject.SetActive (false);

        if (stunIcon != null) {
            stunIcon.SetActive(false);
        }

        setState(PlayerStates.mobile);
	}

	public void onAnimationStateComboAttack (string state, int damage, int knockback)
	{
		switch (state) {
            case "comboAttackStart":
                inCombo = true;
                dc.damage = damage;
                dc.knockback = knockback;
                //setState(PlayerStates.attacking);
                break;
            case "comboAttackEnd":
                inCombo = false;
                setState(PlayerStates.mobile);
                comboReset();
                break;
        }
	}

	public override void Update ()
	{
        base.Update ();
    
        //INVINCIBILITY FRAMES
        if(tempInvulnTimer > 0)
        {
            tempInvulnTimer -= Time.deltaTime;
            if(tempInvulnTimer <= 0)
            {
                tempInvuln = false;
                tempInvulnTimer = 0;
            }
        }

        float targetVelX;
        float targetVelY;

        _anim.SetBool("InputA", inputA);
        _anim.SetBool("InputB", inputB);
        _anim.SetBool("InputC", inputC);
        _anim.SetBool("InputD", inputD);
        _anim.SetBool("RockThrowWindUp", inputDHold);
        _anim.SetBool("FaceLeft", facingLeft);
        _anim.SetBool("FaceRight", facingRight);
        _anim.SetBool("IsMoving", isMoving);
        _anim.SetInteger("Health", playerHealth);

        switch (state)
        {
            case PlayerStates.immobile:
                // Player cannot control character. Used for cutscenes. Does nothing here, and state gets set manually.
                break;
            case PlayerStates.mobile:
                // Primary state. Player is normal. Can move and attack based on inputs. Check for input code here.
                PlayerMovement();
                //PlayerMovement360();
                if (canAttack != false)
                {
                    PlayerAction();
                }
                break;
            case PlayerStates.attacking:
                PlayerAction();
                break;
            case PlayerStates.clownDrill:
                DrillMovement();
                //DrillMovement360();
                break;
            case PlayerStates.dodging:
                // Player is currently dodging. Cannot move. Must wait until dodge is complete, then set state back to moving.
                // TODO: Move player with dodge variable that shrinks (5...4...3...). Once it's 0, setState(PlayerStates.moving).
                targetVelX = dodgeDirection.x * dodgeSpeed;
                targetVelY = dodgeDirection.y * dodgeSpeed / 2;
                _vel.x = Mathf.SmoothDamp(_vel.x, targetVelX, ref velocityXSmoothing, .1f);
                _vel.y = Mathf.SmoothDamp(_vel.y, targetVelY, ref velocityYSmoothing, .1f);
                _controller.Move(_vel * Time.deltaTime);

                dodgeSpeed = Mathf.Max(0, dodgeSpeed - friction);
                break;
            case PlayerStates.shoulderdash:
                // Player is currently charging. Cannot move. Must wait until dodge is complete, then set state back to moving.
                targetVelX = dodgeDirection.x * dodgeSpeed;
                targetVelY = dodgeDirection.y * dodgeSpeed / 2;
                _vel.x = Mathf.SmoothDamp(_vel.x, targetVelX, ref velocityXSmoothing, .1f);
                _vel.y = Mathf.SmoothDamp(_vel.y, targetVelY, ref velocityYSmoothing, .1f);
                _controller.Move(_vel * Time.deltaTime);

                dodgeSpeed = Mathf.Max(0, dodgeSpeed - friction);
                break;
            case PlayerStates.stunned:
                // Player is stunned. Can't do anything. Must wait until stun is over, then set state to moving.
                // TODO: Make a stun timer. When stunned, set to the stun time. In here, subtract deltatime from th stun timer. When stunTimer <= 0, setState(PLayerStates.moving)
                targetVelX = dodgeDirection.x * dodgeSpeed;
                targetVelY = dodgeDirection.y * dodgeSpeed / 2;
                _vel.x = Mathf.SmoothDamp(_vel.x, targetVelX, ref velocityXSmoothing, .1f);
                _vel.y = Mathf.SmoothDamp(_vel.y, targetVelY, ref velocityYSmoothing, .1f);
                _controller.Move(_vel * Time.deltaTime);

                dodgeSpeed = Mathf.Max(0, dodgeSpeed - friction);
                break;
            case PlayerStates.dying:
                // Player is dead. Don't do anything. Game will end now.
                break;
            default:
                // Player is stateless. Do a dance?
                break;
        }
	}

    public void setPlayerState(PlayerStates newState)
    {
        if (_anim == null)
        {
            _anim = GetComponent<Animator>();
        }
        setState(newState);
    }

    protected void setState(PlayerStates newState)
    {
        if (state == newState)
        {
            // Already in this state. Do nothing.
            return;
        }

        Debug.Log("Set State: " + newState);

        // Switch to the new state.
        state = newState;

        // Do stuff to initialize new state.
        switch (newState)
        {
            case PlayerStates.mobile:
                _anim.SetBool("Dodge", false);
                inputA = inputB = inputC = inputD = false;
                break;
            case PlayerStates.dodging:
                _anim.SetBool("Dodge", true);
                dodgeSpeed = dodgeSpeedMax;
                dodgeDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                break;
            case PlayerStates.shoulderdash:
                _anim.SetTrigger("ShoulderDash");
                dodgeSpeed = dodgeSpeedMax;
                dodgeDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                SpawnDust();
                break;
            case PlayerStates.attacking:
                isMoving = false;

                //Spawn some dust. Cause we can.
                SpawnDust();               
                break;
            case PlayerStates.stunned:
                dodgeSpeed = dodgeSpeedMax;
                if(facingRight == true)
                {
                    dodgeDirection = new Vector2(-1, 0);
                }
                else
                {
                    dodgeDirection = new Vector2(1, 0);
                }
                break;
        }
    }

    void PlayerMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput.y > 0 || moveInput.y < 0)
        {
            isMoving = true;
        }
        if (moveInput.x < 0 && (!isMoving || facingRight))
        {
            isMoving = true;
            facingLeft = true;
            facingRight = false;

            // Position elements.
            //hitDustSpawn.transform.localPosition = new Vector3(Mathf.Abs(hitDustSpawn.transform.localPosition.x), hitDustSpawn.transform.localPosition.y, 0);
            hitDustSpawn.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x > 0 && (!isMoving || facingLeft))
        {
            isMoving = true;
            facingLeft = false;
            facingRight = true;

            // Position elements.
            //hitDustSpawn.transform.localPosition = new Vector3(Mathf.Abs(hitDustSpawn.transform.localPosition.x) * -1, hitDustSpawn.transform.localPosition.y, 0);
            hitDustSpawn.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput.x == 0 && moveInput.y == 0)
        {
            isMoving = false;
        }

        float targetVelX = moveInput.x * moveSpeed;
        float targetVelY = moveInput.y * moveSpeed;
        _vel.x = Mathf.SmoothDamp(_vel.x, targetVelX, ref velocityXSmoothing, .1f);
        _vel.y = Mathf.SmoothDamp(_vel.y, targetVelY, ref velocityYSmoothing, .1f);
        _controller.Move(_vel * Time.deltaTime);
    }

    /*void PlayerMovement360()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("X360HorizontalStick"), Input.GetAxisRaw("X360VerticalStick"));

        if (moveInput.y > 0 || moveInput.y < 0)
        {
            isMoving = true;
        }
        if (moveInput.x < 0 && (!isMoving || facingRight))
        {
            isMoving = true;
            facingLeft = true;
            facingRight = false;

            // Position elements.
            //hitDustSpawn.transform.localPosition = new Vector3(Mathf.Abs(hitDustSpawn.transform.localPosition.x), hitDustSpawn.transform.localPosition.y, 0);
            hitDustSpawn.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x > 0 && (!isMoving || facingLeft))
        {
            isMoving = true;
            facingLeft = false;
            facingRight = true;

            // Position elements.
            //hitDustSpawn.transform.localPosition = new Vector3(Mathf.Abs(hitDustSpawn.transform.localPosition.x) * -1, hitDustSpawn.transform.localPosition.y, 0);
            hitDustSpawn.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput.x == 0 && moveInput.y == 0)
        {
            isMoving = false;
        }

        float targetVelX = moveInput.x * moveSpeed;
        float targetVelY = moveInput.y * moveSpeed;
        _vel.x = Mathf.SmoothDamp(_vel.x, targetVelX, ref velocityXSmoothing, .1f);
        _vel.y = Mathf.SmoothDamp(_vel.y, targetVelY, ref velocityYSmoothing, .1f);
        _controller.Move(_vel * Time.deltaTime);
    }
    */

    void DrillMovement() {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput.x < 0 && facingRight) {
            facingLeft = true;
            facingRight = false;

            // Position elements.
            //hitDustSpawn.transform.localPosition = new Vector3(Mathf.Abs(hitDustSpawn.transform.localPosition.x), hitDustSpawn.transform.localPosition.y, 0);
            hitDustSpawn.transform.localScale = new Vector3(1, 1, 1);
        } else if (moveInput.x > 0 && facingLeft) {
            facingLeft = false;
            facingRight = true;

            // Position elements.
            hitDustSpawn.transform.localPosition = new Vector3(Mathf.Abs(hitDustSpawn.transform.localPosition.x) * -1, hitDustSpawn.transform.localPosition.y, 0);
            hitDustSpawn.transform.localScale = new Vector3(-1, 1, 1);
        }

        clownDrillVelocity = Mathf.Max(-moveSpeed * 2, Mathf.Min(moveSpeed * 2, 
            clownDrillVelocity + (clownDrillAccelPerSec * Time.deltaTime * (facingLeft ? -1f : 1f))
        ));
        _vel.x = clownDrillVelocity;
        _vel.y = Mathf.SmoothDamp(_vel.y, moveInput.y * moveSpeed, ref velocityYSmoothing, .1f);
        _controller.Move(_vel * Time.deltaTime);
    }

    /*void DrillMovement360()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("X360HorizontalStick"), Input.GetAxisRaw("X360VerticalStick"));

        if (moveInput.x < 0 && facingRight)
        {
            facingLeft = true;
            facingRight = false;

            // Position elements.
            //hitDustSpawn.transform.localPosition = new Vector3(Mathf.Abs(hitDustSpawn.transform.localPosition.x), hitDustSpawn.transform.localPosition.y, 0);
            hitDustSpawn.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x > 0 && facingLeft)
        {
            facingLeft = false;
            facingRight = true;

            // Position elements.
            hitDustSpawn.transform.localPosition = new Vector3(Mathf.Abs(hitDustSpawn.transform.localPosition.x) * -1, hitDustSpawn.transform.localPosition.y, 0);
            hitDustSpawn.transform.localScale = new Vector3(-1, 1, 1);
        }

        clownDrillVelocity = Mathf.Max(-moveSpeed * 2, Mathf.Min(moveSpeed * 2,
            clownDrillVelocity + (clownDrillAccelPerSec * Time.deltaTime * (facingLeft ? -1f : 1f))
        ));
        _vel.x = clownDrillVelocity;
        _vel.y = Mathf.SmoothDamp(_vel.y, moveInput.y * moveSpeed, ref velocityYSmoothing, .1f);
        _controller.Move(_vel * Time.deltaTime);
    }
    */

    public void PlayerAction()
    {
        if (Input.GetButtonDown("Dodge") || Input.GetButtonDown("X360BumperL"))
        {
            actionDodge();
        }

        if (Input.GetButtonDown("InputA") || Input.GetButtonDown("X360X"))
        {
            attackLight();
        }
        else if (!Input.GetButton("InputA") || !Input.GetButtonDown("X360X"))
        {
            inputA = false;
        }

        if (Input.GetButtonDown("InputB") || Input.GetButtonDown("X360Y"))
        {
            attackMed();
        }
        else if (!Input.GetButton("InputB") || !Input.GetButtonDown("X360Y"))
        {
            inputB = false;
        }

        if (Input.GetButtonDown("InputC") || Input.GetButtonDown("X360B"))
        {
            attackHeavy();
        }
        else if (!Input.GetButton("InputC") || !Input.GetButtonDown("X360B"))
        {
            inputC = false;
        }

        if (Input.GetButtonDown("InputD") || Input.GetButtonDown("X360A"))
        {
            attackCharged();
            throwRight = facingRight;
        } else if (!Input.GetButton("InputD") || !Input.GetButtonDown("X360A"))
        {
            inputD = false;
        }


        if (Input.GetButton("InputD") || Input.GetButton("X360A"))
        {
            if (_chargeBar != null && state != PlayerStates.attacking && _chargeBar.chargePercentage < 100f)
            {
                inputDHold = true;
            }
        }
        else {
            inputDHold = false;
        }

        bool doubleTapRight = false;
        #region doubleTapRight;
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            if(Time.time < _doubleTapTimeRight + 0.2f)
            {
                doubleTapRight = true;
            }
            _doubleTapTimeRight = Time.time;
        }
        #endregion
        bool doubleTapLeft = false;
        #region doubleTapLeft;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time < _doubleTapTimeLeft + 0.2f)
            {
                doubleTapLeft = true;
            }
            _doubleTapTimeLeft = Time.time;
        }
        #endregion
        if (doubleTapRight)
        {
            setState(PlayerStates.shoulderdash);
        }

        if (doubleTapLeft)
        {
            setState(PlayerStates.shoulderdash);
        }
    }

    void attackLight ()
	{
		comboAdd (COMBO_LIGHT);

		if (comboOrder [0] == COMBO_LIGHT && comboOrder [1] == COMBO_LIGHT) {
			// Flurry
			_anim.SetTrigger ("OraOra");
			comboReset ();
		} else if (!inCombo) {
			// Normal
			inputA = true;
		}

        setState(PlayerStates.attacking);
	}

	void attackMed ()
	{
		comboAdd (COMBO_MED);
		if (comboOrder [0] == COMBO_LIGHT && comboOrder [1] == COMBO_MED) {
			// Power kick
			_anim.SetTrigger ("PowerKick");
			comboReset ();
		} else if (!inCombo) {
			// Normal
			inputB = true;
		}

        setState(PlayerStates.attacking);
    }

	void attackHeavy ()
	{
		comboAdd (COMBO_HEAVY);

		if (comboOrder [0] == COMBO_LIGHT && comboOrder [1] == COMBO_LIGHT) {
			// Stomp
			_anim.SetTrigger ("Stomp");
			comboReset ();
		} else if (!inCombo) {
			// Normal
			inputC = true;
		}

        setState(PlayerStates.attacking);
    }

	void attackCharged ()
	{
        Debug.Log("ChargeBar: " + _chargeBar.chargePercentage);
		if (_chargeBar != null && _chargeBar.chargePercentage >= 100) {

            if (hasMermaidCannon) {
                _anim.SetTrigger("MermaidCannon");
                StartCoroutine(fireMermaidCannon());
            } else if (hasClownDrill) {
                _anim.SetTrigger("ClownDrill");
                StartCoroutine(fireClownDrill());
            } else if (hasMaceOfTrit) {
                _anim.SetTrigger("Mace");
                StartCoroutine(fireMaceOfTrit());
            } else if (hasRARLaser) {
                _anim.SetTrigger("Staff");
                StartCoroutine(fireRARLaser());
            } else {
				inputD = true;
				dc.damage = 100;
				dc.type = AbstractDamageCollider.DamageType.heavy;
			}
			_chargeBar.IncreaseChargePercentage (-200);
            setState(PlayerStates.attacking);
        }
	}

    void actionDodge()
    {
        inputDodge = true;
        _anim.SetBool("Dodge", true);
        setState(PlayerStates.dodging);
    }

	void comboAdd (int combo)
	{
		comboOrder [0] = comboOrder [1];
		comboOrder [1] = comboOrder [2];
		comboOrder [2] = combo;
	}

	void comboReset ()
	{
		comboOrder [0] = 0;
		comboOrder [1] = 0;
		comboOrder [2] = 0;
	}

    void OnTriggerStay2D(Collider2D other)
    {
        EnemyDamageCollider collider = other.GetComponent<EnemyDamageCollider>();
        if (collider != null)
        {
            damage(collider.damage, collider.type, collider.knockback);
        }
    }
    
	void OnTriggerEnter2D (Collider2D other)
	{
        EnemyDamageCollider collider = other.GetComponent<EnemyDamageCollider>();
        if (collider != null)
        {
            damage(collider.damage, collider.type, collider.knockback);
        }

        // TODO: For other powerups, you will check their AbstractPowerup Class 
        // and find out what effects it has from there.
        // Eg; Check effect state (health, attack, defence, etc) and level (1, 10, 100) and raise the player's
        // state by that level. If you want to lower, then either create a boolean "raise" or a "PowerDown" object.
        if (other.tag == "PowerUp")
        {
            string itemName = other.gameObject.name.Replace("(Clone)", "");
            Debug.Log("Found Item: " + itemName);
            if (itemName == "ChickenTendies") {
                Debug.Log(other);
                playerHealth += 20;
                base.playSound(AbstractClass.sfx.omnom, false);
                Destroy(other.gameObject);
            }
            else if (itemName == "ChargePart")
            {
                Debug.Log(other);
                _chargeBar.chargePercentage += 20;
                base.playSound(AbstractClass.sfx.omnom, false);
                Destroy(other.gameObject);
            }
            else if (itemName == "ChargeFull")
            {
                Debug.Log(other);
                _chargeBar.chargePercentage += 100;
                base.playSound(AbstractClass.sfx.omnom, false);
                Destroy(other.gameObject);
            }
        }
	}

	public void ThrowRock ()
	{
        Instantiate (rockNormal, dc.transform.position, dc.transform.rotation);
	}

	public void ThrowRockCharged ()
	{
		Instantiate (rockCharge, dc.transform.position, dc.transform.rotation);
	}

	public override void damage (int damage, AbstractDamageCollider.DamageType type, int knockback)
	{
        if (tempInvuln != true)
        {
            // Hit a player! Do death!
            tempInvuln = true;
            tempInvulnTimer = 1f;

            if (type == AbstractDamageCollider.DamageType.stunAttack) {
                StartCoroutine(stunAttack());
            } else if (type == AbstractDamageCollider.DamageType.stunMove) {
                StartCoroutine(stunMove());
            } else if (type == AbstractDamageCollider.DamageType.stunAll) {
                StartCoroutine(stunAttack());
                StartCoroutine(stunMove());
            } else {
                // Normal hit. Show knockback anim.
                _anim.SetTrigger("IsHit");
                setState(PlayerStates.stunned);
            }

            if (playerHealth > 0) {
			    playerHealth -= damage;
			    if (playerHealth <= 0) {
                    // Player dead, yo.
                    SendMessageUpwards("playerDied", null, SendMessageOptions.DontRequireReceiver);
			    }
		    }
        }
	}

    void SpawnDust () {
        GameObject dust = Instantiate(hitDust);
        DustParticleEffect de = dust.GetComponent<DustParticleEffect>();
        de.direction = facingLeft ? 1 : -1;
        dust.transform.parent = hitDustSpawn.transform;
        dust.transform.position = hitDustSpawn.transform.position;
    }

    public IEnumerator stunAttack () {
        if (stunIcon != null) { stunIcon.SetActive(true); }
        canAttack = false;
        yield return new WaitForSeconds(stunTime);
        canAttack = true;
        if (stunIcon != null) { stunIcon.SetActive(false); }
    }

    public IEnumerator stunMove () {
        _anim.SetTrigger("Stunned");
        setState(PlayerStates.stunned);
        yield return new WaitForSeconds(stunTime);
        setState(PlayerStates.mobile);
        _anim.SetTrigger("Unstunned");
    }

    protected IEnumerator fireMermaidCannon ()
	{
        earnKills = false;
        yield return new WaitForSeconds (1f);

		GameObject go = Instantiate (mermaidBomb);
		//TODO: Random between Mermaid and Merman.
		MermaidBomb firstShot = go.GetComponent<MermaidBomb> ();
		firstShot.life = 1f;
		int shotXVel = 0;
		if (facingLeft) {
			shotXVel = -1;
		} else {
			shotXVel = 1;
		}
		firstShot.setSpawnAndTarget (new Vector2 (transform.position.x, transform.position.y), new Vector2 (transform.position.x + (shotXVel * 10f), transform.position.y + 20f));
		firstShot.transform.parent = transform.parent;
		playSound (sfx.mermaid, false);

		yield return new WaitForSeconds (1.3f);

		MermaidBomb bomb;
		float xPos;
		float left = LevelBoundary.left;
		float right = LevelBoundary.left + Mathf.Max (LevelBoundary.topWidth, LevelBoundary.bottomWidth);
		float top = LevelBoundary.bottom + LevelBoundary.height;
		float bottom = LevelBoundary.bottom;
		float y = firstShot.transform.position.y;
		MermaidBomb[] cluster = new MermaidBomb[mermaidCount];
		for (int i = 0; i < mermaidCount; i++) {
			go = Random.value > .5 ? Instantiate(mermanBomb) : Instantiate(mermaidBomb); 
			bomb = cluster [i] = go.GetComponent<MermaidBomb> ();
			bomb.life = Random.Range (1f, 2f);
			xPos = Random.Range (left, right);
			bomb.setSpawnAndTarget (new Vector2 (xPos, y + Random.Range (-10f, 10f)), new Vector2 (xPos, Random.Range (top, bottom)));
			bomb.transform.parent = transform.parent;
			yield return new WaitForSeconds (.01f);
		}
        earnKills = true;
    }

    protected IEnumerator fireClownDrill () {
        tempInvuln = true;
        tempInvulnTimer = 8f;
        earnKills = false;
        yield return new WaitForSeconds(8f);
        _anim.SetTrigger("ClownDrillComplete");
        earnKills = true;
    }

    protected IEnumerator fireMaceOfTrit () {
        yield return new WaitForSeconds(.01f);
    }

    protected IEnumerator fireRARLaser () {
        yield return new WaitForSeconds(.01f);
    }
}
