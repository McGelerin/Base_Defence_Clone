using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Weapon", menuName = "Game/CD_Weapon", order = 0)]
    public class CD_Weapon : ScriptableObject
    {
        public SerializedDictionary<WeaponType, WeaponData> Weapons;
    }
}