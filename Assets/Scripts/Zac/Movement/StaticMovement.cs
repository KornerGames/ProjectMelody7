using NaughtyAttributes;
using UnityEngine;

namespace Zac
{

    public class StaticMovement : BaseCharacterMovement
    {

        #region Inspector Fields



        #endregion //Inspector Fields

        #region Other Fields

        #endregion //Other Fields

        #region Unity Callbacks

        private void Awake()
        {
            isMoving = false;
            moveDuration = 0;
        }

        #endregion //Unity Callbacks

        #region Public API

        public override void StartMove()
        {
            //rigidBody.velocity = Vector2.zero;
            base.StartMove();
        }

        #endregion //Public API

        #region Client Impl



        #endregion //Client Impl

    }

}
