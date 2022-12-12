using AutoMapper;
using MyHostAPI.Common.Configurations;
using MyHostAPI.Domain;
using MyHostAPI.Domain.Reporting;
using MyHostAPI.Domain.Premise;
using MyHostAPI.Models.Premise;
using MyHostAPI.Models;
using MyHostAPI.Domain.PredefinedFilter;
using MyHostAPI.Models.PredefinedFilter;
using MyHostAPI.Common.Helpers;
using Microsoft.Extensions.Configuration;
using static MyHostAPI.Configuration.MappingProfiles;

namespace MyHostAPI.Configuration
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region UserMapping
            CreateMap<User, RegisterModel>()
                .ForMember(x => x.Email, y => y.MapFrom(src => src.Identity.Email))
                .ForMember(x => x.Password, y => y.MapFrom(src => src.Identity.Password))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.PhoneNumber))
                .ReverseMap();
            CreateMap<User, UserUpdateModel>()
                .ForMember(x => x.Image, y => y.Ignore())
                .ReverseMap();
            CreateMap<User, UserResponseModel>()
                .ForMember(x => x.Email, y => y.MapFrom(src => src.Identity.Email))
                .ForMember(x => x.Role, y => y.MapFrom(src => src.Identity.Role))
                .ForMember(x => x.Address, y => y.MapFrom(src => src.Address));
            #endregion

            #region PremiseMapping
            CreateMap<Premise, PremiseModel>().ForMember(x => x.Images, y => y.Ignore());
            CreateMap<PremiseModel, Premise>().ForMember(x => x.Images, y => y.Ignore()).ForMember(x => x.RatingAverage, y => y.Ignore());
            CreateMap<Premise, PremiseResponseModel>().ReverseMap();
            CreateMap<PremiseModel, PremiseResponseModel>().ReverseMap();
            CreateMap<PremiseTimeSettings, PremiseTimeSettingsModel>().ReverseMap();
            CreateMap<PremiseWorkHours, PremiseWorkHoursModel>().ReverseMap();
            #endregion

            #region LocationMapping
            CreateMap<Location, LocationModel>().ReverseMap();
            #endregion

            #region Tag
            CreateMap<Tag, TagModel>().ReverseMap();
            #endregion

            #region ImageMapping
            CreateMap<Image, ImageModel>().ForMember(x => x.ImageFile, y => y.Ignore()).ReverseMap();
            #endregion

            #region AddressMapping
            CreateMap<Address, AddressModel>().ReverseMap();
            #endregion

            #region ReservationMapping
            CreateMap<Reservation, ReservationModel>().ReverseMap();
            CreateMap<ReservationManagerUpdateModel, Reservation>().ReverseMap();
            CreateMap<Reservation, ReservationCustomerUpdateModel>().ReverseMap();
            #endregion

            #region ReviewMapping
            CreateMap<Review, ReviewModel>().ForMember(x => x.CreatedOn, y => y.MapFrom(z => z.CreatedOn));
            CreateMap<ReviewModel, Review>().ForMember(x => x.CreatedOn, y => y.Ignore());
            CreateMap<ReviewUpdateModel, Review>();
            #endregion

            #region EventMapping
            CreateMap<Event, EventModel>().ReverseMap();
            CreateMap<Event, EventUpdateModel>().ReverseMap();
            #endregion

            #region EmailModelMapping
            CreateMap<SendGridEmailSettingsSection, MyHostInformation>().ReverseMap();
            #endregion

            #region PredefinedFilterMapping
            CreateMap<PredefinedFilter, PredefinedFilterModel>().ReverseMap();
            #endregion

            #region OperatingCityMapping
            CreateMap<OperatingCity, OperatingCityModel>().ReverseMap();
            #endregion

            #region PremiseTypes
            CreateMap<PremiseType, PremiseTypeModel>().ReverseMap();
            #endregion

            #region PaginatedList
            CreateMap<PaginatedList<Premise>, PaginatedList<PremiseResponseModel>>().ConvertUsing<PaginationResolver<Premise, PremiseResponseModel>>();
            CreateMap<PaginatedList<User>, PaginatedList<UserResponseModel>>().ConvertUsing<PaginationResolver<User, UserResponseModel>>();
            CreateMap<PaginatedList<Location>, PaginatedList<LocationModel>>().ConvertUsing<PaginationResolver<Location, LocationModel>>();
            CreateMap<PaginatedList<Reservation>, PaginatedList<ReservationModel>>().ConvertUsing<PaginationResolver<Reservation, ReservationModel>>();
            CreateMap<PaginatedList<Event>, PaginatedList<EventModel>>().ConvertUsing<PaginationResolver<Event, EventModel>>();
            CreateMap<PaginatedList<OperatingCity>, PaginatedList<OperatingCityModel>>().ConvertUsing<PaginationResolver<OperatingCity, OperatingCityModel>>();
            CreateMap<PaginatedList<PremiseType>, PaginatedList<PremiseTypeModel>>().ConvertUsing<PaginationResolver<PremiseType, PremiseTypeModel>>();
            CreateMap<PaginatedList<PredefinedFilter>, PaginatedList<PredefinedFilterModel>>().ConvertUsing<PaginationResolver<PredefinedFilter, PredefinedFilterModel>>();
            CreateMap<PaginatedList<Review>, PaginatedList<ReviewModel>>().ConvertUsing<PaginationResolver<Review, ReviewModel>>();
            #endregion
        }

        public class PaginationResolver<T, K> : ITypeConverter<PaginatedList<T>, PaginatedList<K>>
        {
            public PaginatedList<K> Convert(PaginatedList<T> source, PaginatedList<K> destination, ResolutionContext context)
            {
                var paginatedList = new PaginatedList<K>();
                foreach (var item in source)
                {
                    paginatedList.Add(context.Mapper.Map<K>(item));
                }
                paginatedList.Metadata = source.Metadata;
                return paginatedList;
            }
        }
    }
}
