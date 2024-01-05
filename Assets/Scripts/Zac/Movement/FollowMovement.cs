using NaughtyAttributes;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Zac
{

    public class FollowMovement : BaseCharacterMovement
    {

        #region Inspector Fields

        [Space]

        [SerializeField]
        [Required]
        private TargetDetector targetDetector;

        [SerializeField]
        [Range(0f, 100f)]
        private float stoppingDistance = 5f;

        [SerializeField]
        private bool shouldAutoFire;

        #endregion //Inspector Fields

        #region Other Fields

        private CompositeDisposable disposable = new CompositeDisposable();

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
                .Subscribe(isDetected => {
                    if (isDetected)
                    {
                        StartMove();
                    }
                    else
                    {
                        StopMove();
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

        private void FixedUpdate()
        {
            if (!isMoving)
            {
                return;
            }

            Vector2 target = targetDetector.GetFirstTarget().transform.position;
            if (Vector2.Distance(rigidBody.position, target) > stoppingDistance)
            {
                var direction = (target - rigidBody.position).normalized;
                rigidBody.MovePosition(rigidBody.position + direction * Time.fixedDeltaTime * moveDuration);
            }
        }

        #endregion //Unity Callbacks

        #region Public API

        public override void StartMove()
        {
            if (!targetDetector.IsTargetDetected().Value)
            {
                StopMove();
                return;
            }

            base.StartMove();
        }

        public override void StopMove()
        {
            base.StopMove();
            StopAllCoroutines();
        }

        #endregion //Public API

        #region Client Impl



        #endregion //Client Impl

    }

}
