using NaughtyAttributes;
using System;
using UnityEngine;

namespace Zac
{

    public class BaseCharacterStats : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        private CharacterStatsModelSO statSO;

        #endregion //Inspector Fields

        #region Other Fields

        [SerializeField]
        [ReadOnly]
        private CharacterStatsModel stats = new CharacterStatsModel();

        private Action onDeath;

        #endregion //Other Fields

        #region Unity Callbacks

        private void Awake()
        {
            if (statSO == null)
            {
                Debug.LogError($"{gameObject.name}.{GetType().Name}: " +
                    $"No Character Stats SO here!", gameObject);
                return;
            }

            statSO.model.TransferValuesTo(stats);
        }

        #endregion //Unity Callbacks

        #region Public API

        public CharacterStatsModel Stats => stats;

        public void RegisterOnDeath(Action listener) => onDeath += listener;

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

            if (stats.hitPoints <= 0)
            {
                onDeath?.Invoke();
            }
        }

        #endregion //Public API

        #region Client Impl



        #endregion //Client Impl
    }

}