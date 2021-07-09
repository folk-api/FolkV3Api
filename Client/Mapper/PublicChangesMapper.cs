using System;

namespace Us.FolkV3.Api.Client.Mapper
{
    using Model;

    internal class PublicChangesMapper : ChangesMapper<Eu.Xroad.UsFolkV3.Producer.PublicChanges, PublicId>
    {
        protected override Changes<PublicId> DoMap(Eu.Xroad.UsFolkV3.Producer.PublicChanges value)
        {
            return new Changes<PublicId>(
                DateTime.Parse(value.from),
                DateTime.Parse(value.to),
                Mapper.PublicIds(value.ids)
            );
        }
    }
}
