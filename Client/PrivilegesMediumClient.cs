using System;
using System.Collections.Generic;
using System.Linq;

namespace Us.FolkV3.Api.Client
{
    using Eu.Xroad.UsFolkV3.Producer;
    public abstract class PrivilegesMediumClient : BaseClient
    {

        protected PrivilegesMediumClient(HeldinConfig config)
             : base(config)
        {
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            operationClasses.Add(typeof(Eu.Xroad.UsFolkV3.Producer.GetPrivilegesMedium));
            return operationClasses;
        }

        public override ISet<string> GetRequiredPrivileges()
        {
            var method = new GetPrivilegesMedium();
            var request = new GetPrivilegesMediumRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPrivilegesMedium = method
            };
            return Call(
                () => webService.GetPrivilegesMedium(request),
                r => new ResponseWrapper<ISet<string>>(
                    r.GetPrivilegesMediumResponse,
                    r.GetPrivilegesMediumResponse.result.ToHashSet()
                    )
                );
        }

    }
}
