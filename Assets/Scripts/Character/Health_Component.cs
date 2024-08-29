using System;
using UnityEngine;

/// <summary>
/// Class For The Management of The Health of The Characters
/// </summary>
public class Health_Component : MonoBehaviour
{
    [field : SerializeField] public float _health { get; set; }
    [field : SerializeField] public float _maxHealth { get; set; }

    public event Action OnDecrease_Health;
    public event Action OnInsufficient_Health;

    private void OnEnable()
    {
        _health = _maxHealth;
    }

    /// <summary>
    /// Decrease The Health Variable for the Characters
    /// </summary>
    /// <param name="harm_Value"></param>
    public void DecreaseHealth(float harm_Value)
    {
        _health -= harm_Value;
        OnDecrease_Health?.Invoke();
        CheckHealth();
    }

    /// <summary>
    /// Increase The Health Variable for the Characters
    /// </summary>
    public void IncreaseHealth(float heal_Value)
    {
        _health += heal_Value;

        if (_health > _maxHealth)
            _health = _maxHealth;

        CheckHealth();
    }

    /// <summary>
    /// Checks If The Character Health is Greater or Equals Than 0
    /// </summary>
    public void CheckHealth()
    {
        if (_health <= 0)
            OnInsufficient_Health?.Invoke();
    }

    private void OnDisable()
    {
        _health = 0;
    }
}
