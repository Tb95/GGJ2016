using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public int vita;
    public int danno;
    public int velocità;
    public int velocitàAttacco;
    public float raggio;
    [Range(0, 2)]
    public float rotationSpeed;
    public float attackRate;
    public GameObject bulletPrefab;

    Rigidbody myRigidbody;
    float nextAvailableTimeForAttack;
    Health health;

	public AudioClip antShoot;

	public Image gameOverImage;
	public bool shouldFade = false;
	float timeStartFade = 0;
	float timeBetween = 0.02f;
	float timeLastUpdate = 0;
	float timeToRestart = 4.0f;
	bool newSceneStarted = false;

    void Start ()
    {
        myRigidbody = GetComponent<Rigidbody>();
        nextAvailableTimeForAttack = Time.realtimeSinceStartup;
        health = GetComponent<Health>();
    }

    void Update()
    {
        health.ChangeHeartsNumber(vita);

		if (shouldFade) {
			Color c = gameOverImage.color;
			if ((c.a <= 0.98f) && (Time.time - timeLastUpdate >= timeBetween)) {
				timeLastUpdate = Time.time;
				Color newColor = new Color (c.r, c.g, c.b, c.a + 0.01f);
				c = newColor;
				gameOverImage.color = newColor;
			}
		}

		if (Time.time - timeStartFade > timeToRestart && !newSceneStarted && shouldFade) {
			newSceneStarted = true;
			Application.LoadLevel ("MainMenu");
		}
    }

    public void move (Vector3 movimento)
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), rotationSpeed);

        movimento = movimento.normalized;
        movimento = movimento * velocità * Time.deltaTime;
        myRigidbody.MovePosition(transform.position + movimento);

        myRigidbody.MoveRotation(rotation);
    }

    public void attack(InputManager.Side side)
    {
        if (Time.realtimeSinceStartup > nextAvailableTimeForAttack)
        {
			// Instantiate a bullet
            GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2, transform.rotation) as GameObject;
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * velocitàAttacco;
            bullet.GetComponent<BulletCollision>().player = gameObject;
            Destroy(bullet, raggio / velocitàAttacco);
            nextAvailableTimeForAttack = Time.realtimeSinceStartup + attackRate;

			// Play sound
			GetComponent<AudioSource>().clip = antShoot;
			GetComponent<AudioSource>().Play();
        }
    }

    public void Hit(int damage)
    {
        vita -= damage;
		health.ChangeHeartsNumber(vita);
        if (vita <= 0)
        {
            // Dieeee => GAMEOVER
			gameOver();
        }
    }

	void gameOver() {
		InputManager.numberOfPlayers--;
		if (InputManager.numberOfPlayers == 0) {
			// make spiders idle and disable spawner
			InputManager.possibleSpiderCombos.ForEach (sc => sc.spider.movement = Spider.Movement.idle);

			// show gameover plane and buttons
			timeStartFade = Time.time;
			shouldFade = true;
		} else {
			if (GetComponent<InputManager> ().playerNumber == 1) {
				GameObject player2 = GameObject.Find ("Player2");
				GameObject.Find ("Main Camera").GetComponent<CameraFollow> ().target = player2.transform;
				InputManager.possibleSpiderCombos.ForEach (sc => sc.spider.player = player2);
				Destroy (gameObject);
			} else {
				GameObject player1 = GameObject.Find ("Player1");
				InputManager.possibleSpiderCombos.ForEach (sc => sc.spider.player = player1);
				Destroy (gameObject);
			}
		}
	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Spider")
        {
            other.gameObject.GetComponent<Spider>().CollisionEnter(gameObject);
        }
    }
}