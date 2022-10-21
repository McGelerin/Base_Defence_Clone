using System;
using Abstract;
using AIBrain;
using DG.Tweening;
using Enums;
using Signals;
using UnityEngine;

namespace States.Hostage
{
    public class MoveToBarrack : HostageBaseStates
    {
        public override void EnterState(HostageAIBrain hostage)
        {
            hostage.Target = IdleSignals.Instance.onGetSoldierBarrack();
            hostage.Agent.SetDestination(hostage.Target.transform.position);
        }

        public override void UpdateState(HostageAIBrain hostage)
        {
            AnimStateCheck(hostage);
        }

        public override void OnTriggerEnterState(HostageAIBrain hostage, Collider other)
        {
            if (other.CompareTag("SoldierBarrack"))
            {
                IdleSignals.Instance.onHostageEntryBarrack?.Invoke();
                PoolSignals.Instance.onReleasePoolObject(PoolType.Hostage.ToString(),hostage.gameObject);
            }
        }
        
        private void AnimStateCheck(HostageAIBrain hostage)
        {
            hostage.AnimBoolState(HostageAnimState.Follow, hostage.Agent.velocity.magnitude > 0.1f);
        }
    }
}