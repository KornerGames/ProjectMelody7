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
        private TargetDetector targetDetector;

        [Space]

        [SerializeField]
        private bool shouldAutoFire;

        [SerializeField]
        private Rigidbody2D prefabProjectile;

        [SerializeField]
        private bool isDirectionDynamic;

        [SerializeField]
        [DisableIf("isDirectionDynamic")]
        private Vector2 fireDirection;

        [SerializeField]
        [EnableIf("isDirectionDynamic")]
        private Player playerTemp;

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

            if (!shouldAutoFire || (targetDetector == null))
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
            (targetDetector == null) ? true : 
            targetDetector.IsTargetDetected().Value;

        protected override bool CanDoAction() =>
            (targetDetector == null) ? true :
            targetDetector.IsTargetDetected().Value;

        protected override void DoActionLogic()
        {
            if (!isDirectionDynamic && (targetDetector != null) && 
                (targetDetector.GetFirstTarget() == null ||
                prefabProjectile.gameObject.activeInHierarchy))
            {
                return;
            }

            if (isDirectionDynamic)
            {
                FireToPresentDirection();
            }
            else
            {
                prefabProjectile.transform.localPosition = originalProjectilePosition;
                prefabProjectile.gameObject.SetActive(true);

                prefabProjectile.AddForce(fireDirection * fireForce, ForceMode2D.Impulse);
            }
        }

        private void FireToPresentDirection()
        {
            prefabProjectile.gameObject.SetActive(false);
            prefabProjectile.transform.position = transform.position;

            var dir = playerTemp.CachedDirection;

            prefabProjectile.constraints = (dir.x != 0) ?
                RigidbodyConstraints2D.FreezePositionY : RigidbodyConstraints2D.FreezePositionX;

            prefabProjectile.gameObject.SetActive(true);

            Debug.Log($"Direction: {dir} | Projectile Cosntraints: {prefabProjectile.constraints}");

            prefabProjectile.AddForce(dir * fireForce, ForceMode2D.Impulse);
        }

        #endregion //Client Impl

    }

}