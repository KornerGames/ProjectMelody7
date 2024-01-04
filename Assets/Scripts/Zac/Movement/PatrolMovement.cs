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

        #endregion //Inspector Fields

        #region Other Fields

        private int latestPatrolIndex;

        #endregion //Other Fields

        #region Unity Callbacks

        private void Start()
        {
            HasPatrolPoints();
            DeactivatePatrolPoints();
        }

        #endregion //Unity Callbacks

        #region Public API

        [Button]
        public override void StartMove()
        {
            base.StartMove();

            StartCoroutine(C_CyclePatrolPoints());
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

        private IEnumerator C_CyclePatrolPoints()
        {
            isMoving = true;

            if (!HasPatrolPoints())
            {
                StopMove();
                yield break;
            }

            while (true)
            {
                Vector2 origin = rigidBody.position;
                Vector2 destination = patrolPoints[latestPatrolIndex].position;

                if (origin.Equals(destination))
                {
                    SetNextPatrolPointIndex();
                    destination = patrolPoints[latestPatrolIndex].position;
                }

                var time = 0f;
                var direction = (destination - origin).normalized;

                while (Vector2.Distance(rigidBody.position, destination) > 0.25f)
                {
                    //rigidBody.position = Vector2.Lerp(origin, destination, 
                    //    time / moveDuration);

                    rigidBody.MovePosition(rigidBody.position + direction 
                        * Time.deltaTime * moveDuration);

                    time += Time.deltaTime;
                    yield return null;
                }

                SetNextPatrolPointIndex();
            }
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