using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public int minFOV;
    public int maxFOV;

    Camera myCamera;
    int speed;
    enum CameraCollisionSideX
    {
        None,
        Right,
        Left
    }
    CameraCollisionSideX cameraCollisionSideX;
    enum CameraCollisionSideY
    {
        None,
        Up,
        Down
    }
    CameraCollisionSideY cameraCollisionSideY;
    Rigidbody myRigidbody;

    void Start()
    {
        myCamera = GetComponent<Camera>();
        cameraCollisionSideX = CameraCollisionSideX.None;
        cameraCollisionSideY = CameraCollisionSideY.None;
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
		speed = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().velocità;
        var players = GameObject.FindGameObjectsWithTag("Player");
        Vector3 centreOfMass = Vector3.zero;
        for (int i = 0; i < players.Length; i++)
        {
            centreOfMass += players[i].transform.position;
        }
        centreOfMass /= players.Length;

        Vector3 screenPosition = myCamera.WorldToScreenPoint(centreOfMass);
        Vector3 moveTo = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (cameraCollisionSideX == CameraCollisionSideX.None)
        {
            if (screenPosition.x > Screen.width * 0.52f)
                cameraCollisionSideX = CameraCollisionSideX.Right;
            else if (screenPosition.x < Screen.width * 0.48f)
                cameraCollisionSideX = CameraCollisionSideX.Left;
            else
                moveTo.x = centreOfMass.x;
        }
        else
        {
            if (cameraCollisionSideX == CameraCollisionSideX.Right && screenPosition.x < Screen.width * 0.5f)
                cameraCollisionSideX = CameraCollisionSideX.None;
            else if (cameraCollisionSideX == CameraCollisionSideX.Left && screenPosition.x > Screen.width * 0.5f)
                cameraCollisionSideX = CameraCollisionSideX.None;
        }

        if (cameraCollisionSideY == CameraCollisionSideY.None)
        {
            if (screenPosition.y > Screen.height * 0.52f)
                cameraCollisionSideY = CameraCollisionSideY.Up;
            else if (screenPosition.y < Screen.height * 0.48f)
                cameraCollisionSideY = CameraCollisionSideY.Down;
            else
                moveTo.z = centreOfMass.z;
        }
        else
        {
            if (cameraCollisionSideY == CameraCollisionSideY.Up && screenPosition.y < Screen.height * 0.5f)
                cameraCollisionSideY = CameraCollisionSideY.None;
            else if (cameraCollisionSideY == CameraCollisionSideY.Down && screenPosition.y > Screen.height * 0.5f)
                cameraCollisionSideY = CameraCollisionSideY.None;
        }

        Vector3 direction = moveTo - transform.position;
        if (cameraCollisionSideX == CameraCollisionSideX.None || cameraCollisionSideY == CameraCollisionSideY.None)
            myRigidbody.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }
}
