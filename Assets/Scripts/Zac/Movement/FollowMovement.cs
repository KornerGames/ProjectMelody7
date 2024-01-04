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
            StartCoroutine(C_FollowTarget());
        }

        public override void StopMove()
        {
            base.StopMove();
            StopAllCoroutines();
        }

        #endregion //Public API

        #region Client Impl

        private IEnumerator C_FollowTarget()
        {
            isMoving = true;

            while (isMoving)
            {
                Vector2 target = targetDetector.GetFirstTarget().transform.position;
                if (Vector2.Distance(rigidBody.position, target) > stoppingDistance)
                {
                    var direction = (target - rigidBody.position).normalized;
                    rigidBody.MovePosition(rigidBody.position + direction * Time.deltaTime * moveDuration);
                }

                yield return null;
            }
        }

        #endregion //Client Impl

    }

}
