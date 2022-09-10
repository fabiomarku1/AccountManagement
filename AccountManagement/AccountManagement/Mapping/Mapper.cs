using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AutoMapper;

namespace AccountManagement.Mapping
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<ClientRegistrationDto, Client>();
            CreateMap<Client, ClientRegistrationDto>();
            CreateMap<Client, ClientViewModel>().ReverseMap();
            CreateMap<Client, ClientLogin>().ReverseMap();
            CreateMap<Client, Client>();


            CreateMap<Currency, CurrencyViewModel>().ReverseMap();
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            //   CreateMap<CurrencyDto, Currency>

            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Product, ProductViewModel>().ReverseMap();

            //CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
