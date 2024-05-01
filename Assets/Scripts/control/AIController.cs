using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] float confuseTime = 5f;

    private float timeSinceLastSawPlayer;

    private Vector3 beginPosition;

    GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        beginPosition = transform.position;
    }

    private void Update()
    {
        if (IsInRange())
        {

        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {

        }
        else
        {

        }

        UpdateTimer();
    }

    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    private void UpdateTimer()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
    }
}
