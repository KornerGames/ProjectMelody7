using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Zac
{

    public class TargetHPAction : BaseCharacterAction
    {

        #region Inspector Fields

        [Header("Target HP Config")]

        [SerializeField]
        private InflictHPType inflictType;

        [SerializeField]
        [MinMaxSlider(ValueConstants.HP_MIN, ValueConstants.HP_MAX)]
        private Vector2 valueRange;

        [Space]

        [SerializeField]
        [Required]
        private TargetDetector targetDetector;

        [Space]

        [SerializeField]
        private bool shouldAutoFire;

        #endregion //Inspector Fields

        #region Other Fields

        private CompositeDisposable disposable;

        #endregion //Other Fields

        #region Unity Callbacks

        private void OnEnable()
        {
            disposable = new CompositeDisposable();

            if (!shouldAutoFire)
            {
                return;
            }

            targetDetector.IsTargetDetected()
                .Subscribe(isDetected =>
                {
                    if (isDetected)
                    {
                        StartAction();
                    }
                    else
                    {
                        StopAction();
                    }
                })
                .AddTo(disposable);
        }

        private void OnDisable()
        {
            disposable.Clear();
            disposable.Dispose();
            disposable = null;
        }

        #endregion //Unity Callbacks

        #region Public API

        public void SetValueRange(Vector2 valueRange) => this.valueRange = valueRange;

        #endregion //Public API

        #region Client Impl

        protected override bool CanDoAction() => 
            targetDetector.IsTargetDetected().Value;


        protected override void DoActionLogic()
        {
            if (targetDetector.GetFirstTarget() == null)
            {
                return;
            }

            if (targetDetector.GetFirstTarget().TryGetComponent<BaseCharacterStats>(out var charStats))
            {
                Debug.Log($"{gameObject.name}.{GetType().Name} Inflicted " +
                    $"{inflictType} to target '{charStats.gameObject.name}'", gameObject);

                charStats.InflictHP(inflictType,
                    Random.Range((int)valueRange.x, (int)valueRange.y));
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}.{GetType().Name} No BaseCharacterStats found on target! Skipping action...", gameObject);
            }
        }

        #endregion //Client Impl

    }

}