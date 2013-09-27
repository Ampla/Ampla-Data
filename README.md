Ampla-Data-MVC
===

Ampla Webservice Data Access and classes that supports a model based repository pattern

Key Concepts
===
* Repository pattern for data access 
* Attribute based declaration
* Automatic binding of Ampla data to C# classes
* Automatic mapping of Local date time to/from UTC 
* Support for ASP.NET MVC Controllers

Repository Pattern 
===
Provides ReadOnly access to Ampla data.
``` C#
public interface IReadOnlyRepository<TModel>
{
	IList<TModel> GetAll();
	
	TModel FindById(int id);
	
	IList<TModel> FindByFilter(params FilterValue[] filters);
}
```

IRepository Extends IReadOnlyRepository to provide write access

``` C#
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
* ```[AmplaDefaultFields("Confirmed=True")]

Field attributes:
* ```[AmplaField(Field = "Sample Period")]```

These attributes are used to mark model classes for use in the Repository.
Example:
``` C#
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

Automatic Binding of Ampla data to/from Model classes
===
The ```AmplaRepository<TModel>``` class will automatically map the properties to and from the model class. 

Features include:
* Automatic property binding
* Automatic defaults for required fields such as ```Sample Period```
* Id property is updated when a new record is added.
* DateTime properties are automatically presented as Local time in the model.

Example:
``` C#
// set up Repository
IRepositorySet repositorySet = new AmplaRepositorySet("User", "password");
IRepository<ExampleModel> repository = repositorySet.GetRepository<ExampleModel>();

// create the model
ExampleModel model = new ExampleModel {Shift = "Day", Tonnes = 95.3};

// insert the model will create the model in Ampla
repository.Add(model);

// model's Id property will be updated.
int insertedId = model.Id;

// retrieve the current model
ExampleModel updatedModel = repository.FindById(insertedId);

// All properties are automatically bound to the Ampla values.
```

ASP.NET MVC Controllers
===
Provides ```RepositoryController<TModel>``` to provide actions for models.
Example: 
``` C#
using AmplaWeb.Data;
using AmplaWeb.Data.Controllers;
using AmplaWeb.Sample.Models;

namespace AmplaWeb.Sample.Controllers
{
    public class ProductionController : RepositoryController<ProductionModel>
    {
        public ProductionController(IRepositorySet repositorySet) : base(repositorySet)
        {
        }
    }
}
```

The ```IRepositoryController<TModel>``` provides the following actions for models.

* GET  /{Model}
* GET  /{Model}/Index
* GET  /{Model}/Details/{Id}
* GET  /{Model}/Create
* POST /{Model}/Create
* GET  /{Model}/Edit/{Id}
* POST /{Model}/Edit
* GET  /{Model}/Delete/{Id}
* POST /{Model}/Delete/{Id}

``` C#
using System.Web.Mvc;

namespace AmplaWeb.Data.Controllers
{
    /// <summary>
    ///     Interface for Repository operations for the model 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IRepositoryController<in TModel>
    {
        /// <summary>
        ///     GET /{Model}/
        /// </summary>
        /// <returns></returns>
        ActionResult Index();

        /// <summary>
        ///     GET /{Model}/Details/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionResult Details(int id = 0);

        /// <summary>
        ///     GET /{Model}/Create
        /// </summary>
        /// <returns></returns>
        ActionResult Create();

        /// <summary>
        ///     POST /{Model}/Create
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        ActionResult Create(TModel model);

        /// <summary>
        ///     Get /{Model}/Edit/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionResult Edit(int id = 0);

        /// <summary>
        ///     POST /{Model}/Edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        ActionResult Edit(TModel model);

        /// <summary>
        ///     GET /{Model}/Delete/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ActionResult Delete(int id = 0);

        /// <summary>
        ///     POST /{Model}/Delete/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        ActionResult DeleteConfirmed(int id);
    }
}
```