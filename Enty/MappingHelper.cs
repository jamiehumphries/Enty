namespace Enty
{
    using AutoMapper;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    internal class MappingHelper
    {
        internal static void CreateMapsForContextTypes(DbContext context)
        {
            var metadataWorkspace = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
            var objectItemCollection = (ObjectItemCollection)metadataWorkspace.GetItemCollection(DataSpace.OSpace);
            var clrTypes = objectItemCollection.GetItems<EntityType>().Select(objectItemCollection.GetClrType);
            foreach (var clrType in clrTypes)
            {
                Mapper.CreateMap(clrType, clrType);
            }
        }
    }
}
