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
                .From("NWLibrary")
                .Where("Id")
                    .IsEqual(Column.Parameter(bookId)) as Select;
            var bookName = query.ExecuteScalar<string>();
            query = new Select(_userConnection)
                    .Column("NWNotes")
                .From("NWLibrary")
                .Where("Id")
                    .IsEqual(Column.Parameter(bookId)) as Select;
            var noteName = query.ExecuteScalar<string>();

            var pageInsert = new Insert(_userConnection)
                   .Into("NWLibrary")
                .Set("NWName", Column.Const(bookName))
                .Set("NWNotes", Column.Const(noteName));
            pageInsert.Execute();
            #endregion

            #region Запрашиваем данные детальки книги
            var query2 = new Select(_userConnection)
                    .Column("NWAuthorToBookId")
                .From("NWAuthorAndBook")
                .Where("NWBookToAuthorId")
                    .IsEqual(Column.Parameter(bookId)) as Select;
            var authorToBook = query2.ExecuteScalar<Guid>();
            #endregion

            #region Запрашиваем ID новой книги
            query = new Select(_userConnection)
                    .Column("Id")
                .From("NWLibrary")
                .Where("Id")
                    .IsNotEqual(Column.Parameter(bookId))
                    .And("NWName").IsEqual(Column.Const(bookName)) as Select;
            var idNewBook = query.ExecuteScalar<Guid>();

            var detailInsert = new Insert(_userConnection)
                   .Into("NWAuthorAndBook")
                .Set("NWBookToAuthorId", Column.Const(idNewBook))
                .Set("NWAuthorToBookId", Column.Const(authorToBook));
            detailInsert.Execute();
            #endregion
        }
        public void DoCopyESM(Guid bookId)
        {
            var opportunityManager = _userConnection.EntitySchemaManager.GetInstanceByName("NWLibrary");
            var bookForCopy = opportunityManager.CreateEntity(_userConnection);
            bool exist = bookForCopy.FetchFromDB(new Dictionary<string, object>()
            {
                { "Id", bookId }
            });


            
            //var opportunityManager1 = _userConnection.EntitySchemaManager.GetInstanceByName("NWAuthorAndBook");
            //var detailsData = opportunityManager1.CreateEntity(_userConnection);
            //detailsData.FetchFromDB(new Dictionary<string, object>()
            //{
            //    { "NWBookToAuthor", bookId }
            //});
            //var a = detailsData.GetColumnValueNames();

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
