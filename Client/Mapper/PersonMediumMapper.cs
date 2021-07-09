namespace Us.FolkV3.Api.Client.Mapper
{
    using Model;

    internal class PersonMediumMapper : PersonBaseMapper<Eu.Xroad.UsFolkV3.Producer.PersonMedium, PersonMedium>
    {
        protected override PersonMedium DoMap(Eu.Xroad.UsFolkV3.Producer.PersonMedium value)
        {
            return new PersonMedium(
                Mapper.PrivateId(value),
                Mapper.PublicId(value),
                Mapper.Ptal(value),
                Mapper.Name(value),
                Mapper.DateOfBirth(value.dateOfBirth),
                Mapper.Gender(value),
                Mapper.Address(value),
                value.placeOfBirth,
                Mapper.SpecialMarks(value),
                Mapper.Incapacity(value),
                Mapper.CivilStatus(value),
                Mapper.DateOfDeath(value.dateOfDeath)
            );
        }

        public ResponseWrapper<PersonMedium> Map(Eu.Xroad.UsFolkV3.Producer.PersonMediumResponse r)
        {
            return new ResponseWrapper<PersonMedium>(r, Map(r.result));
        }

    }
}
