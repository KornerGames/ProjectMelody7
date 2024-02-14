using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zac
{

    public class DashAction : BaseCharacterAction
    {

        #region Inspector Fields

        [SerializeField]
        [Required]
        private Rigidbody2D rigidBody2D;

        [SerializeField]
        [Range(0f, 10f)]
        private float dashForce = 3f;

        [SerializeField]
        private float dashInterval = 0.02f;

        [SerializeField]
        private int dashBurstCount = 3;

        #endregion //Inspector Fields

        private const float dashMultiplier = 1000f;
        private Vector2 direction;

        #region Unity Callbacks

        private void Start()
        {
            RegisterOnFinish(OnFinish);
        }

        #endregion //Unity Callbacks

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        #region Client Impl

        protected override bool CanContinueAsLongAs() => false;

        protected override bool CanDoAction() => !isDoingAction;

        protected override void DoActionLogic()
        {
            //rigidBody2D.velocity = Vector2.zero;
            //rigidBody2D.AddForce(direction * dashForce);

            //rigidBody2D.velocity += direction * dashForce;

            StartCoroutine(C_Dash());
        }

        private void OnFinish()
        {
            //rigidBody2D.velocity = Vector2.zero;
            isDoingAction = false;
        }

        private IEnumerator C_Dash()
        {
            var waitTime = new WaitForSeconds(dashInterval);
            var count = 0;
            isDoingAction = true;
            var force = direction * dashForce * dashMultiplier;

            while (count < dashBurstCount)
            {
                rigidBody2D.AddForce(force);
                yield return waitTime;
                count++;
            }

            isDoingAction = true;
        }

        #endregion //Client Impl

    }

}