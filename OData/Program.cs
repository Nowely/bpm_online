using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace OData
{
    class Program
    {
        #region
        private const string serverUri = "http://localhost:82/0/ServiceModel/EntityDataService.svc/";
        private const string authServiceUtri = "http://localhost:82//ServiceModel/AuthService.svc/Login";
        private static readonly XNamespace ds = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        private static readonly XNamespace dsmd = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        private static readonly XNamespace atom = "http://www.w3.org/2005/Atom";
        #endregion
        // Получить список средств связи контакта;
        public static void GetOdataObjectByIdExample()
        {
            string contactId = "128DD20F-1D9F-45C9-B4CB-9C225A109DA8";
            string requestUri = serverUri + "ContactCommunicationCollection(guid'" + contactId + "')";
            var request = HttpWebRequest.Create(requestUri) as HttpWebRequest;
            request.Method = "GET";
            request.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            using (var response = request.GetResponse())
            {
                XDocument xmlDoc = XDocument.Load(response.GetResponseStream());
                var contacts = from entry in xmlDoc.Descendants(atom + "entry")
                               select new
                               {
                                   Id = new Guid(entry.Element(atom + "content")
                                                         .Element(dsmd + "properties")
                                                         .Element(ds + "Id").Value),
                                   Number = entry.Element(atom + "content")
                                                   .Element(dsmd + "properties")
                                                   .Element(ds + "Number").Value
                               };
                foreach (var contact in contacts)
                {
                }
            }
        }

        public static void GetCollectionContactsName()
        {
            string contactId = "410006E1-CA4E-4502-A9EC-E54D922D2C00";
            string requestUri = serverUri + "ContactCollection";
            //string requestUri1 = serverUri + "ContactCollection?$filter=CreatedOn gt datetime'" + datetime +
            //"'and CreatedBy/Name eq '" + userName + "'";
            var request = HttpWebRequest.Create(requestUri) as HttpWebRequest;
            request.Method = "GET";
            request.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            using (var response = request.GetResponse())
            {
                XDocument xmlDoc = XDocument.Load(response.GetResponseStream());
                var contacts = from entry in xmlDoc.Descendants(atom + "entry")
                               select new
                               {
                                   Id = new Guid(entry.Element(atom + "content")
                                                     .Element(dsmd + "properties")
                                                     .Element(ds + "Id").Value),
                                   Name = entry.Element(atom + "content")
                                               .Element(dsmd + "properties")
                                               .Element(ds + "Name").Value,
                               };
                foreach (var contact in contacts)
                {
                    Console.WriteLine("{0} {1}", contact.Id, contact.Name);
                }
            }
        }
        public static void GetCollectionCommunication(Guid compId)
        {
            string requestUri1 = serverUri + "ContactCommunicationCollection";
            var request1 = HttpWebRequest.Create(requestUri1) as HttpWebRequest;
            request1.Method = "GET";
            request1.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            var response = request1.GetResponse();
            XDocument xmlDoc = XDocument.Load(response.GetResponseStream());
            var contacts = from entry in xmlDoc.Descendants(atom + "entry")
                           select new
                           {
                               CommunicationTypeId = new Guid(entry.Element(atom + "content")
                                                 .Element(dsmd + "properties")
                                                 .Element(ds + "CommunicationTypeId").Value),
                               ContactId = new Guid(entry.Element(atom + "content")
                                                 .Element(dsmd + "properties")
                                                 .Element(ds + "ContactId").Value),
                               Number = entry.Element(atom + "content")
                                           .Element(dsmd + "properties")
                                           .Element(ds + "Number").Value,
                           };
            foreach (var contact in contacts)
            {

            }
            string requestUri2 = serverUri + "CommunicationTypeCollection";
            var request2 = HttpWebRequest.Create(requestUri2) as HttpWebRequest;
            request2.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            var response1 = request2.GetResponse();
            XDocument xmlDoc1 = XDocument.Load(response1.GetResponseStream());
            var contacts1 = from entry in xmlDoc1.Descendants(atom + "entry")
                            select new
                            {
                                Id = new Guid(entry.Element(atom + "content")
                                                  .Element(dsmd + "properties")
                                                  .Element(ds + "Id").Value),
                                Name = entry.Element(atom + "content")
                                            .Element(dsmd + "properties")
                                            .Element(ds + "Name").Value,
                            };
            foreach (var contact in contacts)
            {
                if (contact.ContactId == compId)
                {
                    foreach (var contact1 in contacts1)
                        if (contact.CommunicationTypeId == contact1.Id)
                        {
                            Console.WriteLine("{0} {1}", contact1.Name, contact.Number);
                        }
                }
            }
        }

        // Добавить средство связи контакту; 
        public static void CreateBpmEntityByOdataHttpExample(Guid compId)
        {
            // Создание сообщения xml, содержащего данные о создаваемом объекте.
            var content = new XElement(dsmd + "properties",
                          new XElement(ds + "ContactId", compId),
                          new XElement(ds + "CommunicationTypeId", "EE1C85C3-CFCB-DF11-9B2A-001D60E938C6"),
                          new XElement(ds + "Number", "aaa@aa.ru"));
            var entry = new XElement(atom + "entry",
                        new XElement(atom + "content",
                        new XAttribute("type", "application/xml"), content));
            Console.WriteLine(entry.ToString());
            // Создание запроса к сервису, который будет добавлять новый объект в коллекцию контактов.
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUri + "ContactCommunicationCollection/");
            request.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            request.Method = "POST";
            request.Accept = "application/atom+xml";
            request.ContentType = "application/atom+xml;type=entry";
            // Запись xml-сообщения в поток запроса.
            using (var writer = XmlWriter.Create(request.GetRequestStream()))
            {
                entry.WriteTo(writer);
            }
            // Получение ответа от сервиса о результате выполнения операции.
            using (WebResponse response = request.GetResponse())
            {
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.Created)
                {
                    // Обработка результата выполнения операции.
                }
            }
        }
        // Удалить существующее средство связи контакта.
        public static void DeleteBpmEntityByOdataHttpExample(string IdcommForDel)
        {
            // Создание запроса к сервису, который будет удалять данные.
            var request = (HttpWebRequest)HttpWebRequest.Create(serverUri
                    + "ContactCommunicationCollection(guid'" + IdcommForDel + "')");
            request.Credentials = new NetworkCredential("Supervisor", "Supervisor");
            request.Method = "DELETE";
            // Получение ответа от сервися о результате выполненя операции.
            using (WebResponse response = request.GetResponse())
            {
                // Обработка результата выполнения операции.
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите пользователя");
            GetCollectionContactsName();
            Console.WriteLine("Введите Id пользователя");
            GetCollectionCommunication(new Guid(Console.ReadLine()));
            //Добавление средвства связи
            //CreateBpmEntityByOdataHttpExample(new Guid("410006E1-CA4E-4502-A9EC-E54D922D2C00"));
            //Удаление средства связи
            //DeleteBpmEntityByOdataHttpExample("FBDECB16-E5EC-4430-8BED-5D50D6BFE6F0);
            Console.ReadKey();
        }
    }
}
