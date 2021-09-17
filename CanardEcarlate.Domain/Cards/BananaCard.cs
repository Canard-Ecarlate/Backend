namespace CanardEcarlate.Domain
{
    class BananaCard : ICard
    {
        public string Name => "Banana";

        public void DrawAction()
        {
            // Skip the current player
        }
    }
}
