namespace Gamemaker.Patterns.Pool
{
    /// <summary>
    /// Interface for using the "Object Pool" pattern <see cref="Object_Pool"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICreation<T>
    {
        /// <summary>
        /// Returns a newly created object
        /// </summary>
        /// <returns></returns>
        T Create();
    }
}