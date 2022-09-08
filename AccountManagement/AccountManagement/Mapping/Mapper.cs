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


            CreateMap<Currency,CurrencyViewModel>().ReverseMap();

            CreateMap<Category, CategoryViewModel>().ReverseMap();

            CreateMap<Product,ProductViewModel>().ReverseMap();

            //CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
