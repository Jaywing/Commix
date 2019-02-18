namespace Commix.ConsoleTest.Tools
{
    public class CommixViewModel<T>
    {
        public T View { get; }

        public CommixViewModel(T view)
        {
            View = view;
        }
    }
}