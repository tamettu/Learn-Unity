using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    //=~=~=~=~=~=~=~=~=~=~=~=~FSM rule=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~
    public interface IState
    {
        void Enter();
        void Update(float dt);
        void Exit();
        void FixedUpdate(float dt);
        void LateUpdate(float dt);
    }
    public interface IEntity
    {
        StateMachine Fsm {get;}
    }
    public class StateMachine
    {
        private IState _currentState;
        public void ChangeState(IState newState)
        {
            if (_currentState != null) _currentState.Exit();
            _currentState = newState;
            if (_currentState != null) _currentState.Enter();
        }
        public void Update(float dt)
        {
            if (_currentState != null) _currentState.Update(dt);
        }
        public void FixedUpdate(float dt)
        {
            if (_currentState != null) _currentState.FixedUpdate(dt);
        }
        public void LateUpdate(float dt)
        {
            if (_currentState != null) _currentState.LateUpdate(dt);
        }
    }
    public abstract class BaseState<T> : IState where T : IEntity
    {
        protected T owner;
        public BaseState(T owner) => this.owner = owner;
        public abstract void Enter();
        public virtual void Update(float dt) {}
        public virtual void FixedUpdate(float dt) {}
        public virtual void LateUpdate(float dt) {}
        public abstract void Exit();
    }
    //=~=~=~=~=~=~=~=~=~=~=~=~End of FSM rule=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~
}