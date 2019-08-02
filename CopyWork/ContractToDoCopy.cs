using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Entities;

namespace CopyWork
{
    public class ContractToDoCopy
    {
        private UserConnection _userConnection;

        public ContractToDoCopy(UserConnection uc)
        {
            _userConnection = uc;
        }
        public void DoCopy(Guid bookId)
        {
            #region Запрашиваем данные книги, затем добавляем новую книгу
            var query = new Select(_userConnection)
                    .Column("NWName")
                    .Column("NWNotes")
                    .Column("NWDateOfPublication")
                    .Column("NWAuthorId")
                    .Column("NWisSale")
                    .Column("NWTirage")
                .From("NWLibrary")
                .Where("Id")
                    .IsEqual(Column.Parameter(bookId)) as Select;

            var insel = new InsertSelect(_userConnection)
                .Into("NWLibrary")
                .Set("NWName")
                .Set("NWNotes")
                .Set("NWDateOfPublication")
                .Set("NWAuthorId")
                .Set("NWisSale")
                .Set("NWTirage")
                .FromSelect(query);
            insel.Execute();
            var bookName = query.ExecuteScalar<string>();

            //Запрашиваем ID новой книги
            query = new Select(_userConnection)
                    .Column("Id")
                .From("NWLibrary")
                .Where("Id")
                    .IsNotEqual(Column.Parameter(bookId))
                    .And("NWName").IsEqual(Column.Const(bookName)) as Select;
            var idNewBook = query.ExecuteScalar<string>();

            //Данные детальки
            var query2 = new Select(_userConnection)
                    .Column(Column.Const(idNewBook))
                    .Column("NWAuthorToBookId")
                .From("NWAuthorAndBook")
                .Where("NWBookToAuthorId")
                    .IsEqual(Column.Parameter(bookId)) as Select;

            var insel1 = new InsertSelect(_userConnection)
                .Into("NWAuthorAndBook")
                .Set("NWBookToAuthorId","NWAuthorToBookId")
                .FromSelect(query2);
            insel1.Execute();
        }
        public void DoCopyESM(Guid bookId)
        {
            var opportunityManager = _userConnection.EntitySchemaManager.GetInstanceByName("NWLibrary");
            var bookForCopy = opportunityManager.CreateEntity(_userConnection);
            bool exist = bookForCopy.FetchFromDB(new Dictionary<string, object>()
            {
                { "Id", bookId }
            });

            if (exist)
            {
                var copedBook = opportunityManager.CreateEntity(_userConnection);

                copedBook.SetDefColumnValues();
                copedBook.SetColumnValue("NWName", bookForCopy.GetColumnValue("NWName"));
                copedBook.SetColumnValue("NWNotes", bookForCopy.GetColumnValue("NWNotes"));
                copedBook.SetColumnValue("NWDateOfPublication", bookForCopy.GetColumnValue("NWDateOfPublication"));
                copedBook.SetColumnValue("NWAuthorId", bookForCopy.GetColumnValue("NWAuthorId"));
                copedBook.SetColumnValue("NWisSale", bookForCopy.GetColumnValue("NWisSale"));
                copedBook.SetColumnValue("NWTirage", bookForCopy.GetColumnValue("NWTirage"));
                copedBook.Save();

                var detailData = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "NWAuthorAndBook");
                var detailDataColumn = detailData.AddColumn("NWBookToAuthor");
                var detailDataColumn1 = detailData.AddColumn("NWAuthorToBook");
                var esqFunction = detailData.CreateFilterWithParameters(FilterComparisonType.Equal,
                   "NWBookToAuthor",
                   bookId);
                detailData.Filters.Add(esqFunction);
                var entitydetailDataColumn = detailData.GetEntityCollection(_userConnection);

                var opportunityManager1 = _userConnection.EntitySchemaManager.GetInstanceByName("NWAuthorAndBook");
                foreach (var entity in entitydetailDataColumn)
                {
                    var detailsDataSet = opportunityManager1.CreateEntity(_userConnection);

                    detailsDataSet.SetDefColumnValues();
                    detailsDataSet.SetColumnValue("NWBookToAuthorId", copedBook.GetColumnValue("Id"));
                    detailsDataSet.SetColumnValue("NWAuthorToBookId", entity.GetTypedColumnValue<Guid>("NWAuthorToBookId"));
                    detailsDataSet.Save();
                }
            }
        }
    }
}
