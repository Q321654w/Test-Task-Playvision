public interface IAnimation<TTarget, TKey>
{
    TKey FirstKey { get; }
    TKey LastKey { get; }
    TKey this[int index] { get; }

    bool IsContinue(float elapsedTime);
    void ApplyLastFrame(TTarget target);
    void ApplyTo(TTarget target, float elapsedTime);
    void ApplyFirstFrame(TTarget target);
}