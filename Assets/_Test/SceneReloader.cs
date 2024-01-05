using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{

    public void Reload()
    {
        Debug.Log($"Reloading....");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
