using                                           Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneBookLibrary
{
    /// <summary>
    /// Phone Book Library class.
    /// </summary>
    /// <remarks>
    /// This class Contains all methods for performing all CRUD operations made on the phone library
    /// </remarks>
    public class PhoneBookRepository
    {

        #region General Methods

        //Represents a lock that is used to manage access to a resource, 
        //allowing multiple threads for reading or exclusive access for writing.
        public static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        #endregion

        #region Library Methods
        /// <summary>
        /// Orderd Get All Phone Book Entries Method
        /// </summary>
        /// <remarks>
        /// This method get all the phone book entries ordered by the following parames(fn_asc,fn_desc,ln_asc,ln_desc)
        /// </remarks>
        /// <param name="orderingType"> Ordering type parameter </param>
        /// <param name="filePath"> File pathe parameter </param>
        /// <returns>This method returns the data stored in the phone book in ordered  manner.
        /// By default, this method returns the phone book entries ordered by the first name ascending</returns>
        public IList<PhoneBookEntry> GetAll(string filePath, string orderby = "fn_asc")
        {
            // Enter a read lock
            _lock.EnterReadLock();

            try
            {
                IList<PhoneBookEntry> phoneBookEntries = new List<PhoneBookEntry>();

                //Check the existence of the Phone Book data file
                if (File.Exists(filePath) == true)
                {
                    using (Stream stream = File.Open(filePath, FileMode.Open))
                    {
                        if (stream.Length > 0)
                        {

                            //Read all the entries from the Phone Book data file
                            var bin = new BinaryFormatter();
                            phoneBookEntries = (List<PhoneBookEntry>)bin.Deserialize(stream);

                            switch (orderby)
                            {
                                //Order by ascending the first name alphabetically 
                                case "fn_asc":
                                    return phoneBookEntries.OrderBy(x => x.Name.Split(' ').First()).ToList();
                                //Order by descending the first name alphabetically 
                                case "fn_desc":
                                    return phoneBookEntries.OrderByDescending(x => x.Name.Split(' ').First()).ToList();
                                //Order by ascending the last name alphabetically 
                                case "ln_asc":
                                    return phoneBookEntries.OrderBy(x => x.Name.Split(' ').Last()).ToList();
                                //Order by descending the last name alphabetically 
                                case "ln_desc":
                                    return phoneBookEntries.OrderByDescending(x => x.Name.Split(' ').Last()).ToList();
                            }

                            return phoneBookEntries;
                        }
                    }
                }
                else
                {
                    throw new Exception("No phone book is created yet");

                }
                return phoneBookEntries;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                // Release the read lock afterward
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Get  Phone Book Entry By Name Method 
        /// </summary>
        /// <remarks>
        /// This method gets phone book entry by quering the name 
        /// </remarks>
        /// <param name="filePath"> File path parameter</param>
        /// <param name="name"> Entry name parameter</param>
        /// <returns>This method returns the requested Phone Book entry if exists</returns>
        public PhoneBookEntry GetBy(string filePath, string name)
        {
            try
            {
                PhoneBookEntry phoneBookEntries = new PhoneBookEntry();

                //Check the PhoneBook is exists   
                if (File.Exists(filePath) == true)
                {
                    //Check the entry exists in the phone book  
                    if (GetAll(filePath).Any(x => x.Name == name))
                    {
                        //Select the entry from the list  
                        phoneBookEntries = GetAll(filePath).Where(x => x.Name == name).FirstOrDefault();
                        return phoneBookEntries;
                    }
                    else
                    {
                        throw new Exception("The requested name does not exist in the phone book library");
                    }
                }
                else
                {
                    throw new Exception("No phone book is created yet");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Phone Book Create, Update, Delete Method 
        /// </summary>
        /// <remarks>
        /// This method creates a Phone Book data file and executes various methods according to the received "Mode" parameter
        /// </remarks>
        /// <param name="filePath"> File path parameter </param>
        /// <param name="entry"> Entry parameter for create and update operations</param>
        /// <param name="mode"> Mode parameter to select the operation type (update, delete, or create)</param>
        /// <param name="name"> Name parameter for update and delete operations  </param>
        /// <returns>This returns bool value, a (true) value stands for operation completed and (false) for error occurred</returns>
        public bool Insert(string filePath, PhoneBookEntry entry, string mode, string name)
        {
            try
            {
                IList<PhoneBookEntry> phoneBookList = new List<PhoneBookEntry>();
                var bin = new BinaryFormatter();

                // check if the file exists then open, if not create .bat file 
                if (File.Exists(filePath) == true)
                {
                    phoneBookList = GetAll(filePath);

                    //Check opreration mode "Add", "Update" or "Delete"
                    if (mode == "Update")
                    {

                        //Check if entry exists in the phone book
                        if (phoneBookList.Any(x => x.Name == name))
                        {
                            // Remove the old entry and add the new coming entry
                            PhoneBookEntry oldEntry = phoneBookList.Where(x => x.Name == name).FirstOrDefault();
                            
                            //Remove from the list 
                            phoneBookList.Remove(oldEntry);

                            // Enter a write lock
                            _lock.EnterWriteLock();

                            //Add to the list and store the list
                            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
                            {
                                phoneBookList.Add(entry);
                                bin.Serialize(stream, phoneBookList);
                            }

                            // Release the lock afterwards
                            _lock.ExitWriteLock();
                        }
                        else
                        {
                            throw new Exception("The requested record does not exist in the phone book");
                        }

                    }
                    else if (mode == "Delete")
                    {
                        //Check the requested entry exists in the Phone Book
                        if (phoneBookList.Any(x => x.Name == name))
                        {

                            // Remove the entry from the list 
                            PhoneBookEntry _entry = phoneBookList.Where(x => x.Name == name).FirstOrDefault();
                            phoneBookList.Remove(_entry);
                           
                            
                            // Enter a write lock
                            _lock.EnterWriteLock();


                            // Store the list after the update 
                            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
                            {
                                 bin.Serialize(stream, phoneBookList);
                            }

                            // Release the lock afterwards
                            _lock.ExitWriteLock();

                        }
                        else
                        {
                            throw new Exception("The requested record does not exist in the phone book");
                        }
                    }
                    else if (mode == "Add")
                    {

                        //Check if the new sent entry already exists in the Phone Book 
                        if (phoneBookList.Any(x => x.Name != entry.Name))
                        {

                            // Enter a write lock
                            _lock.EnterWriteLock();


                            // Add the new coming entry to the list and store the list  
                            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
                            {
                                phoneBookList.Add(entry);
                                 bin.Serialize(stream, phoneBookList);
                            }

                            // Release the lock afterwards
                            _lock.ExitWriteLock();

                        }
                        else
                        {
                            throw new Exception("There is already Phone entry with the same name");
                        }

                    }
                    else
                    {
                        throw new Exception("No phone book is created yet");

                    }
                }
                else if (!File.Exists(filePath) && mode == "Add")
                {
                    // Enter a write lock
                    _lock.EnterWriteLock();


                    // Create a phone book and add the new coming entry to an empty list and store the list to maintain the format of the file content
                    using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
                    {
                        phoneBookList.Add(entry);
                         bin.Serialize(stream, phoneBookList);
                    }

                    // Release the lock afterwards
                    _lock.ExitWriteLock();

                }
                else
                {
                    throw new Exception("No phone book is created yet");

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return true;
        }

        /// <summary>
        /// Update Method 
        /// </summary>
        /// <remarks>
        /// This method first validates the coming entry then checks the existence of the record in the phone book by querying the name parameter. 
        /// </remarks>
        /// <param name="filePath"> Phil path parameter</param>
        /// <param name="name"> The o4ld entry's name </param>
        /// <param name="entry"> The modified entry </param>
        public void Update(string filePath, string name, PhoneBookEntry entry)
        {
            try
            {

                //Validate the coming entry data  
                if (Validation(entry) == true)
                {

                    //Execute an update operation on the Phone Book  
                    Insert(filePath, entry, "Update", name);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Delete Method 
        /// </summary>
        /// <remarks>
        /// This method checks the existence of the record in the phone book by querying the name parameter. 
        /// </remarks>
        /// <param name="filePath">File path parameter</param>
        /// <param name="name"> Entry name parameter</param>
        public void Delete(string filePath, string name)
        {
            try
            {
                //Execute a delete operation on the Phone Book  
                Insert(filePath, null, "Delete", name);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Create Method 
        /// </summary>
        /// <remarks>
        /// This method first validates the coming entry then it creates a new record in the phone book. 
        /// </remarks>
        /// <param name="fileP"> File path parameter </param>
        /// <param name="entry"> New entry parameter</param>
        public void Create(string filePath, PhoneBookEntry entry)
        {
            try
            {

                //Validate the coming entry data  
                if (Validation(entry) == true)
                {

                     //Execute an add operation on the Phone Book  
                     Insert(filePath, entry, "Add", "");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Delete Phoke Book Method 
        /// </summary>
        /// <remarks>
        /// This method first checks the existence of the Phone Book file then delete if exists. 
        /// </remarks>
        public void DeleteFile(string filePath)
        {
            try
            {
                //Check the existence of the Phone Book data file
                if (File.Exists(filePath) == true)
                {

                    //Delete the Phone Book data file
                    Task.Run(() => File.Delete(filePath));
                }
                else
                {
                    throw new Exception("No phone book is created yet");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Get Phone Book Uri Method 
        /// </summary>
        /// <remarks>
        /// This method first checks the existence of the phone book file then creates a file Uri. 
        /// </remarks>
        /// <returns>This method returns a string value of the file path Uri.</returns>
        public string GetFile(string filePath)
        {
            try
            {
                //Check the existence of the Phone Book data file
                if (File.Exists(filePath) == true)
                {

                    //Get the file info of the received file path
                    FileInfo fileInfo =   new FileInfo(filePath);
                    return fileInfo.FullName; ;
                }
                else
                {
                    throw new Exception("No phone book is created yet");
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Phone Book Entry Validation Method 
        /// </summary>
        /// <returns>This method returns a true value as a valid entry, or throw an exception for an invalid entry.</returns>
        private bool Validation(PhoneBookEntry entry)
        {

            //Array of the acceptable number types 
            string[] numberType = { "Work", "Cellphone", "Home" };

            try
            {
                //Check if the name has First and Last parts
                if (entry.Name.Split(' ').Count() <= 1)
                {
                    throw new Exception("The name parameter is incorrect, the name must at least has two parts (first and last name)");
                }

                //Check if the entry has details body
                else if (entry.PhoneBookEntryDetails.Count == 0)
                {
                    throw new Exception("The name parameter has no phone number or type");
                }
                else
                {
                    foreach (var item in entry.PhoneBookEntryDetails)
                    {
                        //Check if the entry details body has a phone number
                        if (item.Number == null || item.Number == "")
                        {
                            throw new Exception("The name parameter has no phone number");
                        }
                        //Check if the entry details body has a number type
                        else if (item.Type == null || item.Type == "")
                        {
                            throw new Exception("The name parameter has no number type");
                        }
                        else 
                        {
                            //Check if the entry details has a valid number type
                            if (!numberType.Contains(item.Type))
                            {
                                throw new Exception("Number type parameter must be one of the following (Work,Cellphone or Home)");
                            }

                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
        #endregion



    }
}
