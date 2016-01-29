using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int vita;
    public int danno;
    public int velocità;
    public int raggio;

    public void move (Vector3 movimento)
    {
        gameobject.transform.position = gameobject.transform.position + movimento;
    }

    public bool CanSeeTarget(int raggio)
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject enemy = new GameObject.FindWithTag("Enemy");

        if (Distance(player.transform.position, player.transform.enemy) < raggio)
            return true;
        else
            return false;
		Vector3.di
	
}
