using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Class that manage the seed input
/// </summary>
public class UISeed : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;

    public Action OnChange;
    
    void Start()
    {
        OnRandomClick();
    }

    /// <summary>
    /// Invoked by Random Button from scene
    /// </summary>
    public void OnRandomClick()
    {
        string result = string.Empty;

        for(int i = 0; i < 10; i++)
        {
            result += (char)UnityEngine.Random.Range(48, 122);
        }

        inputField.text = result;
        OnSeedChange();
    }

    public void OnSeedChange()
    {
        if(OnChange != null)
        {
            OnChange();
        }
    }

    /// <summary>
    /// Funtion that returns seed from Inputfield
    /// </summary>
    /// <returns>seed</returns>
    public int GetSeed()
    {
        int result = 0;
        string inputText = inputField.text;

        for(int i = 0; i < inputText.Length; i++)
        {
            result += (inputText[i] - 48) * (int)(Mathf.Pow(100, i));
        }
        return result;
    }
}
