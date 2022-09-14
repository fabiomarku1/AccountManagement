select p.Id as ProductId,p.Name,p.Price,p.ShortDescription
from Categories c,Products p
where p.CategoryId=c.Id and p.CategoryId=id
order by p.DateCreated