using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Encounter : MonoBehaviour, IPointerDownHandler
{
    public enum State
    {
        Locked,
        CanVisit,
        Visited,
    }

    public Renderer meshRenderer;
    public CharacterSet enemySet;
    public Color lockedColor;
    public Color canVisitColor;
    public Color visitedColor;
    public bool hasBeenCleared;


    public State state;

    public void SetState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.Locked:
                {
                    meshRenderer.material.color = lockedColor; 
                    break;
                }
            case State.CanVisit:
                {
                    meshRenderer.material.color = canVisitColor;
                    break;
                }
            case State.Visited:
                {
                    meshRenderer.material.color = visitedColor;
                    break;
                }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state == State.CanVisit)
        {
            MapManager.instance.MoveParty(this);
        }
    }
}