using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Turret", menuName = "Game/CD_Turret", order = 0)]
    public class CD_Turret : ScriptableObject
    {
        public TurretData TurretData;
    }
}