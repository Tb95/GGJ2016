using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    Camera myCamera;
    int speed;

    void Start()
    {
        myCamera = GetComponent<Camera>();
        speed = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().velocità;
    }
	
	void Update () {
        Vector3 screenPoition = myCamera.WorldToScreenPoint(target.position);

        Vector3 direction = Vector3.zero;
        if (screenPoition.y > Screen.height * 0.8)
            direction += new Vector3(0, 0, 1);
        if (screenPoition.y < Screen.height * 0.2)
            direction += new Vector3(0, 0, -1);
        if (screenPoition.x > Screen.width * 0.8)
            direction += new Vector3(1, 0, 0);
        if (screenPoition.x < Screen.width * 0.2)
            direction += new Vector3(-1, 0, 0);

        transform.position += direction.normalized * speed * Time.deltaTime;
    }
}
