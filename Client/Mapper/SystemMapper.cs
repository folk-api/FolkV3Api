using System.Collections.Generic;

namespace Us.FolkV3.Api.Client.Mapper
{
    using Api.Model;

    internal class SystemMapper
    {
        public IList<PrivateId> PrivateIds(Eu.Xroad.UsFolkV3.Producer.PrivateId[] idList)
        {
            return Mapper.PrivateIds(idList);
        }

    }
}
