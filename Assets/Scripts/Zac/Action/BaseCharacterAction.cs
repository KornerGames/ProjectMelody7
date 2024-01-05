using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Zac
{

    public abstract class BaseCharacterAction : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        [Required]
        protected AudioSource audioSource;

        [SerializeField]
        [Required]
        protected Animator animator;

        [SerializeField]
        [AnimatorParam("animator")]
        protected string animTriggerAction;

        [Space]

        [SerializeField]
        protected GameObject actionFX;

        [SerializeField]
        protected AudioClip audioClip;

        [Space]

        [SerializeField]
        protected UnityEvent eventOnFinish;

        [Header("Config")]

        [SerializeField]
        [Range(0f, 10f)]
        protected float delay = 0f;

        [SerializeField]
        [Range(0f, 10f)]
        protected float duration = 2f;

        [SerializeField]
        protected bool isContinuous;

        #endregion //Inspector Fields

        #region Other Fields

        private Action onFinishListener;
        private bool isDoingAction;

        #endregion //Other Fields

        #region Unity Callbacks



        #endregion //Unity Callbacks

        #region Public API

        public bool IsDoingAction => isDoingAction;

        public void RegisterOnFinish(Action listener) => 
            onFinishListener += listener;

        [Button]
        public void StartAction()
        {
            if (!CanDoAction())
            {
                Debug.LogWarning($"{gameObject.name}.{GetType().Name} " +
                    $"Can't do action...", gameObject);

                isDoingAction = false;
                return;
            }

            StopAllCoroutines();
            StartCoroutine(C_Act());
        }

        public virtual void StopAction()
        {
            StopAllCoroutines();
        }

        #endregion //Public API

        #region Client Impl

        private IEnumerator C_Act()
        {
            Debug.Log($"{gameObject.name}.{GetType().Name} " +
                    $"Action executed...", gameObject);
            
            isDoingAction = true;
            animator.SetTrigger(animTriggerAction);

            yield return new WaitForSeconds(delay);

            DoActionLogic();
            audioSource.PlayOneShot(audioClip);

            if (actionFX != null)
            {
                actionFX.SetActive(true);
            }

            yield return new WaitForSeconds(duration);

            onFinishListener?.Invoke();
            eventOnFinish?.Invoke();
            isDoingAction = false;

            if (isContinuous)
            {
                StartCoroutine(C_Act());
                yield break;
            }
        }

        protected abstract bool CanDoAction();
        protected abstract void DoActionLogic();

        #endregion //Client Impl
    }

}