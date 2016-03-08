using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KomodoLaundry.WebApp.Service.Account
{
    public class GetCOADataAllServices
    {
        public List<Models.COA.COAModel> Get()
        {
            var tx = new DataLayer.DADataContext();

            return (from coa in tx.COAs
                    join globalData in tx.GlobalDatas on coa.GlobalDataId equals globalData.Id
                    select new Models.COA.COAModel()
                    {
                        Id = coa.Id,
                        GlobalData = globalData.Name,
                        Code = coa.Code,
                        Name = coa.Name,
                        Saldo = coa.Saldo
                    })
                    .ToList();
        }
    }
}