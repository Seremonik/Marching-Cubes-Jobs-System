using UnityEngine;
using System.Collections;

/// <summary>
/// Class resposible for managing all UI
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private UISeed seed;
    [SerializeField]
    private WorldManager worldManager;
    
    void Awake()
    {
        Instance = this;
    }
    
    /// <summary>
    ///  Invoked by Generate Button from scene
    /// </summary>
    public void OnGenerateClick()
    {
        worldManager.Generate();
    }
    public int GetSeed()
    {
        return seed.GetSeed();
    }
}
