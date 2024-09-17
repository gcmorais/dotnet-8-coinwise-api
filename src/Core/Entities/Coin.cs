using Domain.Entities;

public class Coin : BaseEntity
{
    // Propriedades
    public string Name { get; private set; }
    public string Abbreviation { get; private set; }
    public decimal Price { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    // Construtor padrão necessário para o Entity Framework
    private Coin() { }

    // Construtor para inicializar propriedades
    public Coin(string name, string abbreviation, decimal price, User user)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(abbreviation))
            throw new ArgumentException("Abbreviation is required.", nameof(abbreviation));

        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.", nameof(price));

        if (user == null)
            throw new ArgumentNullException(nameof(user));

        Name = name;
        Abbreviation = abbreviation;
        Price = price;
        AssignUser(user);
    }

    // Métodos para atualizar propriedades
    public void AssignUser(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        User = user;
        UserId = user.Id;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be greater than zero.", nameof(newPrice));

        Price = newPrice;
        UpdateDate();
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty.", nameof(newName));

        Name = newName;
        UpdateDate();
    }

    public void UpdateAbbreviation(string newAbbreviation)
    {
        if (string.IsNullOrWhiteSpace(newAbbreviation))
            throw new ArgumentException("Abbreviation cannot be empty.", nameof(newAbbreviation));

        Abbreviation = newAbbreviation;
        UpdateDate();
    }
}
