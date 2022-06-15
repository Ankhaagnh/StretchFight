using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncentiveShower : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Image[] stars;
    
    private float remainingHPpercent;

    void Start()
    {
        remainingHPpercent = player.GetComponent<IKController>().Health * 0.01f;
        StartCoroutine(ShowStars());
    }

    IEnumerator ShowStars()
    {
        yield return new WaitForSeconds(0.5f);
        if(remainingHPpercent >= 0.8f)
        {
            for (var i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (remainingHPpercent >= 0.5f)
        {
            for (var i = 0; i < stars.Length - 1; i++)
            {
                stars[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }
        }
        if (remainingHPpercent >= 0f)
        {
            for (var i = 0; i < stars.Length - 2; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }
    }

}
