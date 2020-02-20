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
        private PublicChangesMapper ChangesMapper { get; }

        public PersonMediumClient(string consumerHost, string consumerName)
            : base(consumerHost, consumerName)
        {
            PersonMapper = new PersonMediumMapper();
            ChangesMapper = new PublicChangesMapper();
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
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPrivateId() { request = Mapper.Mapper.PrivateId(id) };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonMediumByPrivateId(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPrivateIdRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonMediumByPrivateId = request
                        }),
                        r => r.GetPersonMediumByPrivateIdResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

        public PersonMedium GetPerson(PublicId id)
        {
            CheckCanUsePublicId();
            Util.RequireNonNull(id, "id");
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPublicId() { request = Mapper.Mapper.PublicId(id) };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonMediumByPublicId(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPublicIdRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonMediumByPublicId = request
                        }),
                        r => r.GetPersonMediumByPublicIdResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

        public PersonMedium GetPerson(Ptal ptal)
        {
            Util.RequireNonNull(ptal, "ptal");
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPtal() { request = ptal.Value };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonMediumByPtal(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByPtalRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonMediumByPtal = request
                        }),
                        r => r.GetPersonMediumByPtalResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

        public PersonMedium GetPerson(NameParam name, AddressParam address)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(address, "address");
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndAddress()
            {
                request = Mapper.Mapper.NameAndAddressParam(name, address)
            };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonMediumByNameAndAddress(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndAddressRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonMediumByNameAndAddress = request
                        }),
                        r => r.GetPersonMediumByNameAndAddressResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

        public PersonMedium GetPerson(NameParam name, DateTime dateOfBirth)
        {
            Util.RequireNonNull(name, "name");
            Util.RequireNonNull(dateOfBirth, "dateOfBirth");
            var request = new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndDateOfBirth()
            {
                request = Mapper.Mapper.NameAndDateOfBirthParam(name, dateOfBirth)
            };
            return CheckStatus(
                Call(() =>
                    WebService.GetPersonMediumByNameAndDateOfBirth(
                        new Eu.Xroad.UsFolkV3.Producer.GetPersonMediumByNameAndDateOfBirthRequest()
                        {
                            consumer = ConsumerHeader,
                            producer = ProducerHeader,
                            userId = UserIdHeader,
                            id = IdHeader(),
                            service = ServiceHeader(request.GetType()),
                            GetPersonMediumByNameAndDateOfBirth = request
                        }),
                        r => r.GetPersonMediumByNameAndDateOfBirthResponse.response
                    ),
                    r => PersonMapper.Map(r.result)
                );
        }

    }
}