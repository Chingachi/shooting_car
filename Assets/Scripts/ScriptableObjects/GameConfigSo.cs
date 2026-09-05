using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "SO/GameConfig")]
    public class GameConfigSo : ScriptableObject
    {
        public int BaseEnemiesAmount = 100;
        public int ExtraEnemiesPerLevel = 3;

        public int BaseLevelLength = 100;
        public int ExtraLevelLengthPerLevel = 5;

        public int BaseCarSpeed = 10;
        public int BaseProjectileSpeed = 100;
        public float BaseFireRateTime = 0.5f;
    }
}