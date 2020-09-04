using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor (typeof (FieldOfView))]

public class FieldOfViewEditor : Editor
{
    //private void OnSceneGUI()
    //{
    //    FieldOfView fov = (FieldOfView)target;

    //    Handles.color = Color.white;

    //    Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.viewRadius);

    //    if (!fov.facingRight)
    //    {
    //        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2 + 180, false);
    //        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2 - 180, false);

    //        Vector3 lineA = fov.transform.position + viewAngleA * fov.viewRadius;
    //        Vector3 lineB = fov.transform.position + viewAngleB * fov.viewRadius;

    //        Handles.DrawLine(fov.transform.position, lineA);
    //        Handles.DrawLine(fov.transform.position, lineB);

    //        Handles.color = Color.red;
    //        foreach (Transform visibleTarget in fov.visibleTargets)
    //        {
    //            Debug.Log("Ray");
    //            Handles.DrawLine(fov.transform.position, visibleTarget.position);
    //        }
    //    }

    //    if (fov.facingRight)
    //    {
    //        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false); //Flip +180
    //        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false); //Flip -180
    //        Vector3 lineA = fov.transform.position + viewAngleA * fov.viewRadius;
    //        Vector3 lineB = fov.transform.position + viewAngleB * fov.viewRadius;
    //        Handles.DrawLine(fov.transform.position, lineA);
    //        Handles.DrawLine(fov.transform.position, lineB);

    //        Handles.color = Color.red;
    //        foreach (Transform visibleTarget in fov.visibleTargets)
    //        {
    //            Handles.DrawLine(fov.transform.position, visibleTarget.position);
    //        }
    //    }
    //}
}