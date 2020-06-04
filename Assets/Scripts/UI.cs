using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI : MonoBehaviour
{

    private static String newEnemyLives;
    public Text enemyLives;

    public static void IncreaseEnemyLives () {
        newEnemyLives = Convert.ToString(Convert.ToInt32(newEnemyLives)+1);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyLives.text = "0";
        UI.newEnemyLives = "0";
    }

    // Update is called once per frame
    void Update()
    {
        enemyLives.text = UI.newEnemyLives;
    }
}
