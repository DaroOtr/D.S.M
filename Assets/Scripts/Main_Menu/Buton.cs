using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Class For The Menu Buttons
/// </summary>
public class Buton : MonoBehaviour
{
    private const string game_Scene_Name = "NewLevel_1";
    private const string main_Menu_Scene_Name = "Main_Menu";
    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private GameObject panelParent;

    /// <summary>
    /// Instantiate A Window Prefab On ClICK
    /// </summary>
    public void Onclick() 
    {
        panelParent.GetComponent<Buttons_Controller>().InstantiateWindow(windowPrefab);
    }

    /// <summary>
    /// Load The Gameplay Scene && Unloads The Menu Scene
    /// </summary>
    public void OnClickStart() 
    {
        SceneManager.LoadScene(game_Scene_Name);
    }

    /// <summary>
    /// Destroy The Prefab Of The Panel
    /// </summary>
    public void OnWindowClose() 
    {
        Destroy(windowPrefab);
    }
}
