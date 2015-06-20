//-----------------------------------------------------------------------
// <copyright>
// This software is licensed as Microsoft Public License (Ms-PL).
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace Mite
{
    public class MiteDataMapperFactory
    {
        private readonly MiteConfiguration _configuration;

        public MiteDataMapperFactory(MiteConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal IDataMapper<T> GetMapper<T>() where T : class, new()
        {
            WebMapper mapper = null;

            if ( typeof(T) == typeof(Customer) )
            {
                mapper = new CustomerMapper
                             {
                                 Converter = new CustomerConverter()
                             };
            }
            if ( typeof(T) == typeof(User) )
            {
                mapper = new UserMapper
                             {
                                 Converter = new UserConverter()
                             };
            }
            if ( typeof(T) == typeof(Project) )
            {
                mapper = new ProjectMapper
                             {
                                 Converter = new ProjectConverter(this)
                             };
            }
            if ( typeof(T) == typeof(Service) )
            {
                mapper = new ServiceMapper
                             {
                                 Converter = new ServiceConverter()
                             };
            }
            if ( typeof(T) == typeof(TimeEntry) )
            {
                mapper = new TimeEntryMapper
                             {
                                 Converter = new TimeEntryConverter(this)
                             };
            }
            if ( typeof(T) == typeof(Timer) )
            {
                mapper = new TimerMapper
                             {
                                 Converter = new TimerConverter()
                             };
            }

            if ( mapper != null )
            {
                mapper.WebAdapter = new DefaultWebAdapter(_configuration);

                return (IDataMapper<T>)mapper;
            }

            throw new NotImplementedException();
        }
    }
}