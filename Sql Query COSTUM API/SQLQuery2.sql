select t.Action,t.Amount,t.DateCreated
from BankTransactions t,BankAccounts acc
where t.BankAccountId=acc.Id and t.BankAccountId=11
order by t.DateCreated