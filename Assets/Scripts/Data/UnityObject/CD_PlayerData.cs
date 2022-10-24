using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Player", menuName = "Game/CD_Player", order = 0)]
    public class CD_PlayerData : ScriptableObject
    {
        public PlayerData Data;
        public StackData MoneyStackData;
        public StackData AmmoStackData;
    }
}