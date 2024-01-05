using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zac
{

    public class PatrolMovement : BaseCharacterMovement
    {

        #region Inspector Fields

        [Space]

        [SerializeField]
        private Transform[] patrolPoints;

        [SerializeField]
        [Range(0f, 100f)]
        private float stoppingDistance = 5f;

        [SerializeField]
        private bool deactivatePatrolPoints;

        #endregion //Inspector Fields

        #region Other Fields

        private int latestPatrolIndex;

        private Vector2 origin;
        private Vector2 destination;
        private Vector2 direction;

        #endregion //Other Fields

        #region Unity Callbacks

        private void Start()
        {
            HasPatrolPoints();
            DeactivatePatrolPoints();
        }

        private void SetUpPatrolPoint()
        {
            origin = rigidBody.position;
            destination = patrolPoints[latestPatrolIndex].position;

            if (origin.Equals(destination))
            {
                SetNextPatrolPointIndex();
                destination = patrolPoints[latestPatrolIndex].position;
            }

            direction = (destination - origin).normalized;
        }

        private void FixedUpdate()
        {
            if (!isMoving)
            {
                return;
            }

            if (!HasPatrolPoints())
            {
                StopMove();
                return;
            }

            if (Vector2.Distance(rigidBody.position, destination) > stoppingDistance)
            {
                rigidBody.MovePosition(rigidBody.position + direction
                    * Time.fixedDeltaTime * moveDuration);
            }
            else
            {
                SetNextPatrolPointIndex();
                SetUpPatrolPoint();
            }
        }

        #endregion //Unity Callbacks

        #region Public API

        [Button]
        public override void StartMove()
        {
            SetUpPatrolPoint();
            base.StartMove();
        }

        public override void StopMove()
        {
            base.StopMove();

            StopAllCoroutines();
        }

        #endregion //Public API

        #region Client Impl

        private bool HasPatrolPoints()
        {
            if (patrolPoints.Length == 0)
            {
                Debug.LogError($"{gameObject.name}.{GetType().Name}: " +
                    $"No patrol points set!!!", gameObject);
                return false;
            }

            return true;
        }

        private void SetNextPatrolPointIndex()
        {
            latestPatrolIndex++;

            if (latestPatrolIndex >= patrolPoints.Length)
            {
                latestPatrolIndex = 0;
            }
        }

        private void DeactivatePatrolPoints()
        {
            if (!deactivatePatrolPoints)
            {
                return;
            }

            foreach (var point in patrolPoints)
            {
                if (point == null)
                {
                    continue;
                }

                point.gameObject.SetActive(false);
            }
        }

        #endregion //Client Impl

    }

}