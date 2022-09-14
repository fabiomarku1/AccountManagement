select b.Code as AccountCode , b.Name as AccountName , c.Code as Currency
from BankAccounts b,Currencies c
where b.CurrencyId=c.Id and b.IsActive='true' and b.ClientId=1