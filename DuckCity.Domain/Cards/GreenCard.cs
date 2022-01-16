namespace DuckCity.Domain.Cards
{
    public class GreenCard : ICard
    {
        public string Name => "Green";

        public void DrawAction()
        {
            // Increment Green cards discovered
        }
    }
}
