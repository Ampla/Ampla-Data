using System;

namespace AmplaData.Data.Records
{
    public class ModelVersion
    {
        protected ModelVersion(bool isCurrent, object model)
        {
            IsCurrentVersion = isCurrent;
            Object = model;
        }

        public object Object { get; protected set; }
        public bool IsCurrentVersion { get; private set; }

        public string User { get; set; }

        public DateTime VersionDate { get; set; }

        public int Version { get; set; }

        public string Display { get; set; }
    }

    public class ModelVersion<TModel> : ModelVersion
    {
        public ModelVersion(bool isCurrent, TModel model)
            : base(isCurrent, model)
        {
        }

        public TModel Model
        {
            get { return (TModel) Object; }
            set { Object = value; }
        }

    }
}