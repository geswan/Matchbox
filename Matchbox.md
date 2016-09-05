# Matchbox#
MatchBox is a small IOC container. At its heart is a simple Dictionary that has a string and a Delegate as the key/value pair. The key is the Type name and the value is a delegate that encapsulates the getter for the type.
##Useage##
The Register method is the means by which the container is informed of the identity of an object and the getter method needed to create it. What makes this container more intuitive than most is that you define the getter method yourself by way of a lambda expression. So to register an instance of IMyClass as MyClass you could write.
`Ioc.Register<IMyClass>(()=>new MyClass());`
You could also use the object initialiser
`Ioc.Register<MyViewModel>(()=> new MyViewModel());`
`Ioc.Register<MyView>(()=new MyView{DataContext=ioc.Get<MyViewModel>());`
The compiler converts lambda expressions into delegates of the type Func<Tin,Treturned> and it’s this that’s stored in the dictionary.
###Registering Transients###
- `Ioc.Register<MyViewModel>(()=> new MyViewModel());`
- `Ioc.Register(()=> new MyViewModel());`
- `Ioc.Register<MyViewModel>();`
- `Ioc.Register<IMyClass,MyClass>();`
- `Ioc.Register<IMyClass>(()=> new MyClass());`
###Registering Singletons###
- `Ioc.RegisterSingleton<MyViewModel>(()=> new MyViewModel());`
- `Ioc.RegisterSingleton(()=> new MyViewModel());`
- `Ioc.RegisterSingleton<MyViewModel>();`
- `Ioc.RegisterSingleton<IMyClass,MyClass>();`
- `Ioc.RegisterSingleton<IMyClass>(()=> new MyClass());`

These methods use the `Lazy<T>` class. The registered Type’s constructor is only called when the  type is first used.  the same instance is returned from the container on each call.
###Retrieving Types From The Container
To retrieve a Type just call `ioc.Get<T>` like this
`Ioc.Get<IMyClass>();`
##Working with Modules.##
With the modular design pattern, separate concerns within an application are developed, each within their own project and class library. The responsibility for loading and initialising the modules at run time usually resides with the ioc container. Each module has a class that implements the interface IModule, this interface defines an Initialize method and a property of type IContainer. The container locates the module in the executing assembly by using the settings detailed in the app.config file. It then finds the IModule Type, sets the type’s Icontainer property to itself and calls the Initialize method.The example app shows how this can be done. The app also shows how to use the container within the Prism framework. The Prism framework is heavily dependent upon its ioc container. By the time the bootstrapper  has run there are over twenty five types registered in the container.

