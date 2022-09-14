select
c.Id as ClientId,
c.FirstName+' '+c.LastName as Name,
b.Code as BankCode,
b.Name as BankName,
cu.Code as Currency
,b.Balance
from Clients c,BankAccounts b , Currencies cu 
where c.Id=b.ClientId AND cu.Id=b.CurrencyId