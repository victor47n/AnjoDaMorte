using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownCamera))]
public class TopDownCameraEditor : Editor
{
    private TopDownCamera targetCamera;

    void OnEnable()
    {
        targetCamera = (TopDownCamera)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    void OnSceneGui()
    {
        /* Make sure we have a target first */
        if (!targetCamera.target)
        {
            return;
        }

        /* Storing target reference */
        Transform camTarget = targetCamera.target;

        /* Drawn distance disc */
        Handles.color = new Color(1f, 0f, 0f, 0.15f);
        Handles.DrawSolidDisc(targetCamera.target.position, Vector3.up, targetCamera.distance);

        Handles.color = new Color(1f, 1f, 0f, 0.75f);
        Handles.DrawWireDisc(targetCamera.target.position, Vector3.up, targetCamera.distance);

        /* Create slider handles to adjust camera properties */
        Handles.color = new Color(1f, 0f, 0f, 0.5f);
        targetCamera.distance = Handles.ScaleSlider(targetCamera.distance, camTarget.position, -camTarget.forward, Quaternion.identity, targetCamera.distance, 1f);
        targetCamera.distance = Mathf.Clamp(targetCamera.distance, 2f, float.MaxValue);

        Handles.color = new Color(0f, 0f, 1f, 0.5f);
        targetCamera.height = Handles.ScaleSlider(targetCamera.height, camTarget.position, Vector3.up, Quaternion.identity, targetCamera.height, 1f);
        targetCamera.height = Mathf.Clamp(targetCamera.height, 5f, float.MaxValue);

        /* Create labels */
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 15;
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.UpperCenter;

        Handles.Label(camTarget.position + (-camTarget.forward * targetCamera.distance), "Distancia", labelStyle);

        labelStyle.alignment = TextAnchor.MiddleRight;
        Handles.Label(camTarget.position + (Vector3.up * targetCamera.height), "Altura", labelStyle);
    }
}
