using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDeInicio : MonoBehaviour
{
    public string escenaJuego;

    public void IniciarJuego()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
