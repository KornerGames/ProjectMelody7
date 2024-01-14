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
            dialogueSetter.Show(lineModel);
        }

        #endregion //Client Impl

    }

}