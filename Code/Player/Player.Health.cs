namespace SCE.Player;

public partial class Player {
    /// <summary>
    /// How much health the player has in total.
    /// </summary>
    [Property]
    [Category("Vitals")]
    public float MaxHealth { get; set; } = 100;

    /// <summary>
    /// How much health the player has on spawn.
    /// </summary>
    [Property]
    [Category("Vitals")]
    public float StartHealth { get; set; } = 100;

    [Property]
    [Category("Vitals")]
    public float CurrentHealth { get; private set; }

    [Property]
    [Category("Vitals")]
    public bool Alive => CurrentHealth <= 0;

    private void OnHealthStart()
    {
        CurrentHealth = StartHealth;
    }

    /// <summary>
    /// Damage the unit, clamped, heal if set to negative.
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(float damage)
    {
        if (!Alive) return;
        CurrentHealth -= damage;
    }

    /// <summary>
    /// Kill the unit instantly.
    /// </summary>
    public void Kill()
    {
        // To kill the unit, we just send double their current health as damage,
        // ensuring the `Alive` condition returns false.
        Damage(CurrentHealth * 2);
    }
}