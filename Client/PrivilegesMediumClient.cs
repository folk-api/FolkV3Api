using System;
using System.Collections.Generic;
using System.Linq;

namespace Us.FolkV3.Api.Client
{
    public abstract class PrivilegesMediumClient : BaseClient
    {
        protected PrivilegesMediumClient(string consumerHost, string consumerName)
             : base(consumerHost, consumerName)
        {
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            operationClasses.Add(typeof(Eu.Xroad.UsFolkV3.Producer.GetPrivilegesMedium));
            return operationClasses;
        }

        public override ISet<string> GetRequiredPrivileges()
        {
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPrivilegesMedium() { request = "" };
            return CheckStatus(
                Call(() =>
                    WebService.GetPrivilegesMedium(new Eu.Xroad.UsFolkV3.Producer.GetPrivilegesMediumRequest()
                    {
                        consumer = ConsumerHeader,
                        producer = ProducerHeader,
                        userId = UserIdHeader,
                        id = IdHeader(),
                        service = ServiceHeader(request.GetType()),
                        GetPrivilegesMedium = request
                    }),
                    r => r.GetPrivilegesMediumResponse.response
                ),
                r => r.result.ToHashSet()
            );
        }

    }
}
