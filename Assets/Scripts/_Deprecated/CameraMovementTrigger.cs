﻿using System.Collections;
using UnityEngine;

public class CameraMovementTrigger : MonoBehaviour
{
    public int camIndex;
    public float moveTime;
    public GameObject otherTrigger;

    private bool canMove;

    private void Start()
    {
        canMove = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && canMove == true)
        {
            StartCoroutine(ActivateCameraMovement());
        }
    }

    private IEnumerator ActivateCameraMovement()
    {
        StartCoroutine(CameraController.instance.SetCameraPos(camIndex, moveTime));

        canMove = false;

        while (CameraController.instance.camInPosition != true)
        {
            yield return null;
        }

        otherTrigger.SetActive(true);
        gameObject.SetActive(false);

        canMove = true;
    }
}