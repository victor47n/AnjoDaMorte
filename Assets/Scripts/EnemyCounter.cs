using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyCounter : MonoBehaviour
{

    public Text textUI;
    protected int max = 0;
    protected int alive = 0;
    
    public void AddToAliveEnemies(int enemiesNumber){
        max += enemiesNumber;
        alive += enemiesNumber;
    }

    public void DecreaseEnemyLives(){
        alive -= 1;
        if(alive == 13)
        {
            PhaseTransition.StartEndPhase(false);
        }
    }

    protected void GenerateText(){
        textUI.text = Convert.ToString(alive) + "/" + Convert.ToString(max);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GenerateText();
    }
}
