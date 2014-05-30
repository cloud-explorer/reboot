namespace Projects.Website
{
    public class Global : Sitecore.ContentSearch.SolrProvider.CastleWindsorIntegration.WindsorApplication
    {
        public override void Application_Start()
        {
            base.Application_Start();
            
        }
    }
}