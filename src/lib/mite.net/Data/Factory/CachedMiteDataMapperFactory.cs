namespace Mite.Data.Factory
{
    using System;
    using System.Collections.Generic;

    public class CachedMiteDataMapperFactory : IMiteDataMapperFactory
    {
        private readonly Dictionary<Type, object> _mappers;

        public CachedMiteDataMapperFactory(MiteConfiguration configuration, TimeSpan timeItemsShouldBeCached)
        {
            var adapter = new DefaultWebAdapter(configuration);

            _mappers = new Dictionary<Type, object>
            {
                {
                    typeof (Project), new CachedMapper<Project>(
                        new ProjectMapper
                        {
                            Converter = new ProjectConverter(this),
                            WebAdapter = adapter
                        }, i => i.Id, timeItemsShouldBeCached)
                },
                {
                    typeof (TimeEntry), new CachedMapper<TimeEntry>(
                        new TimeEntryMapper 
                        {
                            Converter = new TimeEntryConverter(this),
                            WebAdapter = adapter
                        }, i => i.Id, timeItemsShouldBeCached)
                },
                {
                    typeof (Customer), new CachedMapper<Customer>(
                        new CustomerMapper
                        {
                            Converter = new CustomerConverter(),
                            WebAdapter = adapter
                        }, i => i.Id, timeItemsShouldBeCached)
                },
                {
                    typeof (Service), new CachedMapper<Service>(
                        new ServiceMapper
                        {
                            Converter = new ServiceConverter(),
                            WebAdapter = adapter
                        }, i => i.Id, timeItemsShouldBeCached)
                },
                {
                    typeof (User), new CachedMapper<User>(
                        new UserMapper
                        {
                            Converter = new UserConverter(),
                            WebAdapter = adapter
                        }, i => i.Id, timeItemsShouldBeCached)
                },
                {
                    typeof (Timer), new CachedMapper<Timer>(
                        new TimerMapper
                        {
                            Converter = new TimerConverter(),
                            WebAdapter = adapter
                        }, i => i.Id, timeItemsShouldBeCached)
                },
            };
        }


        public IDataMapper<T> GetMapper<T>() where T : class, new()
        {
            var type = typeof (T);
            if(_mappers.ContainsKey(type))
            {
                return (IDataMapper<T>) _mappers[type];
            }
            throw new Exception("Unkown type of mapper.");
        }
    }
}