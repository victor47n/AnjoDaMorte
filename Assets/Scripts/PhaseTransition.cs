using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseTransition : MonoBehaviour
{

    public GameObject startPhasePanel;
    public GameObject endPhasePanel;
    public GameObject endGamePanel;
    private static PhaseTransition phaseTransition;

    // Start is called before the first frame update
    void Start()
    {
        phaseTransition = GameObject.FindWithTag("PhaseTransition").GetComponent(typeof(PhaseTransition)) as PhaseTransition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void Transition(int scene)
    {
        SceneManager.LoadScene(scene);
        StartEndPhase(true);
        //phaseTransition.startPhasePanel.SetActive(true);
        //LoadingController.CallLoading(1);
    }

    public static void StartEndPhase(bool isStart)
    {
        phaseTransition.StartCoroutine(StartEndPhaseCoroutine(isStart));
    }

    public static void EndGame()
    {
        phaseTransition.endGamePanel.SetActive(true);
    }

    private static IEnumerator StartEndPhaseCoroutine(bool isStart)
    {
        if(isStart) phaseTransition.startPhasePanel.SetActive(true);
        else phaseTransition.endPhasePanel.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        if (isStart) phaseTransition.startPhasePanel.SetActive(false);
        else 
        {
            phaseTransition.endPhasePanel.SetActive(false);
            if(LoadingController.Scene + 1 <= 4)
            {
                LoadingController.CallLoading(LoadingController.Scene + 1);
            }
            else
            {
                EndGame();
            }
        } 
    }
}
