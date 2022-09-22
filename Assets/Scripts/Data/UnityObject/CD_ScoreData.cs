using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_ScoreData", menuName = "Game/CD_ScoreData", order = 0)]
    public class CD_ScoreData : ScriptableObject
    {
        public ScoreData ScoreData;
    }
}