using System;
using System.Collections.Generic;
using System.Linq;

namespace Us.FolkV3.Api.Client
{
    using Eu.Xroad.UsFolkV3.Producer;

    public abstract class PrivilegesSmallClient : BaseClient
    {
        protected PrivilegesSmallClient(HeldinConfig config)
            : base(config)
        {
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            operationClasses.Add(typeof(Eu.Xroad.UsFolkV3.Producer.GetPrivilegesSmall));
            return operationClasses;
        }

        public override ISet<string> GetRequiredPrivileges()
        {
            var method = new GetPrivilegesSmall();
            var request = new GetPrivilegesSmallRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPrivilegesSmall = method
            };
            return Call(
                () => webService.GetPrivilegesSmall(request),
                r => new ResponseWrapper<ISet<string>>(
                    r.GetPrivilegesSmallResponse,
                    r.GetPrivilegesSmallResponse.result.ToHashSet()
                    )
                );
        }

    }
}
