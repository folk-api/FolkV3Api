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

        public PrivateCommunityClient(string consumerHost, string consumerName)
            : base(consumerHost, consumerName)
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
            var request = new Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndAddress()
            {
                request = Mapper.Mapper.NameAndAddressParam(name, address)
            };
            return CheckStatus(
                Call(() =>
                    WebService.AddPersonToCommunityByNameAndAddress(
                        new Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndAddressRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            AddPersonToCommunityByNameAndAddress = request
                        }),
                        r => r.AddPersonToCommunityByNameAndAddressResponse.response
                    ),
                    r => CommunityMapper.Map(r.result)
                );
        }

        public CommunityPerson AddPersonToCommunity(NameParam name, DateTime dateOfBirth)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(dateOfBirth, "dateOfBirth");
            var request = new Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndDateOfBirth()
            {
                request = Mapper.Mapper.NameAndDateOfBirthParam(name, dateOfBirth)
            };
            return CheckStatus(
                Call(() =>
                    WebService.AddPersonToCommunityByNameAndDateOfBirth(
                        new Eu.Xroad.UsFolkV3.Producer.AddPersonToCommunityByNameAndDateOfBirthRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            AddPersonToCommunityByNameAndDateOfBirth = request
                        }),
                        r => r.AddPersonToCommunityByNameAndDateOfBirthResponse.response
                    ),
                    r => CommunityMapper.Map(r.result)
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
            var request = new Eu.Xroad.UsFolkV3.Producer.RemovePersonsFromCommunity() { request = Mapper.Mapper.PrivateIdList(ids) };
            return CheckStatus(
                Call(() =>
                    WebService.RemovePersonsFromCommunity(
                        new Eu.Xroad.UsFolkV3.Producer.RemovePersonsFromCommunityRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            RemovePersonsFromCommunity = request
                        }),
                        r => r.RemovePersonsFromCommunityResponse.response
                    ),
                    r => Mapper.Mapper.PrivateIds(r.result)
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
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPrivateChanges() { request = Mapper.Mapper.ChangesParam(from, to) };
            return CheckStatus(
                Call(() =>
                    WebService.GetPrivateChanges(
                        new Eu.Xroad.UsFolkV3.Producer.GetPrivateChangesRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPrivateChanges = request
                        }),
                        r => r.GetPrivateChangesResponse.response
                    ),
                    r => ChangesMapper.Map(r.result)
                );
        }

    }
}
