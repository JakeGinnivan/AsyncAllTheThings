using System;

namespace ClassLibrary1
{
    /// <summary>
    /// Progress`1 posts progress updates to the current sync context. This is no good for testing because the posts are asynchronous.
    /// 
    /// TestProgress synchronously reports progress
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TestProgress<T> : IProgress<T>
    {
        private readonly Action<T> action;

        public TestProgress(Action<T> action)
        {
            this.action = action;
        }

        public void Report(T value)
        {
            action(value);
        }
    }
}