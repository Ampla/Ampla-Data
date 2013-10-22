namespace AmplaWeb.Data.Records
{
    public class ModelVersion
    {
        public ModelVersion(bool isCurrent, object model)
        {
            IsCurrentVersion = isCurrent;
            Object = model;
        }

        public object Object { get; set; }
        public bool IsCurrentVersion { get; private set; }
    }

    public class ModelVersion<TModel> : ModelVersion
    {
        public ModelVersion(bool isCurrent, TModel model) : base(isCurrent, model)
        {
        }

        public TModel Model { 
            get { return (TModel) Object; }
        }
    }
}