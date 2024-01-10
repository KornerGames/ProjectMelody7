using System.Collections;
using TMPro;
using UnityEngine;

namespace Zac
{
    public class LetterByLetterTextAnimator : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI textToAnimate;

        [SerializeField]
        private float letterRevealDelay;

        public void AnimateText(string text)
        {
            StopAllCoroutines();
            StartCoroutine(C_AnimateText(text));
        }

        private IEnumerator C_AnimateText(string text)
        {

            var textChars = text.ToCharArray();
            var index = 0;
            textToAnimate.text = textChars[index].ToString();
            
            while (index < (textChars.Length - 1))
            {
                yield return new WaitForSeconds(letterRevealDelay * Time.deltaTime);
                index++;
                textToAnimate.text = textToAnimate.text + textChars[index].ToString();
            }
        }

    }

}