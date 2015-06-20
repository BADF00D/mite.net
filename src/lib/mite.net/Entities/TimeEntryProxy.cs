//-----------------------------------------------------------------------
// <copyright>
// This software is licensed as Microsoft Public License (Ms-PL).
// </copyright>
//-----------------------------------------------------------------------

namespace Mite
{
    /// <summary>
    /// Proxy for the time entry class.
    /// </summary>
    /// <remarks>
    /// Used for lazy loading of referenced entities
    /// </remarks>
    public class TimeEntryProxy : TimeEntry
    {
        private readonly MiteDataMapperFactory _factory;
        private bool _userLoaded;
        private bool _serviceLoaded;
        private bool _projectLoaded;

        public TimeEntryProxy(MiteDataMapperFactory factory)
        {
            _factory = factory;
        }

        internal string ProjectId
        {
            get;
            set;
        }

        internal string UserId
        {
            get;
            set;
        }


        internal string ServiceId
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the service for this time entry.
        /// </summary>
        /// <value>The service.</value>
        public override Service Service
        {
            get
            {
                if ( !_serviceLoaded && !string.IsNullOrEmpty(ServiceId) )
                {
                    IDataMapper<Service> mapper = _factory.GetMapper<Service>();
                    base.Service = mapper.GetById(ServiceId);

                    _serviceLoaded = true;
                }

                return base.Service;
            }
            set
            {
                base.Service = value;
            }
        }

        /// <summary>
        /// Gets or sets the project for this time entry.
        /// </summary>
        /// <value>The project.</value>
        public override Project Project
        {
            get
            {
                if ( !_projectLoaded )
                {
                    if (string.IsNullOrEmpty(ProjectId))
                    {
                        base.Project = null;
                    }
                    else
                    {
                        IDataMapper<Project> mapper = _factory.GetMapper<Project>();
                        base.Project = mapper.GetById(ProjectId);
                    }

                    _projectLoaded = true;
                }

                return base.Project;
            }
            set
            {
                base.Project = value;
            }
        }

        /// <summary>
        /// Gets or sets the user for this time entry.
        /// </summary>
        /// <value>The user.</value>
        public override User User
        {
            get
            {
                if ( !_userLoaded )
                {
                    IDataMapper<User> mapper = _factory.GetMapper<User>();
                    base.User = mapper.GetById(UserId);

                    _userLoaded = true;
                }

                return base.User;
            }
            set
            {
                base.User = value;
            }
        }
    }
}