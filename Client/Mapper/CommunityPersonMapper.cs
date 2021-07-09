namespace Us.FolkV3.Api.Client.Mapper
{
    using Model;
    internal class CommunityPersonMapper : ClientMapper<Eu.Xroad.UsFolkV3.Producer.CommunityPerson, CommunityPerson>
    {
        private PersonSmallMapper PersonMapper { get; }

        public CommunityPersonMapper() {
            PersonMapper = new PersonSmallMapper();
        }

        protected override CommunityPerson DoMap(Eu.Xroad.UsFolkV3.Producer.CommunityPerson value)
        {
            return new CommunityPerson(
                    PersonMapper.Map(value.person),
                    Mapper.PrivateId(value.existingId),
                    Status(value)
                    );
        }

        public ResponseWrapper<CommunityPerson> Map(Eu.Xroad.UsFolkV3.Producer.CommunityPersonResponse r)
        {
            return new ResponseWrapper<CommunityPerson>(r, Map(r.result));
        }

        internal static CommunityPersonStatus Status(Eu.Xroad.UsFolkV3.Producer.CommunityPerson person)
        {
            return EnumMapper.CommunityPersonStatus(person.status);
        }
    }
}
