using System;
using System.Collections.Generic;
using GameControllers.FSM.Properties;

namespace GameControllers.FSM
{
    public class StateMachine
    {
        private IState _currentState;

        protected Dictionary<Type, IState> _states;
        
        public void EnterIn<TState>() 
            where TState : IState
        {
            if (_states.TryGetValue(typeof(TState), out IState state))
            {
                _currentState?.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }

        public void Update()
        {
            _currentState.Update();
        }
    }
}
