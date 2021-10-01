namespace CanardEcarlate.Domain.Cards
{
    public interface ICard
    {
        string Name { get; }

        void DrawAction();
    }
}
