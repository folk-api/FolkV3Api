namespace Us.FolkV3.Api.Client.Mapper
{
    using Model;

    internal class PersonSmallMapper : PersonBaseMapper<Eu.Xroad.UsFolkV3.Producer.PersonSmall, PersonSmall>
    {
        protected override PersonSmall DoMap(Eu.Xroad.UsFolkV3.Producer.PersonSmall value)
        {
            return new PersonSmall(
                Mapper.PrivateId(value),
                Mapper.Name(value),
                Mapper.Gender(value),
                Mapper.Address(value),
                value.placeOfBirth,
                Mapper.Incapacity(value),
                Mapper.SpecialMarks(value),
                Mapper.DateOfDeath(value.dateOfDeath)
            );
        }

        public ResponseWrapper<PersonSmall> Map(Eu.Xroad.UsFolkV3.Producer.PersonSmallResponse r)
        {
            return new ResponseWrapper<PersonSmall>(r, Map(r.result));
        }

    }
}
