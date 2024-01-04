using NaughtyAttributes;
using UnityEngine;

namespace Zac
{

    public abstract class BaseCharacterMovement : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        [Required]
        protected Rigidbody2D rigidBody;

        [SerializeField]
        protected float moveDuration = 1;

        [Space]

        [SerializeField]
        [Required]
        protected Animator animator;

        [SerializeField]
        [AnimatorParam("animator")]
        protected string animBoolIdle;

        [SerializeField]
        [AnimatorParam("animator")]
        protected string animBoolMove;


        #endregion //Inspector Fields

        #region Other Fields

        protected bool isMoving;

        public bool IsMoving => isMoving;

        #endregion //Other Fields

        #region Unity Callbacks



        #endregion //Unity Callbacks

        #region Public API

        public virtual void SetMoveDuration(float speed) => moveDuration = speed;

        public virtual void StartMove()
        {
            animator.SetBool(animBoolIdle, false);
            animator.SetBool(animBoolMove, true);
            isMoving = true;
        }

        public virtual void StopMove()
        {
            rigidBody.velocity = Vector2.zero;
            animator.SetBool(animBoolIdle, true);
            animator.SetBool(animBoolMove, false);
            isMoving = false;
        }

        #endregion //Public API

        #region Client Impl



        #endregion //Client Impl
    }

}