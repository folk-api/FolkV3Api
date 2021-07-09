using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Us.FolkV3.Api.Client
{
    using Eu.Xroad.UsFolkV3.Producer;
    using Mapper;

    public abstract class BaseClient
    {
        private static long idSequence = Util.CurrentTimeMillis();
        private static readonly string CAN_USE_COMMUNITY_METHODS = "CanUseCommunityMethods";
        private static readonly string PROTOCOL_VERSION = "4.0";
    	private static readonly string SERVICE_VERSION = "v1";

        private readonly HeldinConfig config;
        private readonly IDictionary<Type, XRoadServiceIdentifierType> serviceHeaders;
        protected readonly XRoadClientIdentifierType clientHeader;
        protected readonly string protocolVersionHeader;
        protected readonly string userIdHeader;
        protected readonly string issueHeader;
        protected readonly UsFolkPortType webService;
        protected readonly bool canUseCommunityMethods;

        public BaseClient(HeldinConfig config)
        {
            this.config = config;
            serviceHeaders = InitServiceHeaders();
            clientHeader = InitClientHeader();
            protocolVersionHeader = PROTOCOL_VERSION;
            userIdHeader = config.UserId;
            issueHeader = null;
            webService = FolkClient.WebService(config.Host, config.Secure);
            var myPrivileges = GetMyPrivileges();
            canUseCommunityMethods = myPrivileges.Contains(CAN_USE_COMMUNITY_METHODS);
            CheckPrivileges(myPrivileges);
        }

        private IDictionary<Type, XRoadServiceIdentifierType> InitServiceHeaders()
        {
            var serviceHeaders = new Dictionary<Type, XRoadServiceIdentifierType>();
            AddServiceHeader(typeof(GetSystemStatus), serviceHeaders);
            AddServiceHeader(typeof(GetMyPrivileges), serviceHeaders);
            AddServiceHeader(typeof(RefreshConsumer), serviceHeaders);
            ListOfOperationClasses(new List<Type>()).ForEach(c => AddServiceHeader(c, serviceHeaders));
            return serviceHeaders;
        }

        protected abstract List<Type> ListOfOperationClasses(List<Type> operationClasses);

        private void CheckPrivileges(ISet<string> myPrivileges)
        {
            var privileges = myPrivileges
                .Where(p => !p.Equals(CAN_USE_COMMUNITY_METHODS))
                .ToList();
            var requiredPrivileges = GetRequiredPrivileges(); 
            if (!requiredPrivileges.IsSubsetOf(privileges))
            {
                var missingPrivileges = requiredPrivileges.Where(p => !privileges.Contains(p));
                throw new FolkApiException(string.Format("Insufficient privileges - actual: {0} - required: {1} - missing: {2}",
                    string.Join(", ", myPrivileges), string.Join(", ", requiredPrivileges), string.Join(", ", missingPrivileges)
                    ));
            }
        }

        protected void CheckCanUseCommunityMethods()
        {
            CheckCanOrNot(true, "use community methods");
        }

        protected void CheckCanUsePrivateId()
        {
            CheckCanOrNot(true, "use private id");
        }

        protected void CheckCanUsePublicId()
        {
            CheckCanOrNot(false, "use public id");
        }

        protected void CheckCanGetPrivateChanges()
        {
            CheckCanOrNot(true, "get private changes");
        }

        protected void CheckCanGetPublicChanges()
        {
            CheckCanOrNot(false, "get public changes");
        }

        private void CheckCanOrNot(bool useOrNot, String what)
        {
            if (useOrNot ? canUseCommunityMethods : !canUseCommunityMethods)
            {
                return;
            }
            throw new FolkApiException("Insufficient privileges - can not " + what);
        }

        public string RefreshConsumer()
        {
            var method = new RefreshConsumer();
            var request = new RefreshConsumerRequest() {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                RefreshConsumer = method
            };
            return Call(
                () => webService.RefreshConsumer(request),
                r => new ResponseWrapper<string>(
                    r.RefreshConsumerResponse,
                    r.RefreshConsumerResponse.result
                    )
                );
        }

        public IDictionary<string, string> GetSystemStatus()
        {
            var method = new GetSystemStatus();
            var request = new GetSystemStatusRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetSystemStatus = method
            };
            return Call(
                () => webService.GetSystemStatus(request),
                r => new ResponseWrapper<IDictionary<string, string>>(
                    r.GetSystemStatusResponse,
                    r.GetSystemStatusResponse.result.ToDictionary(ss => ss.name, ss => ss.value)
                    )
                );
        }

        public ISet<string> GetMyPrivileges()
        {
            var method = new GetMyPrivileges();
            var request = new GetMyPrivilegesRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetMyPrivileges = method
            };
            return Call(
                () => webService.GetMyPrivileges(request),
                r => new ResponseWrapper<ISet<string>>(
                    r.GetMyPrivilegesResponse,
                    r.GetMyPrivilegesResponse.result.ToHashSet()
                    )
                );
        }

        public abstract ISet<string> GetRequiredPrivileges();

        protected O Call<I, O>(Func<I> method, Func<I, ResponseWrapper<O>> responseExtractor)
        {
            try
            {
                var wrapper = responseExtractor.Invoke(method.Invoke());
                CheckStatus(wrapper.Status, wrapper.Message);
                return wrapper.Result;
            }
            catch (Exception e)
            {
                throw new FolkApiException("Error when calling web service method", e);
            }
        }

        protected void CheckStatus(String statusValue, String message)
        {
            if (statusValue == null)
            {
                throw new FolkApiException("Invalid response, no status - message: " + message);
            }
            var status = ExtractStatus(statusValue);
            if (status == ResponseStatus.Ok || status == ResponseStatus.NotFound)
            {
                return;
            }
            if (status == ResponseStatus.MoreThanOne)
            {
                throw new MoreThanOneException();
            }
            throw new ResponseStatusException(message, status);
        }

        protected XRoadServiceIdentifierType ServiceHeader(Type operationClass)
        {
            if (!serviceHeaders.TryGetValue(operationClass, out XRoadServiceIdentifierType header))
            {
                throw new InvalidOperationException($"Illegal operation class: {operationClass}");
            }
            return header;
        }

        private void AddServiceHeader(Type serviceClass, IDictionary<Type, XRoadServiceIdentifierType> serviceHeaders)
        {
            serviceHeaders.Add(serviceClass, InitServiceHeader(serviceClass));
        }

        private XRoadServiceIdentifierType InitServiceHeader(Type serviceClass)
        {
            var si = new XRoadServiceIdentifierType();
            si.objectType = XRoadObjectType.SERVICE;
            si.xRoadInstance = config.Service.XRoadInstance;
            si.memberClass = config.Service.MemberClass;
            si.memberCode = config.Service.MemberCode;
            si.subsystemCode = config.Service.SubSystemCode;
            si.serviceCode = serviceClass.Name;
            si.serviceVersion = SERVICE_VERSION;
            return si;
        }

        private XRoadClientIdentifierType InitClientHeader()
        {
            var ci = new XRoadClientIdentifierType();
            ci.objectType = XRoadObjectType.SUBSYSTEM;
            ci.xRoadInstance = config.Client.XRoadInstance;
            ci.memberClass = config.Client.MemberClass;
            ci.memberCode = config.Client.MemberCode;
            ci.subsystemCode = config.Client.SubSystemCode;
            return ci;
        }

        internal static string IdHeader()
        {
            return Interlocked.Increment(ref idSequence).ToString();
        }

        private static ResponseStatus ExtractStatus(string status)
        {
            try
            {
                return EnumMapper.ResponseStatus(status);
            }
            catch (Exception)
            {
                throw new FolkApiException($"Invalid status: {status}");
            }
        }

    }

}
