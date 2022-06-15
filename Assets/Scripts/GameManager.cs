using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject currentPlayer;
    public GameObject currentAI;
    public GameObject[] prefabAI;
    public UIManager manager;

    bool hasShownResult;
    float timer = 0;
    int currentAIindex;
    AIproperties[] AIproperties;
    void Awake()
    {
        SetupAIProperties();
    }

    void Start()
    {
        currentAI = SpawnAI();
    }

    private void Update()
    {
        if(haveKnocked())
        {
            if (!hasShownResult)
            {
                manager.ShowProgression(true);
                hasShownResult = true;
            }
            return;
        } 

        if (currentAI.GetComponent<IKController>().Health <= 0) {
            AIproperties[currentAIindex].isDead = true;

            if (timer < 2)
            {
                timer += Time.deltaTime;
            }
            else {
                Destroy(currentAI);
                timer = 0;
                currentAI = SpawnAI();
            }
        }
    }
    private GameObject SpawnAI()
    {   
        int randomNumber = Random.Range(0,3);
        if(!AIproperties[randomNumber].hasBeenSpawned)
        {
            AIproperties[randomNumber].hasBeenSpawned = true;
            currentAIindex = randomNumber;
            return Instantiate(prefabAI[randomNumber]);
        }else
        {
            return SpawnAI();
        }
    }
    private void SetupAIProperties()
    {
        AIproperties = new AIproperties[prefabAI.Length];
        for (var i = 0; i < AIproperties.Length; i++)
        {
            AIproperties[i] = new AIproperties(i, false, false);
        }
    }

    private bool haveKnocked()
    {   
        float hpPercent = 0;

        foreach (var Ai in AIproperties)
        {
            if(Ai.isDead) return true;    
        }
        hpPercent = currentPlayer.GetComponent<IKController>().Health * 0.01f;
        return false;
    }

}



public class AIproperties
{
    public bool hasBeenSpawned;
    public int index;
    public bool isDead;

    public AIproperties (int _index, bool _hasBeenSpawned, bool isDead)
    {
        this.index = _index;
        this.hasBeenSpawned = _hasBeenSpawned;
        this.isDead = isDead;
    }
}
