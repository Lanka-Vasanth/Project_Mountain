using System;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    public AICharacterCombatManager aiCharacterCombatManager;
    public AICharacterLocomotionManager aiCharacterLocomotionManager;

    [Header("NavMesh")]
    public NavMeshAgent navmeshAgent;

    [Header("Current State")]
    [SerializeField] AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;

    protected override void Awake()
    {
        base.Awake();
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        navmeshAgent = GetComponentInChildren<NavMeshAgent>();

        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);

        currentState = idle; 
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        ProcessStateMachine();
    }

    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);

        if(nextState != null)
        {
            currentState = nextState;
        }

        // POSITION/ROTATION SHOULD BE RESET AFTER STATE WAS PROCESSED
        navmeshAgent.transform.localPosition = Vector3.zero;
        navmeshAgent.transform.localRotation = Quaternion.identity;

        if (navmeshAgent.enabled)
        {
            Vector3 agentDestination = navmeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if(remainingDistance > navmeshAgent.stoppingDistance)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
        else
        {
            isMoving = false;
        }
    }


}
