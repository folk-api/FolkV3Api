using System;
using System.Collections.Generic;

namespace Us.FolkV3.Api.Client
{
    using Mapper;
    using Model;
    using Model.Param;

    public class PrivateCommunityClient : PrivilegesSmallClient
    {
        private CommunityPersonMapper CommunityMapper { get; }
        private PrivateChangesMapper ChangesMapper { get; }

        public PrivateCommunityClient(HeldinConfig config)
            : base(config)
        {
            CommunityMapper = new CommunityPersonMapper();
            ChangesMapper = new PrivateChangesMapper();
            CheckCanUseCommunityMethods();
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            base.ListOfOperationClasses(operationClasses).AddRange(
                new List<Type>()
                {
                    typeof(Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndAddress),
                    typeof(Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndDateOfBirth),
                    typeof(Eu.Xroad.UsFolkV3.Producer.RemovePersonsFromCommunity),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPrivateChanges),
                });
            return operationClasses;
        }

        public CommunityPerson AddPersonToCommunity(NameParam name, AddressParam address)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(address, "address");
            var method = new Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndAddress()
            {
                request = Mapper.Mapper.NameAndAddressParam(name, address)
            };
            var request = new Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndAddressRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                AddPersonToCommunityByNameAndAddress = method
            };
            return Call(
                () => webService.AddPersonToCommunityByNameAndAddress(request),
                (r) => CommunityMapper.Map(r.AddPersonToCommunityByNameAndAddressResponse)
                );
        }

        public CommunityPerson AddPersonToCommunity(NameParam name, DateTime dateOfBirth)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(dateOfBirth, "dateOfBirth");
            var method = new Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndDateOfBirth()
            {
                request = Mapper.Mapper.NameAndDateOfBirthParam(name, dateOfBirth)
            };
            var request = new Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndDateOfBirthRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                AddPersonToCommunityByNameAndDateOfBirth = method
            };
            return Call(
                () => webService.AddPersonToCommunityByNameAndDateOfBirth(request),
                r => CommunityMapper.Map(r.AddPersonToCommunityByNameAndDateOfBirthResponse)
                );
        }

        public PrivateId RemovePersonFromCommunity(PrivateId id)
        {
            var removedIds = RemovePersonsFromCommunity(new List<PrivateId> { id });
            return removedIds.Count == 0 ? null : removedIds[0];
        }

        public IList<PrivateId> RemovePersonsFromCommunity(IList<PrivateId> ids)
        {
            Util.RequireNonNull(ids, "ids");
            var method = new Eu.Xroad.UsFolkV3.Producer.RemovePersonsFromCommunity();
            var request = new Eu.Xroad.UsFolkV3.Producer.RemovePersonsFromCommunityRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                RemovePersonsFromCommunity = method
            };
            return Call(
                () => webService.RemovePersonsFromCommunity(request),
                r => new ResponseWrapper<IList<PrivateId>>(
                    r.RemovePersonsFromCommunityResponse,
                    Mapper.Mapper.PrivateIds(r.RemovePersonsFromCommunityResponse.result)
                    )
                );
        }

        public Changes<PrivateId> GetChanges(DateTime from)
        {
            return GetChanges(from, DateTime.Now);
        }

        public Changes<PrivateId> GetChanges(DateTime from, DateTime to)
        {
            Util.RequireNonNull(from, "from");
            Util.RequireNonNull(to, "to");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPrivateChanges()
            {
                request = Mapper.Mapper.ChangesParam(from, to)
            };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPrivateChangesRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPrivateChanges = method
            };
            return Call(
                () => webService.GetPrivateChanges(request),
                r => new ResponseWrapper<Changes<PrivateId>>(
                    r.GetPrivateChangesResponse,
                    ChangesMapper.Map(r.GetPrivateChangesResponse.result)
                    )
                );
        }

    }
}
