using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBookLibrary;

namespace PhoneBookLibraryTest
{

    /// <summary>
    /// Phone Book Library Testing Class
    /// </summary>
    [TestClass]
    public class PhoneBookTest
    {
        #region General Methods
        private PhoneBookEntry entity = new PhoneBookEntry();
        private PhoneBookRepository repository = new PhoneBookRepository();
        private static string filePath = @"TestDataFile.dat";
        #endregion

        #region Successfully Test Completed Methods
        /// <summary>
        /// Test Creating A New Phone Book Entry Method
        /// </summary>
        /// <remarks>
        /// This method tests creating a new record in the Phone Book library
        /// </remarks>
        [TestMethod]
        public void Test_Create_A_New_Phone_Book_Entry_Method()
        {
            //Delete the old phone book if exists
            if (File.Exists(filePath) == true)
            {
                File.Delete(filePath);
            }

            //Create new phone book
            entity.Name = "Joseph Tribbiani";
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Work", Number = "0554047784" });
            repository.Create(filePath, entity);
            
            //Get all the phone book entries
            var PhoneBookEntryList = repository.GetBy(filePath, "Joseph Tribbiani");
            Assert.IsTrue(PhoneBookEntryList.Name == "Joseph Tribbiani");
        }

        /// <summary>
        /// Test Reading All Phone Book Entries Method
        /// </summary>
        /// <remarks>
        /// This method tests reading all the Phone Book library entries
        /// </remarks>
        [TestMethod]
        public void Test_Get_All_Phone_Book_Entries_Method()
        {
            //Delete the old phone book if exists
            if (File.Exists(filePath) == true)
            {
                File.Delete(filePath);
            }

            //Create new phone book
            entity.Name = "Joseph Tribbiani";
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Work", Number = "0554047784" });
            repository.Create(filePath, entity);

            //Create another phone book entry
            entity = new PhoneBookEntry();
            entity.Name = "Chandler Bing";
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Work", Number = "0578047667" });
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554040084" });
            repository.Create(filePath, entity);

            //Get all the phone book entries
            var PhoneBookEntryList = repository.GetAll(filePath);

            //Check the length of the created phone book list to assure the success of the operations above
            Assert.IsTrue(PhoneBookEntryList.Count > 1);
        }

        /// <summary>
        /// Test Deleting A Phone Book Entry Method
        /// </summary>
        /// <remarks>
        /// This method tests deleting a specific entry from the Phone Book library
        /// </remarks>
        [TestMethod]
        public void Test_Delete_Phone_Book_Entry_Method()
        {
            //Delete the old phone book if exists
            if (File.Exists(filePath) == true)
            {
                File.Delete(filePath);
            }

            //Create new phone book
            entity.Name = "Joseph Tribbiani";
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Home", Number = "0554047784" });
            repository.Create(filePath, entity);

            //Get all the phone book entries
            var PhoneBookEntryList = repository.GetAll(filePath);

            //Check the existence of the old entry
            Assert.IsTrue(PhoneBookEntryList.Any(x => x.Name == "Joseph Tribbiani"));
            repository.Delete(filePath, "Joseph Tribbiani");

            //Get the updated phone book entries
            PhoneBookEntryList = repository.GetAll(filePath);
            //Check the existence of the deleted entry
            Assert.IsFalse(PhoneBookEntryList.Any(x => x.Name == "Joseph Tribbiani"));
        }

        /// <summary>
        /// Test Updating A Phone Book Entry Method
        /// </summary>
        /// <remarks>
        /// This method tests updating a specific entry in the Phone Book library
        /// </remarks>
        [TestMethod]
        public void Test_Update_Phone_Book_Entry_Method()
        {
            //Delete the old phone book if exists

            if (File.Exists(filePath) == true)
            {
                File.Delete(filePath);
            }

            //Create new phone book
            entity.Name = "Joseph Tribbiani";
            entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
            repository.Create(filePath, entity);

            //Get a specific entry by name for update purpose entry
            entity = repository.GetBy(filePath, "Joseph Tribbiani");

            //update the entry by changing the name
            entity.Name = "Ross Geller";
            repository.Update(filePath, "Joseph Tribbiani", entity);

            //Get all the phone book entries
            var PhoneBookEntryList = repository.GetAll(filePath);

            //Check the existence of the old entry
            Assert.IsFalse(PhoneBookEntryList.Any(x => x.Name == "Joseph Tribbiani"));
            //Check the existence of the updated entry
            Assert.IsTrue(PhoneBookEntryList.Any(x => x.Name == "Ross Geller"));
        }

        #endregion

        #region Exception Capturing  Test Completed Methods

        /// <summary>
        /// Test Creating A Phone Book Entry With Same Name Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of creating a new entry has the same name as an already
        /// created entry in the Phone Book library
        /// </remarks>
        [TestMethod]
        public void Test_Create_Phone_Book_Entry_With_Same_Name_Parameters_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book
                entity.Name = "Joseph Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
                 repository.Create(filePath, entity);

                //Create new entry with the same name
                entity.Name = "Joseph  Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
                 repository.Create(filePath, entity);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equals to "There is already Phone entry with the same name"
                Assert.AreEqual(ex.Message, "There is already Phone entry with the same name");
            }
        }

        /// <summary>
        /// Test Creating A Phone Book Entry With No Number Type Parameter Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of creating a new entry has no number type parameter in the request body
        /// </remarks>
        [TestMethod]
        public void Test_Create_Phone_Book_Entry_With_No_Number_Type_Parameters_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book entry with wrong parameters 
                entity.Name = "Joseph Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Number = "0554047777" });
                 repository.Create(filePath, entity);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equals to "The name parameter has no phone type"
                Assert.AreEqual(ex.Message, "The name parameter has no number type");
            }
        }

        /// <summary>
        /// Test Creating A Phone Book Entry With No Number Parameter Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of creating a new that has an invalid number type parameter in the request body
        /// </remarks>
        [TestMethod]
        public void Test_Create_Or_Edit_Phone_Book_Entry_Has_Invalid_Number_Type_Parameters_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book entry with wrong parameters 
                entity.Name = "Joseph Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
                repository.Create(filePath, entity);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equals to "Number type parameter must be one of the following (Work,Cellphone or Home)"
                Assert.AreEqual(ex.Message, "Number type parameter must be one of the following (Work,Cellphone or Home)");
            }
        }

        /// <summary>
        /// Test Creating A Phone Book With No Number Parameter Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of creating a new entry that has no number parameter in the request body
        /// </remarks>
        [TestMethod]
        public void Test_Create_Phone_Book_Entry_With_No_Number_Parameters_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book entry with wrong parameters 
                entity.Name = "Joseph Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone" });
                repository.Create(filePath, entity);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "The name parameter has no phone number"
                Assert.AreEqual(ex.Message, "The name parameter has no phone number");
            }
        }

        /// <summary>
        /// Test Creating A Phone Book Entry With No Phone Number Details Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of creating a new entry that has no number details parameter in the request body
        /// </remarks>
        [TestMethod]
        public void Test_Create_Phone_Book_Entry_With_No_Entry_Detail_Body_Parameters_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book entry with wrong parameters 
                entity.Name = "Joseph Tribbiani";
                repository.Create(filePath, entity);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "The name parameter has no phone number or type"
                Assert.AreEqual(ex.Message, "The name parameter has no phone number or type");
            }
        }

        /// <summary>
        /// Test Creating A Phone Book Entry With A Single Name Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of creating a new entry that has a single name (first or last) parameter in the request body
        /// </remarks>
        [TestMethod]
        public void Test_Create_Phone_Book_Entry_With_Single_Name_Parameters_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book entry with wrong parameters 
                entity.Name = "Joseph";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
                repository.Create(filePath, entity);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "The name parameter is incorrect, the name must at least has two parts (first and last name)"
                Assert.AreEqual(ex.Message, "The name parameter is incorrect, the name must at least has two parts (first and last name)");
            }
        }

        /// <summary>
        /// Test Updating A Non Exist Phone Book Entry Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of updating a non exist phone book entry
        /// </remarks>
        [TestMethod]
        public void Test_Update_Non_Exist_Phone_Book_Entry_From_The_Phone_Book_Library_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book  
                entity.Name = "Joseph Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
                repository.Create(filePath, entity);

                //Update a non exist phone book entry
                entity.Name = "Joey Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
                Task.Run(() => repository.Update(filePath, "Joey Tribbiani", entity));
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "The requested record does not exist in the phone book"
                Assert.AreEqual(ex.Message, "The requested record does not exist in the phone book");
            }
        }

        /// <summary>
        /// Test Reading A Non Exist Phone Book Entry Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of reading a non exist phone book entry
        /// </remarks>
        [TestMethod]
        public void Test_Get_Non_Exist_Phone_Book_Entry_From_The_Phone_Book_Library_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book  
                entity.Name = "Joseph Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
                 repository.Create(filePath, entity);

                //Get a non exist phone book entry
                 Task.Run(() => repository.GetBy(filePath, "Joey Tribbiani"));
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "The requested name does not exist in the phone book library"
                Assert.AreEqual(ex.Message, "The requested name does not exist in the phone book library");
            }
        }

        /// <summary>
        /// Test Deleting A Non Exist Phone Book Entry Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of deleting a non exist phone book entry from a non exsist phone book library
        /// </remarks>
        [TestMethod]
        public void Test_Delete_Non_Exist_Phone_Book_Entry_From_The_Phone_Book_Library_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Create new phone book  
                entity.Name = "Joseph Tribbiani";
                entity.PhoneBookEntryDetails.Add(new PhoneBookEntryDetails { Type = "Cellphone", Number = "0554047777" });
                 repository.Create(filePath, entity);

                //Delete a non exist phone book entry
                 Task.Run(() => repository.Delete(filePath, "Joey Tribbiani"));
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "The requested record does not exist in the phone book"
                Assert.AreEqual(ex.Message, "The requested record does not exist in the phone book");
            }
        }

        /// <summary>
        /// Test Geting A Non Exist Phone Book Library Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of geting a non exist phone book library
        /// </remarks>
        [TestMethod]
        public void Test_Get_Non_Exist_Phone_Book_Library_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Get a non exist phone book   
                repository.GetAll(filePath);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "No phone book is created yet"
                Assert.AreEqual(ex.Message, "No phone book is created yet");
            }
        }

        /// <summary>
        /// Test Geting A Non Exist Phone Book Entry From A Non Exist Phone Book Library Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of geting a non exist phone book entry from a non exsist phone book library
        /// </remarks>
        [TestMethod]
        public void Test_Get_Phone_Book_Entry_From_Non_Exist_Phone_Book_Library_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Get a non exist phone book entry from non exist phone book  
                repository.GetBy(filePath, "Joseph Tribbiani");
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "No phone book is created yet"
                Assert.AreEqual(ex.Message, "No phone book is created yet");
            }
        }

        /// <summary>
        /// Test Geting A Non Exist Phone Book Entry From A Non Exist Phone Book Library Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of deleting a non exist phone book entry from a non exsist phone book library
        /// </remarks>
        [TestMethod]
        public void Test_Delete_Phone_Book_Entry_From_Non_Exist_Phone_Book_Library_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Delete a non exist phone book entry from non exist phone book  
                 repository.Delete(filePath, "Joseph Tribbiani");
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "No phone book is created yet"
                Assert.AreEqual(ex.Message, "No phone book is created yet");
            }
        }

        /// <summary>
        /// Test Deleting A Non Exist Phone Book Library Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of deleting a non exist phone book library
        /// </remarks>
        [TestMethod]
        public void Test_Delete_Non_Exist_Phone_Book_Library_File_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Delete a non exist phone book entry from non exist phone book  
                repository.DeleteFile(filePath);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "No phone book is created yet"
                Assert.AreEqual(ex.Message, "No phone book is created yet");
            }
        }

        /// <summary>
        /// Test Geting A Non Exist Phone Book Library File Uri Method
        /// </summary>
        /// <remarks>
        /// This method assures the failure of geting a file info of a non exist phone book library
        /// </remarks>
        [TestMethod]
        public void Test_Get_Non_Exist_Phone_Book_Library_File_Method()
        {
            try
            {
                //Delete the old phone book if exists
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                //Delete a non exist phone book entry from non exist phone book  
               repository.GetFile(filePath);
            }
            catch (Exception ex)
            {
                //The function must respond with an exception message which equle to "No phone book is created yet"
                Assert.AreEqual(ex.Message, "No phone book is created yet");
            }
        }


        #endregion
    }
}
