using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Zac
{

    public class BaseCharacterAI : MonoBehaviour
    {

        #region Inspector Fields

        [Header("Base Config")]

        [SerializeField]
        [Required]
        protected BaseCharacterMovement movement;

        [SerializeField]
        [Required]
        protected BaseCharacterAction mainAction;

        [SerializeField]
        //[Required]
        protected TargetHPAction targetHPAction;

        [SerializeField]
        [Required]
        protected BaseCharacterStats stats;

        [Space]

        [SerializeField]
        [Required]
        protected TargetDetector targetDetector;

        [Space]

        [SerializeField]
        protected bool canMoveAndAttackSimultaneously;

        #endregion //Inspector Fields

        #region Other Fields

        protected CompositeDisposable disposable = new CompositeDisposable();

        #endregion //Other Fields

        #region Unity Callbacks

        protected virtual void Awake()
        {
            stats.RegisterOnDeath(OnDeath);
        }

        protected virtual void Start()
        {
            movement.SetMoveDuration(stats.Stats.moveDuration);

            if (targetHPAction != null) 
            {
                targetHPAction.SetValueRange(stats.Stats.targetHPValueRange);
            }
        }

        protected virtual void OnEnable()
        {
            disposable = new CompositeDisposable();

            if (canMoveAndAttackSimultaneously)
            {
                movement.StartMove();
            }
            else
            {
                targetDetector.IsTargetDetected()
                    .Subscribe(isDetected => {
                        if (isDetected)
                        {
                            movement.StopMove();
                            mainAction.StartAction();
                        }
                        else
                        {
                            movement.StartMove();
                            mainAction.StopAction();
                        }
                    })
                    .AddTo(disposable);
            }
        }

        protected virtual void OnDisable()
        {
            disposable.Clear();
            disposable.Dispose();
            disposable = null;

            movement.StopMove();
            mainAction.StopAction();
        }

        #endregion //Unity Callbacks

        #region Public API



        #endregion //Public API

        #region Client Impl

        private void OnDeath()
        {
            Debug.Log($"{gameObject.name}.{GetType().Name}: Just died!", gameObject);
            //TODO

            //gameObject.SetActive(false);
        }

        #endregion //Client Impl

    }

}