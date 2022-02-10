using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FOVAnimator : MonoBehaviour
{
    public float FOVAnimIn;
    public float FOVAnimOut;
    public float restingFOV;
    public float sprintingFOV;
    bool sprintState = false;
    public void SprintStateChanged() 
    {
        sprintState = !sprintState;
        if (sprintState)
        {
            DOTween.To(() => Camera.main.fieldOfView, x => Camera.main.fieldOfView = x, sprintingFOV, FOVAnimIn);
        }
        else
        {
            DOTween.To(() => Camera.main.fieldOfView, x => Camera.main.fieldOfView = x, restingFOV, FOVAnimOut);
        }
    }
}
