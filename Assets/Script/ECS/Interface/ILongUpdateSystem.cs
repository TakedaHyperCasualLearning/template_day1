namespace Donuts
{
    public interface IPostUpdateSystem
    {
        void StarPostUpdate();
        void OnPostUpdate(float deltaTime);
        bool IsFinishedPostUpdate();
    }

    public interface IPreUpdateSystem 
    {
        void StartPreUpdate();
        void OnPreUpdate(float deltaTime);
        bool IsFinishedPreUpdate();
    }
}