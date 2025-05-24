namespace TyrDK
{
    public abstract class TyrClassService<T> where T : TyrClassService<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.Initialize();
                }   
                return _instance;
            }
        }

        protected virtual void Initialize()
        {
        }
    }
}