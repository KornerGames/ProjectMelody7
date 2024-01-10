using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Zac
{

    public class BaseCharacterStats : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        private CharacterStatsModelSO statSO;

        [SerializeField]
        private TextMeshPro textStatChange;

        [Space]

        [SerializeField]
        private UnityEvent eventOnHealthUpdate;

        [SerializeField]
        private UnityEvent eventOnDeath;

        #endregion //Inspector Fields

        #region Other Fields

        [SerializeField]
        [ReadOnly]
        private CharacterStatsModel stats = new CharacterStatsModel();

        private Action onDeath;
        private Action onHealthUpdate;

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
        public void RegisterOnHealthUpdate(Action listener) => onHealthUpdate += listener;

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

            ShowTextStatChange((tempHP > stats.hitPoints), value);

            if (stats.hitPoints <= 0)
            {
                onDeath?.Invoke();
                eventOnDeath?.Invoke();
            }
            else
            {
                onHealthUpdate?.Invoke();
                eventOnHealthUpdate?.Invoke();
            }
        }

        #endregion //Public API

        #region Client Impl

        private void ShowTextStatChange(bool isNegative, int value)
        {
            if (textStatChange == null)
            {
                return;
            }

            textStatChange.gameObject.SetActive(false);

            textStatChange.color = isNegative ? Color.red : Color.blue;
            textStatChange.text = (isNegative ? "-" : "+") + value;
            textStatChange.gameObject.SetActive(true);
        }

        #endregion //Client Impl
    }

}