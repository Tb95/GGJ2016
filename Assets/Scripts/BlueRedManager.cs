using UnityEngine;
using System.Collections;

public class BlueRedManager : MonoBehaviour {
	public int beginSection2 = 10;

	public int beginSection3 = 20;

	public int beginSection4 = 30;
	bool isOk = false;
	bool goRight = true;
	int translateTimes = 200;
	bool firstTimeTranslate = true;
	int counter = 0;

	public int restartAfter = 40;

	void Update () {
        if (Time.timeScale > 0)
        {
			if (Time.time <= beginSection2) {

			} else if (Time.time > beginSection2 && Time.time <= beginSection3) {
				transform.Rotate (0, 0.5f, 0);
			} else if (Time.time > beginSection3 && Time.time <= beginSection4) {
				transform.Rotate (0, Mathf.Abs (Mathf.Sin (Time.time)), 0);
			} else if (Time.time > beginSection4 && Time.time <= restartAfter) {
				if (transform.eulerAngles.y >= 180.0f && transform.eulerAngles.y <= 355.0f) {
					transform.Rotate (0, 0.5f, 0);
				} else if (transform.eulerAngles.y < 180.0f && transform.eulerAngles.y >= 0.5f) {
					transform.Rotate (0, -0.5f, 0);
				} else if (!isOk) {
					transform.Rotate (0, -transform.eulerAngles.y, 0);
					isOk = true;
				} else if (goRight) {
					transform.Translate (-0.2f, 0, 0);
					counter++;
					if (counter >= translateTimes && firstTimeTranslate) {
						counter = 0;
						goRight = !goRight;
						firstTimeTranslate = false;
					} else if (counter >= translateTimes * 2) {
						counter = 0;
						goRight = !goRight;
					}
				} else {
					transform.Translate (0.2f, 0, 0);
					counter++;
					if (counter >= translateTimes * 2) {
						counter = 0;
						goRight = !goRight;
					}
				}
			} else if (!(transform.position.x >= -0.3 && transform.position.x <= 0.3)) {
				transform.Translate (0.2f * (-1) * Mathf.Sign (transform.position.x), 0, 0);
			} else {
				beginSection2 += restartAfter + 5;
				beginSection3 += restartAfter + 5;
				beginSection4 += restartAfter + 5;
			}
        }
	}
}
