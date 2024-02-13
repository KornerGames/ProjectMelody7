using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zac
{

    public class ObjectLifetime : MonoBehaviour
    {

        [SerializeField]
        [Range(0f, 15f)]
        private float lifetime = 0.5f;

        [SerializeField]
        private bool isDestroyed;

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(C_Countdown());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator C_Countdown()
        {
            yield return new WaitForSeconds(lifetime);
            
            if (isDestroyed)
            {
                Destroy(gameObject);
                //return;
            }

            gameObject.SetActive(false);
        }

    }

}