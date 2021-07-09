using System;
using System.Collections.Generic;

namespace Us.FolkV3.Api.Client
{
    using Model;
    using Model.Param;
    using Mapper;

    public class PersonMediumClient : PrivilegesMediumClient
    {
        private PersonMediumMapper PersonMapper { get; }

        public PersonMediumClient(HeldinConfig config)
            : base(config)
        {
            PersonMapper = new PersonMediumMapper();
        }

        protected override List<Type> ListOfOperationClasses(List<Type> operationClasses)
        {
            base.ListOfOperationClasses(operationClasses).AddRange(
                new List<Type>()
                {
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPrivateId),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPublicId),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPtal),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndAddress),
                    typeof(Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndDateOfBirth),
                });
            return operationClasses;
        }

        public PersonMedium GetPerson(PrivateId id)
        {
            CheckCanUsePrivateId();
            Util.RequireNonNull(id, "id");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPrivateId() { request = Mapper.Mapper.PrivateId(id) };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPrivateIdRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonMediumByPrivateId = method
            };
            return Call(
                () => webService.GetPersonMediumByPrivateId(request),
                r => PersonMapper.Map(r.GetPersonMediumByPrivateIdResponse)
                );
        }

        public PersonMedium GetPerson(PublicId id)
        {
            CheckCanUsePublicId();
            Util.RequireNonNull(id, "id");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPublicId() { request = Mapper.Mapper.PublicId(id) };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPublicIdRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonMediumByPublicId = method
            };
            return Call(
                () => webService.GetPersonMediumByPublicId(request),
                r => PersonMapper.Map(r.GetPersonMediumByPublicIdResponse)
                );
        }

        public PersonMedium GetPerson(Ptal ptal)
        {
            Util.RequireNonNull(ptal, "ptal");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPtal() { request = ptal.Value };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPtalRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonMediumByPtal = method
            };
            return Call(
                () => webService.GetPersonMediumByPtal(request),
                r => PersonMapper.Map(r.GetPersonMediumByPtalResponse)
                );
        }

        public PersonMedium GetPerson(NameParam name, AddressParam address)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(address, "address");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndAddress()
            {
                request = Mapper.Mapper.NameAndAddressParam(name, address)
            };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndAddressRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonMediumByNameAndAddress = method
            };
            return Call(
                () => webService.GetPersonMediumByNameAndAddress(request),
                r => PersonMapper.Map(r.GetPersonMediumByNameAndAddressResponse)
                );
        }

        public PersonMedium GetPerson(NameParam name, DateTime dateOfBirth)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(dateOfBirth, "dateOfBirth");
            var method = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndDateOfBirth()
            {
                request = Mapper.Mapper.NameAndDateOfBirthParam(name, dateOfBirth)
            };
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndDateOfBirthRequest()
            {
                client = clientHeader,
                service = ServiceHeader(method.GetType()),
                userId = userIdHeader,
                id = IdHeader(),
                issue = issueHeader,
                protocolVersion = protocolVersionHeader,
                GetPersonMediumByNameAndDateOfBirth = method
            };
            return Call(
                () => webService.GetPersonMediumByNameAndDateOfBirth(request),
                r => PersonMapper.Map(r.GetPersonMediumByNameAndDateOfBirthResponse)
                );
        }

    }
}