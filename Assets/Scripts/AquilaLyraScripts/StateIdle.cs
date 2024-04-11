using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState
{
    [SerializeField] private Transform randomTarget;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float startTime;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float minX = 114f;
    [SerializeField] private float maxX = 140f;
    [SerializeField] private float minY = -24f;
    [SerializeField] private float maxY = -10f;

    [SerializeField] private Transform transform;
    [SerializeField] IState stealState;
    [SerializeField] IState swapState;

    [SerializeField] private bool finished;
    public StateIdle(Transform t, Transform rt) {
        transform = t;
        randomTarget = rt;
        finished = false;
    }
    public void setNext(IState steal, IState swap) {
        stealState = steal;
        swapState = swap;
    }
    public void Enter() {
        Debug.Log("entering idle");
        randomTarget.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        startTime = Time.time;
        finished = false;
        StealNotes.setStolenNote(null);
    }
    public void Execute() {
        if (changeTarget() || transform.position == randomTarget.position) {
            Enter();
        }
        transform.position = Vector2.MoveTowards(transform.position, randomTarget.position, Time.deltaTime * moveSpeed);
        if (ChildNoteScript.correctNotes.Count > 0) {
            finished = true;
        }
    }
    public IState getNext() {
        return stealState;
    }
    public void Exit() {
        Debug.Log("switching out of Idle");
        finished = true;
    }
    public bool Finished() {
        return finished;
    }
    private bool changeTarget() {
        return Time.time >= startTime + duration;
    }
}
