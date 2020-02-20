using System;
using System.Collections.Generic;

namespace Us.FolkV3.Api.Client
{
    using Mapper;
    using Model;

    public class PublicCommunityClient : BaseClient
    {
        private PublicChangesMapper ChangesMapper { get; }

        public PublicCommunityClient(string consumerHost, string consumerName)
            : base(consumerHost, consumerName)
        {
            ChangesMapper = new PublicChangesMapper();
            CheckCanGetPublicChanges();
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            operationClasses.Add(typeof(Eu.Xroad.UsFolkV3.Producer.GetPublicChanges));
            return operationClasses;
        }

        public override ISet<string> GetRequiredPrivileges()
        {
            return new HashSet<string>();
        }

        public Changes<PublicId> GetChanges(DateTime from)
        {
            return GetChanges(from, DateTime.Now);
        }

        public Changes<PublicId> GetChanges(DateTime from, DateTime to)
        {
            Util.RequireNonNull(from, "from");
            Util.RequireNonNull(to, "to");
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPublicChanges() { request = Mapper.Mapper.ChangesParam(from, to) };
            return CheckStatus(
                Call(() =>
                    WebService.GetPublicChanges(
                        new Eu.Xroad.UsFolkV3.Producer.GetPublicChangesRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPublicChanges = request
                        }),
                        r => r.GetPublicChangesResponse.response
                    ),
                    r => ChangesMapper.Map(r.result)
                );
        }

    }
}
