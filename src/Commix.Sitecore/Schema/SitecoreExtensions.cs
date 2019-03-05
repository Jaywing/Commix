namespace Commix.Schema
{
    public static class SitecoreExtensions
    {
        public static SitecoreHelpers<TModel, TProp> Sitecore<TModel, TProp>(
            this SchemaPropertyBuilder<TModel, TProp> builder)
        {
            return new SitecoreHelpers<TModel, TProp>(builder);
        }

        public static SitecoreHelpers<TModel> Sitecore<TModel>(
            this SchemaModelBuilder<TModel> builder)
        {
            return new SitecoreHelpers<TModel>(builder);
        }
    }
}