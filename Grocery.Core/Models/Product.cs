using CommunityToolkit.Mvvm.ComponentModel;

namespace Grocery.Core.Models
{
    public partial class Product : Model
    {
        [ObservableProperty]
        private int stock;

        [ObservableProperty] 
        private decimal _price;
        public DateOnly ShelfLife { get; set; }

        // Bestaande ctor zonder prijs: ketent door met price = 0m
        public Product(int id, string name, int stock)
            : this(id, name, stock, default, 0m) { }

        // Bestaande ctor zonder prijs maar mét THT: ketent door met price = 0m
        public Product(int id, string name, int stock, DateOnly shelfLife)
            : this(id, name, stock, shelfLife, 0m) { }

        // Nieuwe volledige ctor inclusief prijs
        public Product(int id, string name, int stock, DateOnly shelfLife, decimal price)
            : base(id, name)
        {
            Stock = stock;
            ShelfLife = shelfLife;
            UpdatePrice(price); 
        }

        /// <summary>
        /// Valideert en stelt de actuele prijs in.
        /// Regels: prijs ≥ 0,00; max 2 decimalen. (Eenvoudig; kun je later aanscherpen.)
        /// </summary>
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0m)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "Prijs mag niet negatief zijn.");
            if (HasMoreThanTwoDecimals(newPrice))
                throw new ArgumentException("Prijs heeft maximaal 2 decimalen.", nameof(newPrice));

            // Rond expliciet naar 2 decimalen (bankers rounding vermijden)
            Price = decimal.Round(newPrice, 2, MidpointRounding.AwayFromZero);
        }

        private static bool HasMoreThanTwoDecimals(decimal value)
        {
            // Werkt zonder string-conversie; robuust tegen cultuurinstellingen.
            var scaled = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
            return scaled != value;
        }

        public override string? ToString() => $"{Name} - {Stock} op voorraad - €{Price:0.00}";
    }
}
