namespace DuckCity.Domain.Cards
{
    class GreenCard : ICard
    {
        public string Name => "Green";

        public void DrawAction()
        {
            // Increment Green cards discovered
        }
    }
}
