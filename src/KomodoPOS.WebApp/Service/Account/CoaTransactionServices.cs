using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service.Account
{
    public class CoaTransactionServices
    {
        public void Debit(Guid coaId, string name, string desciption, decimal amount)
        {
            var tx = new DataLayer.DADataContext();

            var coa = tx.COAs.FirstOrDefault(x => x.Id == coaId);
            coa.Saldo += amount;

            var newTran = new DataLayer.COATransaction()
            {
                Id = Guid.NewGuid(),
                COAId = coa.Id,
                CreatedDate = DateTime.Now,
                Name = name,
                Description = desciption,
                Debit = amount,
                Balance = coa.Saldo
            };

            tx.COATransactions.InsertOnSubmit(newTran);
            tx.SubmitChanges();
            tx.Dispose();
        }

        public void Credit(Guid coaId, string name, string desciption, decimal amount)
        {
            var tx = new DataLayer.DADataContext();

            var coa = tx.COAs.FirstOrDefault(x => x.Id == coaId);
            coa.Saldo -= amount;

            var newTran = new DataLayer.COATransaction()
            {
                Id = Guid.NewGuid(),
                COAId = coa.Id,
                CreatedDate = DateTime.Now,
                Name = name,
                Description = desciption,
                Credit = amount,
                Balance = coa.Saldo
            };

            tx.COATransactions.InsertOnSubmit(newTran);
            tx.SubmitChanges();
            tx.Dispose();
        }
    }
}