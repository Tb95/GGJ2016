using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Spider : MonoBehaviour {

	public enum Movement {idle, chase, chaseZigZag, chaseArchs};

	public Movement movement = Movement.chase;
	// BASIC
	public int speed = 4;
	public float doNothingTriggerDistance = 0.3f;
	public float chaseTriggerDistance = 2.0f;
	public bool isDown = false;
	public InputManager.Side side;
	public int health = 3;
    public int resistance = 1;
	public float downTime = 2.0f;
	// ZIG ZAG
	public int minZigZagLength = 3;
	public int maxZigZagLength = 7;
	// ARCHS
	public int minArchLength = 3;
	public int maxArchLength = 7;
	public int archProbability = 40;
    [HideInInspector]
	public GameObject player;
	// COMBO
	public int comboLength = 3;
	ButtonsManager buttonsManager;
	public List<ButtonsManager.Button> comboList;
	// RADIUS FOR ATTACK
	public float radiusForAttack = 4.0f;
	// AUDIO CLIPS
	public AudioClip flippinSpider;
	public AudioClip spiderCatch;
	public AudioClip spiderKill;
	public AudioClip spiderFall;
	// BAD THINGS
	SpiderCombo spiderCombo;
    // ANIMATOR
    Animator animator;
    //SPAWNER
    SpawnGameObjects spawner;
    public GameObject comboTextPlayer1;
    public GameObject comboTextPlayer2;
	GameObject spiderTrail;
	public GameObject trail;

	// Use this for initialization
	void Start () {
        comboTextPlayer1 = spawner.comboTextPlayer1;
        comboTextPlayer2 = spawner.comboTextPlayer2;
		spiderTrail = spawner.spiderTrail;
		
		var players = GameObject.FindGameObjectsWithTag("Player");
        player = players[Random.Range(0, players.Length)];

		side = player.GetComponent<InputManager> ().getRandomSide ();
		buttonsManager = GameObject.FindGameObjectWithTag ("ButtonsManager").GetComponent<ButtonsManager>();
		comboList = buttonsManager.getRandomCombo(comboLength, side);
		spiderCombo = new SpiderCombo (comboList, this);
		InputManager.possibleSpiderCombos.Add(spiderCombo);

        animator = GetComponent<Animator>();

		// Play fall sound
		gameObject.GetComponent<AudioSource>().clip = spiderFall;
        gameObject.GetComponent<AudioSource>().PlayScheduled(2);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isDown)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                GetComponent<Collider>().enabled = true;

                float dist = Vector3.Distance(transform.position, player.transform.position);
                // If dist < 0.3 : do nothing
                // If 0.3 < dist < 2 : just chase
                // If 2 < dist : perform movement
                if (dist >= doNothingTriggerDistance && dist <= chaseTriggerDistance)
                {
                    chase(player);
                }
                else if (dist > chaseTriggerDistance)
                {
                    if (movement == Movement.chase)
                        chase(player);
                    else if (movement == Movement.chaseZigZag)
                        chaseZigZag(player);
                    else if (movement == Movement.chaseArchs)
                            chaseArchs(player);
                }
            }
        }

        // MANAGE IS DOWN
        else
        {
            GameObject player1 = null;
            GameObject player2 = null;
            foreach (var item in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (item.GetComponent<InputManager>().playerNumber == 1)
                    player1 = item;
                else
                    player2 = item;
            }

			// 1 - Mostra combo e trail se è quello down più vicino
            bool activateTrail1 = false;
            bool activateTrail2 = false;
            //PLAYER1
			List<SpiderCombo> sc = InputManager.possibleSpiderCombos.FindAll(aSc => aSc.spider.isDown);
			sc.Sort((c1, c2) => (int)Vector3.Distance(player1.transform.position, c1.spider.transform.position) - (int)Vector3.Distance(player1.transform.position, c2.spider.transform.position));
			if (sc [0].spider == this) {
                activateTrail1 = true;
				for (int i = 0; i < butsPlayer1.Count; i++) {
					butsPlayer1 [i].SetActive (true);
				}
			} else {
                activateTrail1 = false;

				for (int i = 0; i < butsPlayer1.Count; i++) {
					butsPlayer1 [i].SetActive (false);
				}
			}

            //PLAYER2
            if (player2 != null)
            {
                sc.Sort((c1, c2) => (int)Vector3.Distance(player2.transform.position, c1.spider.transform.position) - (int)Vector3.Distance(player2.transform.position, c2.spider.transform.position));
                if (sc[0].spider == this)
                {
                    activateTrail2 = true;
                    for (int i = 0; i < butsPlayer1.Count; i++)
                    {
                        butsPlayer2[i].SetActive(true);
                    }
                }
                else
                {
                    activateTrail2 = false;

                    for (int i = 0; i < butsPlayer1.Count; i++)
                    {
                        butsPlayer2[i].SetActive(false);
                    }
                }
            }

            trail.SetActive(activateTrail1 || activateTrail2);

            // 2 - Dopo tot tempo torna not down
            if ((Time.time - timeOfGettingDown) >= downTime)
            {
                isDown = false;

                // Reenable trigger that causes the player to lose health
                gameObject.GetComponent<CapsuleCollider>().enabled = true;

                //Animate turn
                animator.SetTrigger("Turn");

                //Destroy trail
				Destroy(trail.gameObject);

                //Destroy combo buttons
				for (int i = 0; i < butsPlayer1.Count; i++) {
					Destroy (butsPlayer1 [i]);
				}

                for (int i = 0; i < butsPlayer2.Count; i++)
                {
                    Destroy(butsPlayer2[i]);
                }
            }
        }
	}

	void chase(GameObject player) {
		transform.position += (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
		transform.LookAt (player.transform.position);
	}

	float deltaXForChaseZigZag = 0;

	void chaseZigZag(GameObject player) {
		if ((deltaXForChaseZigZag <= 0.3) && (deltaXForChaseZigZag >= -0.3)) {
			deltaXForChaseZigZag = (float)(Random.Range (minZigZagLength, maxZigZagLength + 1));
			if (Random.Range (0, 2) == 0) { // Once every two
				deltaXForChaseZigZag = -deltaXForChaseZigZag;
			}
		}
		float tempDeltaX;
		if (deltaXForChaseZigZag > 0) {
			tempDeltaX = speed * Time.deltaTime;
		} else {
			tempDeltaX = (-speed) * Time.deltaTime;
		}
		deltaXForChaseZigZag -= tempDeltaX;
		transform.position += ((player.transform.position - transform.position).normalized + (transform.right * tempDeltaX).normalized).normalized * speed * Time.deltaTime;
		transform.LookAt (player.transform.position);
	}

	float deltaXForChaseWandering = 0;
	int directionForChaseWandering = 0;

	void chaseArchs(GameObject player) {
		if ((deltaXForChaseWandering <= 0.3) && (Random.Range (0, 100 - archProbability) == 0)) {
			deltaXForChaseWandering = (float)(Random.Range (minArchLength, maxArchLength + 1));
			directionForChaseWandering = Random.Range (0, 2);
		}
		if (deltaXForChaseWandering > 0.3) {
			if (directionForChaseWandering == 0) {
				transform.position += transform.right * speed * Time.deltaTime;
				deltaXForChaseWandering -= speed * Time.deltaTime;
			} else {
				transform.position += transform.right * (-1) * speed * Time.deltaTime;
				deltaXForChaseWandering -= speed * Time.deltaTime;
			}
		} else {
			transform.position += (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
		}
		transform.LookAt (player.transform.position);
	}

	float lastAttackTime = 0;
	public float attackEveryTotSeconds = 0.5f;
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player" && (Time.time - lastAttackTime) > attackEveryTotSeconds) {
			other.gameObject.GetComponent<Player> ().Hit (1);
			lastAttackTime = Time.time;
		}
	}

	void OnCollisionStay(Collision other) {
		if (other.gameObject.tag == "Player" && (Time.time - lastAttackTime) > attackEveryTotSeconds) {
			other.gameObject.GetComponent<Player> ().Hit (1);

			// Play sound
			gameObject.GetComponent<AudioSource>().clip = spiderCatch;
			gameObject.GetComponent<AudioSource> ().Play ();

			lastAttackTime = Time.time;
		}
	}

	float timeOfGettingDown = 0;
    public List<GameObject> butsPlayer1 = new List<GameObject>();
    public List<GameObject> butsPlayer2 = new List<GameObject>();
	public void Hit(int damage, GameObject playerThatHit) {
		health -= damage;

		if (health <= 0) {
			InputManager.possibleSpiderCombos.Remove (spiderCombo);

			// Play sound
			player.GetComponent<AudioSource>().clip = spiderKill;
			player.GetComponent<AudioSource> ().Play ();

            playerThatHit.GetComponent<Health>().DeadEnemy(false);
            spawner.DeadEnemy();
			Destroy (gameObject);
		} else if (health % resistance == 0) {
			isDown = true;
			timeOfGettingDown = Time.time;

			// Create trail renderer
			trail = Instantiate (spiderTrail);
			trail.transform.position = transform.position + transform.right * 2.0f;
			trail.GetComponent<RotateAround> ().target = gameObject.transform;
			trail.GetComponent<RotateAround> ().vec = gameObject.transform.up;

			// Show combo label
            butsPlayer1.Clear();
            butsPlayer2.Clear();
			int distanceBetweenButtons = 50;
			for (int i = 0; i < spiderCombo.buttonsList.Count; i++) {
				ButtonsManager.Button b = spiderCombo.buttonsList [i];
                butsPlayer1.Add(buttonsManager.getGameObjectFromButton(b));

                if(comboTextPlayer2 != null)
                    butsPlayer2.Add(buttonsManager.getGameObjectFromButton(b));
			}
			for (int i = 0; i < butsPlayer1.Count; i++) {
				butsPlayer1 [i].transform.SetParent(comboTextPlayer1.transform);
                butsPlayer1[i].transform.position = comboTextPlayer1.transform.position - comboTextPlayer1.transform.up * (i + 1) * distanceBetweenButtons;

                if (comboTextPlayer2 != null)
                {
                    butsPlayer2[i].transform.SetParent(comboTextPlayer2.transform);
                    butsPlayer2[i].transform.position = comboTextPlayer2.transform.position - comboTextPlayer2.transform.up * (i + 1) * distanceBetweenButtons;
                }			
			}

			// Play sound
			gameObject.GetComponent<AudioSource>().clip = flippinSpider;
			gameObject.GetComponent<AudioSource> ().Play ();

			// Deactivate trigger that cause the player to lose health
			gameObject.GetComponent<CapsuleCollider>().enabled = false;

            //Animate turn
            animator.SetTrigger("Turn");

            //Attack who hit you
            player = playerThatHit;
		}
        else
        {
            // Play sound
            gameObject.GetComponent<AudioSource>().clip = flippinSpider;
            gameObject.GetComponent<AudioSource>().Play();

            //Attack who hit you
            player = playerThatHit;
        }
	}

    public void SetSpawner(SpawnGameObjects spawner)
    {
        this.spawner = spawner;
    }

    public SpawnGameObjects GetSpawner()
    {
        return spawner;
    }
}