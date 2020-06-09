using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTransition : MonoBehaviour
{

    public GameObject startPhasePanel;
    public GameObject endPhasePanel;
    private static PhaseTransition phaseTransition;

    // Start is called before the first frame update
    void Start()
    {
        phaseTransition = GameObject.Find("StartEndPhaseCanvas").GetComponent(typeof(PhaseTransition)) as PhaseTransition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Transition()
    {
        //phaseTransition.startPhasePanel.SetActive(true);
        //LoadingController.CallLoading(1);
    }

    public static void StartEndPhase(bool isStart)
    {
        phaseTransition.StartCoroutine(StartEndPhaseCoroutine(isStart));
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
            LoadingController.CallLoading(2);
        } 
    }
}
