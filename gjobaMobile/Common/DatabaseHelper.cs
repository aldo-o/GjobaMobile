using gjobaMobile.Tabela;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gjobaMobile.Common
{
    public class DatabaseHelperClass
    {
        SQLiteConnection dbConn;

        //krijon tabelen
        public async Task<bool> onCreate(string DB_PATH)
        {
            try
            {
                if (!CheckFileExists(DB_PATH).Result)
                {
                    using (dbConn = new SQLiteConnection(DB_PATH))
                    {
                        dbConn.CreateTable<makinat>();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Retrieve the specific contact from the database. 
        public makinat ReadContact(int contactid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<makinat>("select * from makinat where Id =" + contactid).FirstOrDefault();
                return existingconact;
            }
        }

        // Retrieve the all contact list from the database. 
        public ObservableCollection<makinat> ReadContacts()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<makinat> myCollection = dbConn.Table<makinat>().ToList<makinat>();
                ObservableCollection<makinat> ContactsList = new ObservableCollection<makinat>(myCollection);
                return ContactsList;
            }
        }

        //Update existing conatct 
        public void UpdateContact(makinat contact)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<makinat>("select * from makinat where Id =" + contact.Id).FirstOrDefault();
                if (existingconact != null)
                {
                    existingconact.sqMakina = contact.sqMakina;
                    existingconact.sqTarga = contact.sqTarga;
                    existingconact.sqShasia = contact.sqShasia;
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existingconact);
                    });
                }
            }
        }
        
        // Insert the new contact in the Contacts table. 
        public void Insert(makinat newcontact)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newcontact);
                });
            }
        }

        //Delete specific contact 
        public void DeleteContact(int Id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<makinat>("select * from makinat where Id =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingconact);
                    });
                }
            }
        }

        //Delete all contactlist or delete Contacts table 
        public void DeleteAllContact()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() => 
                //   { 
                dbConn.DropTable<makinat>();
                dbConn.CreateTable<makinat>();
                dbConn.Dispose();
                dbConn.Close();
                //}); 
            }
        }
    }
}
