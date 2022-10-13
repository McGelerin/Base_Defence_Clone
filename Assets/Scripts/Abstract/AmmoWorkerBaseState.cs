using UnityEngine;

namespace Abstract
{
    public abstract class AmmoWorkerBaseState
    {
        public abstract void EnterState();

        public abstract void OnTriggerEnterState(Collider other);
    }
}