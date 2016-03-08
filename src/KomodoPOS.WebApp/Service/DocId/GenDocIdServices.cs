using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace KomodoLaundry.WebApp.Service.DocId
{
    public class GenDocIdServices
    {
        private string _branchId = "0"; //0 is All Branch
        private bool _isGetOnly = false;

        public GenDocIdServices()
        {

        }
        
        public GenDocIdServices(string branchId)
        {
            _branchId = branchId;
        }
        public GenDocIdServices(string branchId, bool isGetOnly)
        {
            _branchId = branchId;
            _isGetOnly = isGetOnly;
        }
        public GenDocIdServices(bool isGetOnly)
        {
            _isGetOnly = isGetOnly;
        }

        public string STO()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }

        public string USE()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }

        public string PUR()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }

        public string BTR()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Expense
        /// </summary>
        /// <returns></returns>
        public string EXP()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }

        public string BDE()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }

        public string BWD()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }

        public string ORD()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }

        public string DEP()
        {
            return GenerateFunction(MethodBase.GetCurrentMethod().Name);
        }


        //---------------------------------------------
        private string GenerateFunction(string docName)
        {
            var tx = new DataLayer.DADataContext();
            
            var data = tx.DocIdSequences.FirstOrDefault(x => x.Name == docName);

            if (!_isGetOnly)
            {
                data.LastSequenceNumber++;
                tx.SubmitChanges();
            }
            if (_isGetOnly)
                data.LastSequenceNumber++;

            string resultId = data.LastSequenceNumber.ToString().PadLeft(4, '0');
            string resultBranchId = _branchId.PadLeft(2, '0');
            
            return docName + resultBranchId + resultId + DateTime.Now.ToString("MMyy");
        }

        public void SaveLastIdFunction(string docName)
        {
            var tx = new DataLayer.DADataContext();

            var data = tx.DocIdSequences.FirstOrDefault(x => x.Name == docName);
            data.LastSequenceNumber++;
            tx.SubmitChanges();            
        }
    }
}