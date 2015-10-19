using System;
using System.Collections.Generic;
using AmplaData.AmplaData2008;
using AmplaData.Binding.ModelData;

namespace AmplaData.AmplaRepository
{
    /// <summary>
    ///     Ampla Repository that allows the manipulation of Ampla models
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class AmplaCalendarRepository<TModel> : ICalendarRepository<TModel> where TModel : class, new()
    {

        private IDataWebServiceCalendarClient calendarClient;
        private readonly ICredentialsProvider credentialsProvider;
        private readonly IModelProperties<TModel> modelProperties;

        public AmplaCalendarRepository(IDataWebServiceCalendarClient calendarClient, ICredentialsProvider credentialsProvider)
        {
            this.calendarClient = calendarClient;
            this.credentialsProvider = credentialsProvider;
            modelProperties = new ModelProperties<TModel>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            calendarClient = null;
        }

        /// <summary>
        /// Gets the model properties.
        /// </summary>
        /// <value>
        /// The model properties.
        /// </value>
        protected IModelProperties<TModel> ModelProperties
        {
            get { return modelProperties; }
        }

        /// <summary>
        /// Gets the web service client.
        /// </summary>
        /// <value>
        /// The web service client.
        /// </value>
        protected IDataWebServiceCalendarClient WebServiceClient
        {
            get { return calendarClient; }
        }

 

        /// <summary>
        /// Creates the credentials.
        /// </summary>
        /// <returns></returns>
        protected Credentials CreateCredentials()
        {
            return credentialsProvider.GetCredentials();
        }

        public IList<TModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public TModel FindForDateTime(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public IList<TModel> FindByFilter(params FilterValue[] filters)
        {
            throw new NotImplementedException();
        }

        public void Add(TModel model)
        {
            throw new NotImplementedException();
        }

        public void Delete(TModel model)
        {
            throw new NotImplementedException();
        }

        public void Update(TModel model)
        {
            throw new NotImplementedException();
        }
    }
}