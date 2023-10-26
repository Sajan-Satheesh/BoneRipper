using UnityEditor;
using UnityEngine;

public class MainMenuEvents : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    // Start is called before the first frame update
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    public void openRequired(GameObject req)
    {
        gameObject.SetActive(false);
        req.SetActive(true);
    }
}
