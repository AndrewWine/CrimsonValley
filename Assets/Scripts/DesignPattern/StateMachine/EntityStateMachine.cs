using System.Collections.Generic;
using UnityEngine;

public class EntityStateMachine<TBlackboard> : MonoBehaviour where TBlackboard : MonoBehaviour
{
    [SerializeField] public State<TBlackboard> CurrentState;
    public List<State<TBlackboard>> listOfStates = new();
    public TBlackboard blackboard;
    public State<TBlackboard> StartingState;
    private bool isStateLocked = false;

    private void Awake()
    {
        if (blackboard == null)
        {
            blackboard = GetComponent<TBlackboard>();
        }

        if (blackboard == null)
        {
            Debug.LogError($"{gameObject.name}: Blackboard vẫn NULL sau khi GetComponent!", this);
        }

        if (StartingState == null)
        {
            Debug.LogError("StartingState is not assigned.");
            return;
        }

        State<TBlackboard>[] states = blackboard.GetComponentsInChildren<State<TBlackboard>>();
        listOfStates = new List<State<TBlackboard>>(states);

        foreach (var state in listOfStates)
        {
            state.Initialize(blackboard, this);
        }

        CurrentState = StartingState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        CurrentState.PhysicUpdate();
    }

    public void ChangeState(State<TBlackboard> newState)
    {
        if (isStateLocked) return;

        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void LockState()
    {
        isStateLocked = true;
    }
}
