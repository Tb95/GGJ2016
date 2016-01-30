using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public Image hearthImage;
    public Canvas canvas;
    public Vector2 firstHearthPosition;
    public Vector2 offset;

    List<GameObject> hearts;
    int lastActiveHeart;

    void Start()
    {
        hearts = new List<GameObject>();
        lastActiveHeart = 0;
    }

    public void ChangeHeartsNumber(int n)
    {
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
                newHeart.rectTransform.anchoredPosition = firstHearthPosition + lastActiveHeart * offset;
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
}
