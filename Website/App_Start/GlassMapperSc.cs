/*************************************

DO NOT CHANGE THIS FILE - UPDATE GlassMapperScCustom.cs

**************************************/

using Glass.Mapper.Sc.CastleWindsor;
using Projects.Website.App_Start;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(GlassMapperSc), "Start")]

namespace Projects.Website.App_Start
{
	public static class  GlassMapperSc
	{
		public static void Start()
		{
			//create the resolver
			var resolver = DependencyResolver.CreateStandardResolver();

			//install the custom services
			GlassMapperScCustom.CastleConfig(resolver.Container);

			//create a context
			var context = Glass.Mapper.Context.Create(resolver);
			context.Load(      
				GlassMapperScCustom.GlassLoaders()        				
				);

			GlassMapperScCustom.PostLoad();
		}
	}
}