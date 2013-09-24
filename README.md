Ampla-Data-MVC
==============

Ampla Webservice Data Access and classes that supports a model based repository pattern

Key Concepts
============
* Repository pattern for data access 
* Attribute based declaration
* Automatic binding of Ampla data to C# classes
* Support for ASP.NET MVC Controllers

Repository Pattern 
===
Provides ReadOnly access to Ampla data.
```
public interface IReadOnlyRepository<TModel>
{
	IList<TModel> GetAll();
	
	TModel FindById(int id);
	
	IList<TModel> FindByFilter(params FilterValue[] filters);
}
```

IRepository Extends IReadOnlyRepository to provide write access

```
public interface IRepository<TModel> : IReadOnlyRepository<TModel>
{
	/// Read access from IReadOnlyRepository
	/// IList<TModel> GetAll();
	/// TModel FindById(int id);
	/// IList<TModel> FindByFilter(params FilterValue[] filters);
	
	void Add(TModel model);
	void Delete(TModel model);
	void Update(TModel model);
	void Confirm(TModel model);
	void Unconfirm(TModel model);
}
```

Attribute based Declaration
===
Attributes are used to declare models for use in the repository.
Class attributes:
* ```[AmplaLocation(Location="Enterprise.Site.Area.Point")]```
* ```[AmplaModule(Module="Production")]```

Field attributes:
* ```[AmplaField(Field = "Sample Period")]```

These attributes are used to mark model classes for use in the Repository.
Example:
```
namespace AmplaWeb.Models
{
	[AmplaLocation(Location = "Enterprise.Site.Area.Example")]
	[AmplaModule(Module = "Production")]
	public class ExampleModel
	{
		public int Id {get; set;}
		
		[AmplaField(Field = "Sample Period")]
		public DateTime Sample { get; set; }
		
		public string Shift { get; set; }
		
		public double Tonnes { get; set; }
	}
}
```
