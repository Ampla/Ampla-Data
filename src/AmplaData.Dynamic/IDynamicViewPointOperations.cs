namespace AmplaData.Dynamic
{
    public interface IDynamicViewPointOperations
    {
        dynamic Save(object model);
        dynamic Insert(object model);
        dynamic Update(object model);
    }
}