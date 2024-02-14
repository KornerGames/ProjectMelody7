using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Zac
{

    public class MissionObjectiveView : MonoBehaviour
    {

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private TextMeshProUGUI textMission;

        [SerializeField]
        private float lerpDuration = 1f;

        [SerializeField]
        private float displayDuration = 5f;

        [Button]
        private void TestShowMission()
        {
            ShowMission("this is a test mission");
        }

        public void ShowMission(string text)
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(C_ShowMission(text));
        }

        private IEnumerator C_ShowMission(string text)
        {
            textMission.text = text;

            var timeElapsed = 0f;
            var alpha = 0f;

            canvasGroup.alpha = 0f;

            while (timeElapsed < lerpDuration)
            {
                alpha = Mathf.Lerp(0f, 1f, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;

                canvasGroup.alpha = alpha;
            }

            canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(displayDuration);

            timeElapsed = 0f;
            alpha = 1f;

            canvasGroup.alpha = 1f;

            while (timeElapsed < lerpDuration)
            {
                alpha = Mathf.Lerp(1f, 0f, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;

                canvasGroup.alpha = alpha;
            }

            canvasGroup.alpha = 0f;
        }

    }

}