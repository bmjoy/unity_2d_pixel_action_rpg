﻿using System.Linq;
using UnityEngine;
using static Assets.Scripts.Utils.Enums;

public class FleeingStrategy : MovingStrategy
{
    public float playerDetectionRange = 100f;
    public float fleeDistance = 60f;

    private float searchDelay = 1f;
    private float nextSearch = 0f;

    public static FleeingStrategy CreateComponent(GameObject where, float playerDetectionRange = 5f, float fleeDistance = 5f, float searchDelay = 1f)
    {
        FleeingStrategy strategy = where.AddComponent<FleeingStrategy>();
        strategy.playerDetectionRange = playerDetectionRange;
        strategy.fleeDistance = fleeDistance;
        strategy.searchDelay = searchDelay;
        strategy.Start();
        return strategy;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        switch (state)
        {
            case WalkingState.Standing:
            default:
                ProcessStance();
                break;
            case WalkingState.Walking:
                break;
        }

        base.FixedUpdate();
    }

    //TODO: JAKOŚ DZIWNIE CHODZI, COŚ JEST NIE TAK Z WEKTOREM UCIECZKI (PRZECHODZI CZASEM PRZEZ PLAYERA)
    private void ProcessStance()
    {
        if (Time.time < nextSearch)
            return;

        var target = SearchForTarget();
        if (target != null)
        {
            var vectorFromTarget = transform.position - target.transform.position;
            var destination = (transform.position + vectorFromTarget).normalized * fleeDistance;
            Move(transform.position + destination);
        }
    }

    private Collider2D SearchForTarget()
    {
        nextSearch = Time.time + searchDelay;
        var player = Physics2D.OverlapCircleAll(transform.position, playerDetectionRange, LayerMask.GetMask("Player"))
            .SingleOrDefault();

        return player;
    }
}
