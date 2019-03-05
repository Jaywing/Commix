using System;

namespace Commix.Schema
{
    public class SitecoreHelpers<TModel, TProp>
    {
        public SchemaPropertyBuilder<TModel, TProp> SchemaBuilder { get; }

        public SitecoreHelpers(SchemaPropertyBuilder<TModel, TProp> schemaBuilder)
        {
            SchemaBuilder = schemaBuilder ?? throw new ArgumentNullException(nameof(schemaBuilder));
        }
    }

    public class SitecoreHelpers<TModel>
    {
        public SchemaModelBuilder<TModel> SchemaBuilder { get; }

        public SitecoreHelpers(SchemaModelBuilder<TModel> schemaBuilder)
        {
            SchemaBuilder = schemaBuilder ?? throw new ArgumentNullException(nameof(schemaBuilder));
        }
    }
}