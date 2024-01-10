using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zac
{

    public class PlayerCharacterAI : BaseCharacterAI
    {

        #region Inspector Fields

        [SerializeField]
        private Slider sliderHealth;

        [SerializeField]
        private Image sliderFill;

        [SerializeField]
        private Color colorGood;

        [SerializeField]
        private Color colorAverage;

        [SerializeField]
        private Color colorDanger;

        #endregion //Inspector Fields

        #region Other Fields

        #endregion //Other Fields

        #region Unity Callbacks

        protected override void Awake()
        {
            base.Awake();
            stats.RegisterOnHealthUpdate(OnHealthUpdate);
        }

        protected override void Start()
        {
            //base.Start();
        }

        protected override void OnEnable()
        {
            //base.OnEnable();
        }

        protected override void OnDisable()
        {
            //base.OnDisable();
        }

        #endregion //Unity Callbacks

        #region Public API



        #endregion //Public API

        #region Client Impl

        private void OnHealthUpdate()
        {
            var hp = stats.Stats.hitPoints;
            sliderHealth.value = hp;

            sliderFill.color = (hp <= (sliderHealth.maxValue / 3)) ? colorDanger
                : (hp <= ((sliderHealth.maxValue / 3) * 2)) ? colorAverage
                : colorGood;
        }

        #endregion //Client Impl

    }

}