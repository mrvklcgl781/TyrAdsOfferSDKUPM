using System.Collections;
using System.Collections.Generic;

namespace TyrDK
{
    public class TyrCoroutineService: TyrMonoService<TyrCoroutineService>
    {
        protected override bool IsGlobal => true;
        private static Queue<IEnumerator> _coroutineQue = new Queue<IEnumerator>();
        protected override void Initialize()
        {
            while(_coroutineQue.Count > 0)
            {
                StartCoroutine(_coroutineQue.Dequeue());
            }
        }

        public void Execute(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
        
        public void Terminate(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }
        
        public static void AddToQueue(IEnumerator coroutine)
        {
            _coroutineQue.Enqueue(coroutine);
        }
    }
}