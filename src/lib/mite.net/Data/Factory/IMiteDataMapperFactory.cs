namespace Mite.Data.Factory
{
    /// <summary>
    /// Factory that creates Mapper for different types.
    /// </summary>
    public interface IMiteDataMapperFactory
    {
        /// <summary>
        /// Gets mapper for given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDataMapper<T> GetMapper<T>() where T : class, new();
    }
}