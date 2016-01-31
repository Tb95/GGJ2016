using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public Image hearthImage;
    public Text pointsPrefab;
    public Canvas canvas;
    public Vector2 firstHearthPosition;
    public Vector2 offset;

    List<GameObject> hearts;
    int lastActiveHeart;
    Text points;

    void Start()
    {
        hearts = new List<GameObject>();
        lastActiveHeart = 0;

        points = Instantiate(pointsPrefab);
        points.transform.SetParent(canvas.transform);
        points.rectTransform.anchoredPosition = firstHearthPosition;
    }

    public void ChangeHeartsNumber(int n)
    {
        if (n < 0)
            return;

        if (n > lastActiveHeart)
        {
            while (lastActiveHeart < hearts.Count && lastActiveHeart < n)
            {
                hearts[lastActiveHeart].SetActive(true);
                lastActiveHeart++;
            }

            while (hearts.Count < n)
            {
                Image newHeart = Instantiate(hearthImage);
                newHeart.transform.SetParent(canvas.transform);
                newHeart.rectTransform.anchoredPosition = firstHearthPosition + (lastActiveHeart + 1) * offset;
                hearts.Add(newHeart.gameObject);
                lastActiveHeart++;
            }
        }
        else if(n < lastActiveHeart)
        {
            while (lastActiveHeart > n)
            {
                hearts[lastActiveHeart - 1].SetActive(false);
                lastActiveHeart--;
            }
        }
    }

    public void DeadEnemy(bool withCombo, int remainingHealth)
    {
        int previousPoints = int.Parse(points.text);
        points.text = (previousPoints + (withCombo ? remainingHealth : 1)).ToString();
    }
}
