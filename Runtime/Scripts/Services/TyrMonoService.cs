using System;
using UnityEngine;

namespace TyrDK
{
    public abstract class TyrMonoService<T>: MonoBehaviour where T : TyrMonoService<T>
    {
        public static T Instance { get; private set; }
        protected abstract bool IsGlobal { get; }
        public static event Action OnServiceInitialized;

        protected virtual void Initialize()
        {
            
        }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                if (IsGlobal)
                    DontDestroyOnLoad(gameObject);
                Initialize();
                OnServiceInitialized?.Invoke();
                OnServiceInitialized = null;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}