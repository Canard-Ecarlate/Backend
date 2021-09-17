namespace CanardEcarlate.Domain
{
    public interface ICard
    {
        string Name { get; }

        void DrawAction();
    }
}
