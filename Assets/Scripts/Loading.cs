using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour
{
    public float loadingTime;

    float whenToLoad;
    int level;

    void Update () {
        if (Time.realtimeSinceStartup > whenToLoad)
        {
            Application.LoadLevel(level);
            Destroy(gameObject);
            Destroy(gameObject.transform.parent.gameObject);
        }
	}

    public void LoadScene(int sceneNumber)
    {
        Time.timeScale = 1;
        gameObject.SetActive(true);
        whenToLoad = Time.realtimeSinceStartup + loadingTime;
        level = sceneNumber;
    }
}
