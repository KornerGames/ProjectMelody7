using NaughtyAttributes;
using UnityEngine;

namespace Zac
{

    [System.Serializable]
    public class CharacterStatsModel
    {

        public int hitPoints;
        public float moveDuration;

        [MinMaxSlider(ValueConstants.HP_MIN, ValueConstants.HP_MAX)]
        public Vector2 targetHPValueRange;

        //TODO add more

        public void TransferValuesTo(CharacterStatsModel model)
        {
            model.hitPoints = hitPoints;
            model.moveDuration = moveDuration;

            model.targetHPValueRange = targetHPValueRange;
            //TODO add more
        }

    }

}
