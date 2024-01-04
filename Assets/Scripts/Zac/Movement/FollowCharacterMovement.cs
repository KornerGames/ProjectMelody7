using NaughtyAttributes;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Zac
{

    public class FollowCharacterMovement : BaseCharacterMovement
    {

        #region Inspector Fields

        [SerializeField]
        [Required]
        private TargetDetector targetDetector;

        [SerializeField]
        [Range(0f, 100f)]
        private float stoppingDistance = 5f;

        #endregion //Inspector Fields

        #region Other Fields

        private CompositeDisposable disposable = new CompositeDisposable();

        #endregion //Other Fields

        #region Unity Callbacks

        private void OnEnable()
        {
            disposable = new CompositeDisposable();

            targetDetector.IsTargetDetected()
                .Subscribe(isDetected => {
                    if (isDetected)
                    {
                        DoMove();
                    }
                    else
                    {
                        DoStop();
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

        public override void DoMove()
        {
            if (!targetDetector.IsTargetDetected().Value)
            {
                DoStop();
                return;
            }

            base.DoMove();
            StartCoroutine(C_FollowTarget());
        }

        public override void DoStop()
        {
            base.DoStop();
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
