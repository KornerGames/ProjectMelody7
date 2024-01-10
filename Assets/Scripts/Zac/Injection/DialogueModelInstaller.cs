using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Zac
{
    public class DialogueModelInstaller : MonoInstaller, 
        IDialogue.IGetter, IDialogue.ISetter
    {

        #region Inspector Fields

        [Header("UI Elements")]

        [SerializeField]
        private GameObject rootUI;

        [Space]

        //[SerializeField]
        //private TextMeshProUGUI textDialogue;

        [SerializeField]
        private LetterByLetterTextAnimator textDialogue;

        [SerializeField]
        private Button buttonNext;

        #endregion //Inspector Fields

        #region Other Fields

        private DialogueLineModel activeLine;
        private int activeLineIndex;

        #endregion //Other Fields

        #region Unity Callbacks

        private void Awake()
        {
            buttonNext.onClick.AddListener(OnNextLine);
            Hide();
        }

        #endregion //Unity Callbacks

        #region Public API

        public override void InstallBindings()
        {
            Container.BindInstance<IDialogue.IGetter>(this);
            Container.BindInstance<IDialogue.ISetter>(this);
        }

        public void Show(DialogueLineModel lineModel)
        {
            activeLine = lineModel;
            activeLineIndex = -1;

            rootUI.SetActive(true);

            OnNextLine();
        }

        public void Hide() => rootUI.SetActive(false);

        #endregion //Public API

        #region Client Impl

        private void OnNextLine()
        {
            if (activeLine == null)
            {
                Hide();
                return;
            }

            activeLineIndex++;

            if (activeLineIndex == activeLine.lines.Length)
            {
                Hide();
                return;
            }

            //textDialogue.text = activeLine.lines[activeLineIndex];
            textDialogue.AnimateText(activeLine.lines[activeLineIndex]);
        }

        #endregion //Client Impl

    }

}
