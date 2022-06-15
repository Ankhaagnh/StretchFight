using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController controller;
    public bool gameOver = false;
    static int currentLevelNumber = 1;
    [SerializeField]   Text levelNumber;

    private void Awake()
    {
        levelNumber.text = currentLevelNumber.ToString();

        if (controller == null)
        {
            controller = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        if (controller != this)
            Destroy(this.gameObject);
    }

    public void IncreaseNumber()
    {
        currentLevelNumber += 1;
    }

    

}
