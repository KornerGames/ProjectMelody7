using NaughtyAttributes;
using UnityEngine;

namespace Zac
{

    public class BaseCharacterAI : MonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [Required]
        protected BaseCharacterMovement movement;

        [SerializeField]
        [Required]
        protected BaseCharacterAction action;

        [SerializeField]
        [Required]
        protected BaseCharacterStats stats;

        #endregion //Inspector Fields

        #region Unity Callbacks



        #endregion //Unity Callbacks

        #region Public API



        #endregion //Public API

        #region Client Impl



        #endregion //Client Impl

    }

}