using System;
using System.Collections.Generic;
using System.Linq;

namespace Us.FolkV3.Api.Client
{
    public abstract class PrivilegesSmallClient : BaseClient
    {
        protected PrivilegesSmallClient(string consumerHost, string consumerName)
            : base(consumerHost, consumerName)
        {
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            operationClasses.Add(typeof(Eu.Xroad.UsFolkV3.Producer.GetPrivilegesSmall));
            return operationClasses;
        }

        public override ISet<string> GetRequiredPrivileges()
        {
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPrivilegesSmall() { request = "" };
            return CheckStatus(
                Call(() =>
                    WebService.GetPrivilegesSmall(new Eu.Xroad.UsFolkV3.Producer.GetPrivilegesSmallRequest()
                    {
                        consumer = ConsumerHeader,
                        producer = ProducerHeader,
                        userId = UserIdHeader,
                        id = IdHeader(),
                        service = ServiceHeader(request.GetType()),
                        GetPrivilegesSmall = request
                    }),
                    r => r.GetPrivilegesSmallResponse.response
                ),
                r => r.result.ToHashSet()
            );
        }

    }
}
