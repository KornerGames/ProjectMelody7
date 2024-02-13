using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Zac
{

    public class InitiateDialogAction : BaseCharacterAction
    {

        #region Inspector Fields

        [Space]

        [SerializeField]
        private bool canTurnToTarget;

        [SerializeField]
        [EnableIf("canTurnToTarget")]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        [EnableIf("canTurnToTarget")]
        private Sprite spriteLeft;

        [SerializeField]
        [EnableIf("canTurnToTarget")]
        private Sprite spriteRight;

        [SerializeField]
        [EnableIf("canTurnToTarget")]
        private Sprite spriteUp;

        [SerializeField]
        [EnableIf("canTurnToTarget")]
        private Sprite spriteDown;

        [Space]

        [SerializeField]
        [Required]
        private TargetDetector targetDetector;

        [SerializeField]
        private bool shouldAutoFire;

        [Space]

        [SerializeField]
        private DialogueLineModel lineModel;

        #endregion //Inspector Fields

        #region Other Fields

        [Inject]
        private IDialogue.ISetter dialogueSetter;

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

        public override void StopAction()
        {
            base.StopAction();
            dialogueSetter.Hide();
        }

        #endregion //Public API

        #region Client Impl

        protected override bool CanContinueAsLongAs() => false;

        protected override bool CanDoAction() => 
            targetDetector.IsTargetDetected().Value;

        protected override void DoActionLogic()
        {
            TurnToTarget();
            dialogueSetter.Show(lineModel);
        }

        private void TurnToTarget()
        {
            if (!canTurnToTarget)
            {
                return;
            }

            var heading = targetDetector.GetFirstTarget().transform.position 
                - gameObject.transform.position;
            Vector2 direction = heading / heading.magnitude;

            animator.enabled = false;

            //TODO improve this
            if (direction.y >= 0f && ((direction.x >= -0.5f) && (direction.x <= 0.5f)))
            {
                spriteRenderer.sprite = spriteUp;
            }
            else if (direction.y <= 0f && ((direction.x >= -0.5f) && (direction.x <= 0.5f)))
            {
                spriteRenderer.sprite = spriteDown;
            }
            else if (direction.x <= 0f && ((direction.y >= -0.5f) && (direction.y <= 0.5f)))
            {
                spriteRenderer.sprite = spriteLeft;
            }
            else if (direction.x >= 0f && ((direction.y >= -0.5f) && (direction.y <= 0.5f)))
            {
                spriteRenderer.sprite = spriteRight;
            }
        }

        #endregion //Client Impl

    }

}