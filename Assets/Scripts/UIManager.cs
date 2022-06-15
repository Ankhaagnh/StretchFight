using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameController controller;
    public GameObject gameOverScreen;

    void Update()
    {
        if (GameController.controller.gameOver) {
            gameOverScreen.SetActive(true);
        }
    }
    public void Restart() {
        GameController.controller.gameOver = false;
        SceneManager.LoadScene(0);
    }

    public void ShowProgression(bool condition)
    {
        StartCoroutine(Enabler(condition));

        // UI.transform.GetChild(0).GetComponent<UIProgressBAr>().IncrementProgress(hpPercent);
    }

    IEnumerator Enabler(bool condition)
    {
        yield return new WaitForSeconds(2f);
        UI.gameObject.SetActive(condition);
        controller.IncreaseNumber();
    }
}
