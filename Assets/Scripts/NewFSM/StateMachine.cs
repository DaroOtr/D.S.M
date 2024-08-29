using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
   private StateNode current;
   private Dictionary<Type, StateNode> _nodes = new();
   /// <summary>
   /// Transitions that can happen in any time;
   /// </summary>
   private HashSet<ITransition> anyTransitions = new();
   
   
   public void Update()
   {
      var transition = GetTransition();
      if (transition != null)
         ChangeState(transition.To);
      
      current._state?.Update();
   }

   public void FixedUpdate()
   {
      current._state?.FixedUpdate();
   }

   public void SetState(Istate state)
   {
      current = _nodes[state.GetType()];
      current._state?.OnEnter();
   }
   
   
   private void ChangeState(Istate state)
   {
      if (state == current._state) return;

      var previousState = current._state;
      var nexState = _nodes[state.GetType()]._state;
      
      previousState?.OnExit();
      nexState?.OnEnter();
      current = _nodes[state.GetType()];
   }
   
   private ITransition GetTransition()
   {
      
      // Firs Search for the states that can transition on any time
      foreach (var transition in anyTransitions)
      {
         if (transition.Condition.Evaluate())
            return transition;
      }
      
      // Second Search for the transition that can happen under a condition
      foreach (var transition in current._transitions)
      {
         if (transition.Condition.Evaluate())
            return transition;
      }

      return null;
   }

   public void AddTransition(Istate from,Istate to ,Ipredicate condition)
   {
      GetOrAddNode(from).AddTransition(GetOrAddNode(to)._state, condition);
   }
   
   public void AddAnyTransition(Istate to ,Ipredicate condition)
   {
      anyTransitions.Add(new Transition(GetOrAddNode(to)._state, condition));
   }

   StateNode GetOrAddNode(Istate state)
   {
      var node = _nodes.GetValueOrDefault(state.GetType());

      if (node == null)
      {
         node = new StateNode(state);
         _nodes.Add(state.GetType(),node);
      }

      return node;
   }

   private class StateNode
   {
      // What is a HasSet https://www.bytehide.com/blog/hashset-csharp;
      public Istate _state { get; }

      public HashSet<ITransition> _transitions { get; }

      public StateNode(Istate _state)
      {
         this._state = _state;
         _transitions = new HashSet<ITransition>();
      }

      public void AddTransition(Istate to, Ipredicate condition)
      {
         _transitions.Add(new Transition(to, condition));
      }
   }
}
