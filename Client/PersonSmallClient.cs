using System;
using System.Collections.Generic;

namespace Us.FolkV3.Api.Client
{
    using Model;
    using Model.Param;
    using Mapper;
 
    public class PersonSmallClient : PrivilegesSmallClient
    {
        private PersonSmallMapper PersonMapper { get; }

        public PersonSmallClient(HeldinConfig config)
            : base(config)
        {
            PersonMapper = new PersonSmallMapper();
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            base.ListOfOperationClasses(operationClasses).AddRange(
                new List<Type>()
                {
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPrivateId),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPtal),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndAddress),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndDateOfBirth),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPrivateChanges)
                });
            return operationClasses;
        }

        public PersonSmall GetPerson(PrivateId id)
        {
            Util.RequireNonNull(id, "id");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPrivateId() { request = Mapper.Mapper.PrivateId(id) };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPrivateIdRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonSmallByPrivateId = method
            };
            return Call(
                () => webService.GetPersonSmallByPrivateId(request),
                r => PersonMapper.Map(r.GetPersonSmallByPrivateIdResponse)
                );
        }

        public PersonSmall GetPerson(Ptal ptal)
        {
            Util.RequireNonNull(ptal, "ptal");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPtal() { request = ptal.Value };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPtalRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonSmallByPtal = method
            };
            return Call(
                () => webService.GetPersonSmallByPtal(request),
                r => PersonMapper.Map(r.GetPersonSmallByPtalResponse)
                );
        }

        public PersonSmall GetPerson(NameParam name, AddressParam address)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(address, "address");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndAddress()
            {
                request = Mapper.Mapper.NameAndAddressParam(name, address)
            };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndAddressRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonSmallByNameAndAddress = method
            };
            return Call(
                () => webService.GetPersonSmallByNameAndAddress(request),
                r => PersonMapper.Map(r.GetPersonSmallByNameAndAddressResponse)
                );
        }

        public PersonSmall GetPerson(NameParam name, DateTime dateOfBirth)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(dateOfBirth, "dateOfBirth");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndDateOfBirth()
            {
                request = Mapper.Mapper.NameAndDateOfBirthParam(name, dateOfBirth)
            };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndDateOfBirthRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonSmallByNameAndDateOfBirth = method
            };
            return Call(
                () => webService.GetPersonSmallByNameAndDateOfBirth(request),
                r => PersonMapper.Map(r.GetPersonSmallByNameAndDateOfBirthResponse)
                );
        }

    }
}
