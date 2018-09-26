using Models;
using Repositories;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace Bridges
{
    public class BridgeCommandService
    {
        public SqliteResult<Contact> LoadContacts(int id = 0)
        {
            SqliteResult<Contact> operation = null;

            try
            {
                operation = new ContactRepository().SelectAllAsync().Result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("BridgeCommandService.LoadContacts -> {0}", e.ToString());
            }

            return operation;
        }

        public SqliteResult<Contact> Contacts(string command, Contact entity = null)
        {
            SqliteResult<Contact> operation = null;

            try
            {
                switch (command)
                {
                    case "insert":
                        operation = new ContactRepository().InsertAsync(entity).Result;
                        break;
                    case "delete":
                        operation = new ContactRepository().DeleteAsync(entity).Result;
                        break;
                    case "update":
                        operation = new ContactRepository().UpdateAsync(entity).Result;
                        break;
                    case "selectall":
                        operation = new ContactRepository().SelectAllAsync().Result;
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("BridgeCommandService.Contacts -> {0}", e.ToString());
            }

            return operation;
        }

        public string ShowMessage(string message, string caption = "Message")
        {
            MessageBox.Show(message, caption);
            return message;
        }
    }
}
