using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Zac
{
    public class ThrowAction : BaseCharacterAction
    {

        #region Inspector Fields

        [Space]

        [SerializeField]
        [Required]
        private TargetDetector targetDetector;

        [Space]

        [SerializeField]
        private bool shouldAutoFire;

        [SerializeField]
        private Rigidbody2D prefabProjectile;

        [SerializeField]
        private Vector2 fireDirection;

        [SerializeField]
        private float fireForce;

        #endregion //Inspector Fields

        #region Other Fields

        private Vector3 originalProjectilePosition;
        private CompositeDisposable disposable;

        #endregion //Other Fields

        #region Unity Callbacks

        private void Awake()
        {
            originalProjectilePosition = prefabProjectile.transform.localPosition;
        }

        private void OnEnable()
        {
            disposable = new CompositeDisposable();

            if (!shouldAutoFire)
            {
                return;
            }

            targetDetector.IsTargetDetected()
                .Where(isDetected => isDetected)
                .Subscribe(_ => StartAction())
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



        #endregion //Public API

        #region Client Impl

        protected override void OnEndAction()
        {
            prefabProjectile.gameObject.SetActive(false);
        }

        protected override bool CanContinueAsLongAs() =>
            targetDetector.IsTargetDetected().Value;

        protected override bool CanDoAction() =>
            targetDetector.IsTargetDetected().Value;

        protected override void DoActionLogic()
        {
            if (targetDetector.GetFirstTarget() == null ||
                prefabProjectile.gameObject.activeInHierarchy)
            {
                return;
            }

            prefabProjectile.transform.localPosition = originalProjectilePosition;
            prefabProjectile.gameObject.SetActive(true);

            prefabProjectile.AddForce(fireDirection * fireForce, ForceMode2D.Impulse);
        }

        #endregion //Client Impl

    }

}