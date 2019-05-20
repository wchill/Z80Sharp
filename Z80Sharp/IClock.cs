namespace Z80Sharp
{
    public interface IClock : IClockedComponent
    {
        long Ticks { get; }
        void AttachClockableDevice(IClockedComponent device);
        void DetachClockableDevice(IClockedComponent device);
        void TickMultiple(int ticks);
    }
}