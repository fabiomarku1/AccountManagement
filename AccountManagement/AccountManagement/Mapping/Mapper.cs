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
            CreateMap<Product, ProductCreateUpdateDto>().ReverseMap();
            CreateMap<Product, ProductGDto>().ReverseMap();


            CreateMap<BankAccount, BankAccountCreateUpdateDto>().ReverseMap();
            CreateMap<BankAccount, BankAccountModelView>().ReverseMap();
            CreateMap<BankAccount, BankAccountGetDto>().ReverseMap();


            CreateMap<BankTransaction, BankTransactionCreateDto>().ReverseMap();
            CreateMap<BankTransaction, BankTransactionGetDto>().ReverseMap();

            CreateMap<Product, ProductCheckoutDTO>().ReverseMap();

            CreateMap<Sales, SalesDTO>().ReverseMap();


            //CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
