//-----------------------------------------------------------------------
// <copyright>
// This software is licensed as Microsoft Public License (Ms-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Mite
{
    /// <summary>
    /// Data context for mite api
    /// </summary>
    public class MiteDataContext : BaseDataContext
    {
        private MiteConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiteDataContext"/> class.
        /// </summary>
        /// <param name="miteConfiguration">The configuration of the data context.</param>
        public MiteDataContext(MiteConfiguration miteConfiguration)
        {
            _configuration = miteConfiguration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiteDataContext"/> class.
        /// </summary>
        /// <remarks>
        /// The configuration is read from the app.config or web.config
        /// </remarks>
        public MiteDataContext()
        {
            _configuration = MiteConfiguration.ReadFromConfig();
        }

        /// <summary>
        /// Gets a data mapper for a type.
        /// </summary>
        /// <typeparam name="T">The type for which a data mapper should be retrieved</typeparam>
        /// <returns>A data mapper for the type</returns>
        protected override IDataMapper<T> GetDataMapper<T>()
        {
            return _configuration.Factory.GetMapper<T>();
        }
    }
}