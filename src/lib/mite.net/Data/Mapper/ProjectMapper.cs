//-----------------------------------------------------------------------
// <copyright>
// This software is licensed as Microsoft Public License (Ms-PL).
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;

namespace Mite
{
    internal class ProjectMapper : WebMapper, IDataMapper<Project>
    {
        public IEntityConverter<Project> Converter { get; set; }

        public virtual Project Create(Project item)
        {
            string result = WebAdapter.SendPostRequest("projects.xml", Converter.Convert(item));

            return Converter.Convert(result);
        }

        public virtual Project Update(Project item)
        {
            string result = WebAdapter.SendPutRequest(string.Format(CultureInfo.InvariantCulture,"projects/{0}.xml", item.Id), Converter.Convert(item));

            if (string.IsNullOrEmpty(result.Trim()))
            {
                return item;
            }

            return Converter.Convert(result);
        }

        public virtual void Delete(Project item)
        {
            WebAdapter.SendDeleteRequest(string.Format(CultureInfo.InvariantCulture,"projects/{0}.xml", item.Id));
        }

        public virtual Project GetById(object id)
        {
            string result = WebAdapter.SendGetRequest(string.Format(CultureInfo.InvariantCulture,"projects/{0}.xml", id));

            return Converter.Convert(result);
        }

        public virtual IList<Project> GetAll()
        {
            string result = WebAdapter.SendGetRequest("projects.xml");

            return Converter.ConvertToList(result);
        }

        public IList<string> CriteriaProperties
        {
            get
            {
                return new List<string> { "name" };
            }
        }

        public virtual IList<Project> GetByCriteria(QueryExpression queryExpression)
        {
            var query = new QueryTranslator(queryExpression, this.CriteriaProperties).Translate();
            var result = WebAdapter.SendGetRequest(string.Format(CultureInfo.InvariantCulture, "projects.xml{0}", query));

            return Converter.ConvertToList(result);
        }     
    }
}