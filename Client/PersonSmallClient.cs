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

        public PersonSmallClient(string consumerHost, string consumerName)
            : base(consumerHost, consumerName)
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
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPrivateId() { request = Mapper.Mapper.PrivateId(id) };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonSmallByPrivateId(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPrivateIdRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonSmallByPrivateId = request
                        }),
                        r => r.GetPersonSmallByPrivateIdResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

        public PersonSmall GetPerson(Ptal ptal)
        {
            Util.RequireNonNull(ptal, "ptal");
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPtal() { request = ptal.Value };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonSmallByPtal(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByPtalRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonSmallByPtal = request
                        }),
                        r => r.GetPersonSmallByPtalResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

        public PersonSmall GetPerson(NameParam name, AddressParam address)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(address, "address");
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndAddress()
            {
                request = Mapper.Mapper.NameAndAddressParam(name, address)
            };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonSmallByNameAndAddress(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndAddressRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonSmallByNameAndAddress = request
                        }),
                        r => r.GetPersonSmallByNameAndAddressResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

        public PersonSmall GetPerson(NameParam name, DateTime dateOfBirth)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(dateOfBirth, "dateOfBirth");
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndDateOfBirth()
            {
                request = Mapper.Mapper.NameAndDateOfBirthParam(name, dateOfBirth)
            };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonSmallByNameAndDateOfBirth(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonSmallByNameAndDateOfBirthRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonSmallByNameAndDateOfBirth = request
                        }),
                        r => r.GetPersonSmallByNameAndDateOfBirthResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

    }
}
