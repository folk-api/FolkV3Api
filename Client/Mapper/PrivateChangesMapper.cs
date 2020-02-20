using System;

namespace Us.FolkV3.Api.Client.Mapper
{
    using Model;

    internal class PrivateChangesMapper : ChangesMapper<Eu.Xroad.UsFolkV3.Producer.PrivateChanges, PrivateId>
    {
        protected override Changes<PrivateId> DoMap(Eu.Xroad.UsFolkV3.Producer.PrivateChanges value)
        {
            return new Changes<PrivateId>(
                DateTime.Parse(value.from),
                DateTime.Parse(value.to),
                Mapper.PrivateIds(value.ids)
            );
        }
    }
}
