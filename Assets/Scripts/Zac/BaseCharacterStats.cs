using UnityEngine;

namespace Zac
{

    [System.Serializable]
    public class CharacterStatsModel
    {



        public int hitPoints;
        public float moveDuration;

    }

    public class BaseCharacterStats : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        private CharacterStatsModel stats;

        #endregion //Inspector Fields

        #region Other Fields



        #endregion //Other Fields

        #region Unity Callbacks



        #endregion //Unity Callbacks

        #region Public API

        public void InflictHP(InflictHPType inflictType, int value)
        {
            var tempHP = stats.hitPoints;

            switch (inflictType)
            {
                case InflictHPType.Heal:
                    {
                        stats.hitPoints = Mathf.Clamp(stats.hitPoints + value,
                            ValueConstants.HP_MIN, ValueConstants.HP_MAX);
                        break;
                    }
                case InflictHPType.Damage:
                default:
                    {
                        stats.hitPoints = Mathf.Clamp(stats.hitPoints - value,
                            ValueConstants.HP_MIN, ValueConstants.HP_MAX);
                        break;
                    }
            }

            Debug.Log($"{gameObject.name}.{GetType().Name}: HP went from {tempHP} " +
                $"to {stats.hitPoints}", gameObject);
        }

        #endregion //Public API

        #region Client Impl



        #endregion //Client Impl
    }

}