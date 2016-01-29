using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int vita;
    public int danno;
    public int velocità;

    public void move (Vector3 movimento)
    {
        gameobject.transform.position = gameobject.transform.position + movimento;
    }
}
