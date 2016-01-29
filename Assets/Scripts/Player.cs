using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int vita;
    public int danno;
    public int velocità;
    public int velocitàAttacco;
    public int raggio;

    public void move (Vector3 movimento)
    {
        movimento = movimento.normalized;
        movimento = movimento * velocità * Time.deltaTime;
        transform.position = transform.position + movimento;
    }

    public void attack()
    {
        GameObject fireball = GameObject.FindWithTag("FireBall");
        fireball.SetActive(true);
        fireball.transform.position = fireball.transform.position + new Vector3(0, velocitàAttacco * Time.deltaTime, 0);
        Destroy(fireball, 0.5f);
        Instantiate(fireball);
        fireball.SetActive(false);
    }



}
