using System;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace GetNumberOfPerfomance
{
    public class ContractToGetNumber
    {
        private UserConnection _userConnection;

        public ContractToGetNumber(UserConnection uc)
        {
            _userConnection = uc;
        }
        public int GetNumber(int CodeOfProgramm)
        {
            var opportunityManager = _userConnection.EntitySchemaManager.GetInstanceByName("NWConcertProgramms");
            var concertProgramm = opportunityManager.CreateEntity(_userConnection);
            bool exist = concertProgramm.FetchFromDB("NWCode", CodeOfProgramm);

            if (exist)
            {
                var detailData = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "NWPerformance");
                detailData.AddColumn("Id");
                var esqFirst = detailData.CreateFilterWithParameters(FilterComparisonType.Equal,
                   "NWPerformanceStates",
                   new Guid("73CD01F5-2053-4005-BAEA-EF2E6644EE50"));
                var esqSecond = detailData.CreateFilterWithParameters(FilterComparisonType.Equal,
                   "NWConcertProgram",
                   concertProgramm.GetTypedColumnValue<Guid>("Id"));
                detailData.Filters.Add(esqFirst);
                detailData.Filters.Add(esqSecond);
                var entities = detailData.GetEntityCollection(_userConnection);


                return entities.Count;
            }
            else return -1;
        }
        //public void AddDetails()
        //{
        //    var opportunityManager = _userConnection.EntitySchemaManager.GetInstanceByName("NWLibrary");
        //    DateTime myDateTime = DateTime.Now;
        //    Random rnd = new Random();
        //    for (int i = 0; i < 8; i++)
        //    {
        //        myDateTime = myDateTime.AddDays(1);
        //        var details = opportunityManager.CreateEntity(_userConnection);
        //        details.SetDefColumnValues();
        //        details.SetColumnValue("NWConcertProgramId", "Сюда вставить EntityId что-то там");
        //        details.SetColumnValue("NWPerformanceStatesId", new Guid("73CD01F5-2053-4005-BAEA-EF2E6644EE50"));
        //        details.SetColumnValue("NWNumberOfTickets", rnd.Next(0, 100000));
        //        details.SetColumnValue("NWResponsibleId", new Guid("410006E1-CA4E-4502-A9EC-E54D922D2C00")); // здесь тоже можно потом поменять
        //        details.SetColumnValue("NWDateOfPerformance", myDateTime);
        //    }
        //}
    }
}
