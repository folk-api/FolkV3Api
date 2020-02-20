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
        private static long IdSequence = Util.CurrentTimeMillis();
        private static readonly string CAN_USE_COMMUNITY_METHODS = "CanUseCommunityMethods"; 

        private IDictionary<Type, string> ServiceHeaders { get; }
        protected string ProducerHeader { get; }
        protected string ConsumerHeader { get; }
        protected string UserIdHeader { get; }
        protected UsFolkPortType WebService { get; }
        protected bool CanUseCommunityMethods { get; }

        public BaseClient(string consumerHost, string consumerName)
        {
            ServiceHeaders = InitServiceHeaders();
            ProducerHeader = Const.UsFolkV3;
            ConsumerHeader = Util.RequireNonNull(consumerName, "consumerName");
            UserIdHeader = "-";
            WebService = FolkClient.WebService(consumerHost, false);
            var myPrivileges = GetMyPrivileges();
            CanUseCommunityMethods = myPrivileges.Contains(CAN_USE_COMMUNITY_METHODS);
            CheckPrivileges(myPrivileges);
        }

        private IDictionary<Type, string> InitServiceHeaders()
        {
            var serviceHeaders = new Dictionary<Type, string>();
            AddServiceHeader(typeof(Eu.Xroad.UsFolkV3.Producer.GetSystemStatus), serviceHeaders);
            AddServiceHeader(typeof(Eu.Xroad.UsFolkV3.Producer.GetMyPrivileges), serviceHeaders);
            AddServiceHeader(typeof(Eu.Xroad.UsFolkV3.Producer.RefreshConsumer), serviceHeaders);
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
                    string.Join(", ", privileges), string.Join(", ", requiredPrivileges), string.Join(", ", missingPrivileges)
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
            if (useOrNot ? CanUseCommunityMethods : !CanUseCommunityMethods)
            {
                return;
            }
            throw new FolkApiException("Insufficient privileges - can not " + what);
        }

        public string RefreshConsumer()
        {
            var request = new Eu.Xroad.UsFolkV3.Producer.RefreshConsumer() { request = "" };
            return CheckStatus(
                Call(() =>
                    WebService.RefreshConsumer(
                        new Eu.Xroad.UsFolkV3.Producer.RefreshConsumerRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            RefreshConsumer = request
                        }),
                        r => r.RefreshConsumerResponse.response
                    ),
                    r => r.result
                );
        }

        public IDictionary<string, string> GetSystemStatus()
        {
            var request = new Eu.Xroad.UsFolkV3.Producer.GetSystemStatus() { request = "" };
            return CheckStatus(
                Call(() =>
                    WebService.GetSystemStatus(
                        new Eu.Xroad.UsFolkV3.Producer.GetSystemStatusRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetSystemStatus = request
                        }),
                        r => r.GetSystemStatusResponse.response
                    ),
                    r => r.result.ToDictionary(ss => ss.name, ss => ss.value)
                );
        }

        public ISet<string> GetMyPrivileges()
        {
            var request = new Eu.Xroad.UsFolkV3.Producer.GetMyPrivileges() { request = "" };
            return CheckStatus(
                Call(() =>
                    WebService.GetMyPrivileges(
                        new Eu.Xroad.UsFolkV3.Producer.GetMyPrivilegesRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetMyPrivileges = request
                        }),
                        (r) => r.GetMyPrivilegesResponse.response
                    ),
                    r => r.result.ToHashSet()
                );
        }

        public abstract ISet<string> GetRequiredPrivileges();

        protected O Call<I, O>(Func<I> webServiceMethod, Func<I, O> responseExtractor) where O : Eu.Xroad.UsFolkV3.Producer.ResponseBase
        {
            try
            {
                return responseExtractor.Invoke(webServiceMethod.Invoke());
            }
            catch (Exception e)
            {
                throw new FolkApiException("Error when calling web service method", e);
            }
        }

        protected O CheckStatus<I, O>(I response, Func<I, O> mapper) where I : Eu.Xroad.UsFolkV3.Producer.ResponseBase
        {
            if (response.status == null)
            {
                throw new FolkApiException($"Invalid response, no status - message: {response.message}");
            }
            var status = ExtractStatus(response);
            if (status == ResponseStatus.Ok || status == ResponseStatus.NotFound)
            {
                return mapper.Invoke(response);
            }
            if (status == ResponseStatus.MoreThanOne)
            {
                throw new MoreThanOneException();
            }
            throw new ResponseStatusException(response.message, status);
        }

        protected string ServiceHeader(Type operationClass)
        {
            if (!ServiceHeaders.TryGetValue(operationClass, out string header))
            {
                throw new InvalidOperationException($"Illegal operation class: {operationClass}");
            }
            return header;
        }

        internal static string IdHeader()
        {
            return Interlocked.Increment(ref IdSequence).ToString();
        }

        private static void AddServiceHeader(Type serviceClass, IDictionary<Type, string> serviceHeaders)
        {
            serviceHeaders.Add(
                serviceClass,
                $"{Const.UsFolkV3}.{serviceClass.Name}.{Const.V1}"
                );
        }

        private static ResponseStatus ExtractStatus(Eu.Xroad.UsFolkV3.Producer.ResponseBase response)
        {
            try
            {
                return EnumMapper.ResponseStatus(response.status);
            }
            catch (Exception)
            {
                throw new FolkApiException($"Invalid status: {response.status}");
            }
        }

    }

}
