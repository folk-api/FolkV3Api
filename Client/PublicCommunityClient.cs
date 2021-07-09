using System;
using System.Collections.Generic;

namespace Us.FolkV3.Api.Client
{
    using Mapper;
    using Model;

    public class PublicCommunityClient : PrivilegesMediumClient
    {
        private PublicChangesMapper ChangesMapper { get; }

        public PublicCommunityClient(HeldinConfig config)
            : base(config)
        {
            ChangesMapper = new PublicChangesMapper();
            CheckCanGetPublicChanges();
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            base.ListOfOperationClasses(operationClasses).Add(typeof(Eu.Xroad.UsFolkV3.Producer.GetPublicChanges));
            return operationClasses;
        }

        public Changes<PublicId> GetChanges(DateTime from)
        {
            return GetChanges(from, DateTime.Now);
        }

        public Changes<PublicId> GetChanges(DateTime from, DateTime to)
        {
            Util.RequireNonNull(from, "from");
            Util.RequireNonNull(to, "to");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPublicChanges() {
                request = Mapper.Mapper.ChangesParam(from, to)
            };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPublicChangesRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPublicChanges = method
            };
            return Call(
                () => webService.GetPublicChanges(request),
                r => new ResponseWrapper<Changes<PublicId>>(
                    r.GetPublicChangesResponse,
                    ChangesMapper.Map(r.GetPublicChangesResponse.result)
                    )
                );
        }

    }
}
