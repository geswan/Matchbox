
namespace  MatchBoxIoc
{
 

    using Prism.Modularity;

    public class MatchBoxModule :IModule
 {
 public void OnLoad(IContainer container)
        {
           
            this.ioc = container;
            this.Initialize();
        }
		   public virtual void Initialize() {}
		   
		     public IContainer ioc { get; private set; }

      public void SetContainer(IContainer container)
      {
          this.ioc = container;
      }
			 
 }
}
