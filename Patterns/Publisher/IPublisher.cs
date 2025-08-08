using System;

namespace Gamemaker.Patterns.Publisher
{
    public interface IPublisher<T> where T : struct, IConvertible
    {
        void Subscribe(T channel, ReturnAdress observer);
        void Unsubscribe(T channel, ReturnAdress observer);
    }
}