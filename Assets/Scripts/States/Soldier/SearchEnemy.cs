using System;
using Abstract;
using AIBrain;
using Enums;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace States.Soldier
{
    public class SearchEnemy : SoldierBaseStates
    {
        #region Self Variables
        #region Private Variables

        private SoldierAIWorker _manager;
        private NavMeshAgent _agent;
        private Vector3 _randomPosition;

        #endregion
        
        #endregion

        public SearchEnemy(ref SoldierAIWorker manager, ref NavMeshAgent agent)
        {
            _manager = manager;
            _agent = agent;
        }


        public override void EnterState()
        {
            _randomPosition = RandomPosition();
            _agent.SetDestination(_manager.SearchInitPosition.transform.position +_randomPosition);
        }

        public override void UpdateState()
        {
            if (_agent.stoppingDistance >= _agent.remainingDistance)
            {
                _manager.SwitchState(SoldierStates.SearchEnemy);
            }
            _manager.AnimSetFloat(_agent.velocity.magnitude);
        }

        private Vector3 RandomPosition()
        {
            return new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-5f, 20f));
        }
    }
}