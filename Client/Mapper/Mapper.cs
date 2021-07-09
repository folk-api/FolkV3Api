using System;
using System.Collections.Generic;
using System.Linq;

namespace Us.FolkV3.Api.Client.Mapper
{
    using Api.Model;
    using Api.Model.Param;

    internal static class Mapper
    {

        public static Address Address(Eu.Xroad.UsFolkV3.Producer.Address address)
        {
            if (address == null)
            {
                return new Address(null, null, null, null, null, null, null, null);
            }
            return new Address(
                NullIfBlank(address.streetAndNumbers),
                NullIfBlank(address.street),
                HouseNumber(address.houseNumber),
                NullIfBlank(address.apartment),
                NullIfBlank(address.postalCode),
                NullIfBlank(address.city),
                Country(address.country),
                Date(address.from, true, "address.from")
                );
        }

        public static Address Address(Eu.Xroad.UsFolkV3.Producer.PersonSmall person)
        {
            return Address(person.address);
        }

        public static CivilStatus CivilStatus(Eu.Xroad.UsFolkV3.Producer.PersonMedium person)
        {
            if (person.civilStatus == null)
            {
                return null;
            }
            return new CivilStatus(EnumMapper.CivilStatusType(person.civilStatus.type), DateTime.Parse(person.civilStatus.from));
        }

        public static Country Country(Eu.Xroad.UsFolkV3.Producer.Country country)
        {
            if (country == null)
            {
                throw new FolkApiException("no country");
            }
            if (string.IsNullOrWhiteSpace(country.code) || string.IsNullOrWhiteSpace(country.nameFo)
                    || string.IsNullOrWhiteSpace(country.nameEn))
            {
                throw new FolkApiException("invalid country");
            }
            return new Country(
                country.code,
                country.nameFo,
                country.nameEn
            );
        }

        public static DateTime? DateOfBirth(string value)
        {
            return Date(value, false, "DateOfBirth");
        }

        public static DateTime? DateOfDeath(string value)
        {
            return Date(value, false, "DateOfDeath");
        }

        public static DateTime? Date(string value, bool required, string what)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (required)
                {
                    throw new FolkApiException("no date of " + what);
                }
                return null;
            }
            try
            {
                return DateTime.Parse(value);
            }
            catch (Exception e)
            {
                throw new FolkApiException("invalid date of " + what + ' ' + value, e);
            }
        }
        public static Gender Gender(Eu.Xroad.UsFolkV3.Producer.PersonSmall person) => EnumMapper.Gender(person.gender);

        public static Guardian Guardian(Eu.Xroad.UsFolkV3.Producer.Guardian guardian)
        {
            if (guardian == null)
            {
                return null;
            }
            var name = Name(guardian.name);
            var address = Address(guardian.address);
            return name.IsComplete() && address.IsComplete ? new Guardian(name, address) : null;
        }

        public static HouseNumber HouseNumber(Eu.Xroad.UsFolkV3.Producer.HouseNumber houseNumber)
        {
            if (houseNumber == null)
            {
                return null;
            }
            return string.IsNullOrWhiteSpace(houseNumber.buildingCode)
                ? Model.HouseNumber.Create(houseNumber.number)
                : Model.HouseNumber.Create(houseNumber.number, houseNumber.buildingCode[0]);
        }

        public static Incapacity Incapacity(Eu.Xroad.UsFolkV3.Producer.Incapacity incapacity)
        {
            if (incapacity == null)
            {
                return null;
            }
            var guardian1 = Guardian(incapacity.guardian1);
            var guardian2 = Guardian(incapacity.guardian2);
            return guardian2 == null
                ? new Incapacity(guardian1, null)
                : new Incapacity(guardian1, guardian2);
        }

        public static Incapacity Incapacity(Eu.Xroad.UsFolkV3.Producer.PersonSmall person)
        {
            return person == null ? null : Incapacity(person.incapacity);
        }

        public static Name Name(Eu.Xroad.UsFolkV3.Producer.PersonName name)
        {
            return name == null
                ? null
                : new Name(
                    NullIfBlank(name.first),
                    NullIfBlank(name.middle),
                    NullIfBlank(name.last),
                    NullIfBlank(name.official)
                );
        }

        public static Name Name(Eu.Xroad.UsFolkV3.Producer.PersonSmall person)
        {
            return person == null ? null : Name(person.name);
        }

        public static PrivateId PrivateId(Eu.Xroad.UsFolkV3.Producer.PrivateId id)
        {
            return id == null ? null : Model.PrivateId.Create(id.value);
        }

        public static PrivateId PrivateId(Eu.Xroad.UsFolkV3.Producer.PersonSmall person)
        {
            return person == null ? null : PrivateId(person.privateId);
        }

        public static IList<PrivateId> PrivateIds(Eu.Xroad.UsFolkV3.Producer.PrivateId[] ids)
        {
            return (ids == null
                ? new List<PrivateId>()
                : ids.Select(id => Model.PrivateId.Create(id.value)).ToList()
                )
                .AsReadOnly();
        }

        public static Ptal Ptal(Eu.Xroad.UsFolkV3.Producer.PersonMedium person)
        {
            return person == null ? null : Model.Ptal.Create(person.ptal);
        }

        public static PublicId PublicId(Eu.Xroad.UsFolkV3.Producer.PublicId id)
        {
            return id == null ? null : Model.PublicId.Create(id.value);
        }

        public static PublicId PublicId(Eu.Xroad.UsFolkV3.Producer.PersonMedium person)
        {
            return person == null ? null : PublicId(person.publicId);
        }

        public static IList<PublicId> PublicIds(Eu.Xroad.UsFolkV3.Producer.PublicId[] ids)
        {
            return (ids == null
                ? new List<PublicId>()
                : ids.Select(id => Model.PublicId.Create(id.value)).ToList()
                )
                .AsReadOnly();
        }

        private static SpecialMarks SpecialMarks(string[] specialMarks)
        {
            return specialMarks == null
                ? null
                : new SpecialMarks(
                new HashSet<SpecialMarkType>(specialMarks.Select(v => EnumMapper.SpecialMarkType(v)).ToList())
                );
        }

        public static SpecialMarks SpecialMarks(Eu.Xroad.UsFolkV3.Producer.PersonSmall person)
        {
            return person == null ? null : SpecialMarks(person.specialMarks);
        }

        public static string NullIfBlank(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public static Eu.Xroad.UsFolkV3.Producer.NameAndAddressParam NameAndAddressParam(NameParam name, AddressParam address)
        {
            return new Eu.Xroad.UsFolkV3.Producer.NameAndAddressParam()
            {
                name = NameParam(name),
                address = AddressParam(address)
            };
        }

        public static Eu.Xroad.UsFolkV3.Producer.NameAndDateOfBirthParam NameAndDateOfBirthParam(NameParam name, DateTime dateOfBirth)
        {
            return new Eu.Xroad.UsFolkV3.Producer.NameAndDateOfBirthParam()
            {
                name = NameParam(name),
                dateOfBirth = dateOfBirth.ToISOString()
            };
        }

        public static Eu.Xroad.UsFolkV3.Producer.ChangesParam ChangesParam(DateTime from, DateTime to)
        {
            return new Eu.Xroad.UsFolkV3.Producer.ChangesParam()
            {
                from = from.ToISOString(),
                to = to.ToISOString()
            };
        }

        public static Eu.Xroad.UsFolkV3.Producer.PrivateId[] PrivateIdList(IList<PrivateId> ids)
        {
            return ids.Select(id => PrivateId(id)).ToArray();
        }

        public static Eu.Xroad.UsFolkV3.Producer.PublicId[] PublicIdList(IList<PublicId> ids)
        {
            return ids.Select(id => PublicId(id)).ToArray();
        }

        private static Eu.Xroad.UsFolkV3.Producer.AddressParam AddressParam(AddressParam address)
        {
            return new Eu.Xroad.UsFolkV3.Producer.AddressParam()
            {
                street = address.Street,
                houseNumber = new Eu.Xroad.UsFolkV3.Producer.HouseNumber()
                {
                    number = address.HouseNumber.Number,
                    buildingCode = address.HouseNumber.HasLetter ? address.HouseNumber.Letter.Value.ToString() : null
                },
                city = address.City
            };
        }

        private static Eu.Xroad.UsFolkV3.Producer.NameParam NameParam(NameParam param)
        {
            return new Eu.Xroad.UsFolkV3.Producer.NameParam()
            {
                first = param.First,
                last = param.Last
            };
        }

        public static Eu.Xroad.UsFolkV3.Producer.PrivateId PrivateId(PrivateId id)
        {
            return new Eu.Xroad.UsFolkV3.Producer.PrivateId()
            {
                value = id.Value
            };
        }

        public static Eu.Xroad.UsFolkV3.Producer.PublicId PublicId(PublicId id)
        {
            return new Eu.Xroad.UsFolkV3.Producer.PublicId()
            {
                value = id.Value
            };
        }

    }
}
