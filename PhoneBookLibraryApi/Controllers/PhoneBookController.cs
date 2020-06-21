using PhoneBookLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PhoneBookLibraryApi.Controllers
{
    public class PhoneBookController : ApiController
    {

        #region General methods 

        // Include the Phone Book library 
        PhoneBookRepository _repository = new PhoneBookRepository();

        // Set a Phone Book data file path
        private static string filePath = @"/DataFile.dat";
        #endregion

        #region Api Routes

        /// <summary>
        /// Get Method
        /// </summary>
        /// <remarks>
        /// This method reads all the phone book entries ordered by the following parames (fn_asc,fn_desc,ln_asc,ln_desc)
        /// </remarks>
        /// <param name="orderingType"> Ordering type parameter </param>
        /// <returns>This method returns a JSON response of data stored in the Phone Book in ordered  manner</returns>
        [Route("api/phonebook/all/{orderby}")]
        [HttpGet]
        public HttpResponseMessage Get(string orderby)
        {
            try
            {
                //Read all phone book entries
                var phoneBookList = _repository.GetAll(filePath, orderby);
                return Request.CreateResponse(HttpStatusCode.OK, phoneBookList);
            }
            catch (Exception ex)
            {
                //Capture the error and return only the error message and code in the response 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Get Method
        /// </summary>
        /// <remarks>
        /// This method reads an entry from the Phone Book by querying the entry name
        /// </remarks>
        /// <param name="name"> entry name parameter </param>
        /// <returns>This method returns a JSON response of the by queried entry </returns>
        [Route("api/phonebook/{name}")]
        [HttpGet]
        public HttpResponseMessage GetbyName(string name)
        {
            try
            {
                //Get Phone Book entry by name 
                var data = _repository.GetBy(filePath, name);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception e)
            {
                //Capture the error and return only the error message and code in the response 
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

        }

        /// <summary>
        /// Post Method
        /// </summary>
        /// <remarks>
        /// This method adds an entryto the Phone Book
        /// </remarks>
        /// <param name="json_body"> JSON body attached to the post method </param>
        /// <returns>This method returns a JSON response contains the newly created entry </returns>
        [Route("api/phonebook/")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] PhoneBookEntry entry)
        {

            try
            {
                //Validate the Request Body 
                if (ModelState.IsValid)
                {

                    //Create a new entry in the Phone book 
                    _repository.Create(filePath,entry);
                    return Request.CreateResponse(HttpStatusCode.Created, entry);
                }
                else
                {
                    //Return the "ModelState" validation result in the response 
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ModelState);
                }
            }
            catch (Exception ex)
            {
                //Capture the error and return only the error message and code in the response 
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, ex.Message);
            }
        }

        /// <summary>
        /// Put Method
        /// </summary>
        /// <remarks>
        /// This method updates a specific entry in the Phone Book by receiving the old entry name as a parameter.
        /// </remarks>
        /// <param name="json_body"> JSON body attached to the put method  </param>
        /// <returns>This method returns a JSON response contains the updated entry </returns>
        [Route("api/phonebook/{name}")]
        [HttpPut]
        public HttpResponseMessage Put(string name, [FromBody] PhoneBookEntry entry)
        {
            try
            {
                //Validate the Request Body 
                if (ModelState.IsValid)
                {
                    //Update the requested entry in the Phone book 
                    _repository.Update(filePath,name, entry);
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Update the recored with the name (" + name + ") has been successful");
                }
                else
                {
                    //Return the "ModelState" validation result in the response 
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, ModelState);
                }
            }
            catch (Exception ex)
            {
                //Capture the error and return only the error message and code in the response 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        /// <summary>
        /// Delete Method
        /// </summary>
        /// <remarks>
        /// This method deletes a specific entry from the Phone Book by receiving the entry name as a parameter.
        /// </remarks>
        /// <param name="name"> entry name parameter   </param>
        /// <returns>This method returns a response message of successful or invalid request </returns>
        [Route("api/phonebook/{name}")]
        [HttpDelete]
        public HttpResponseMessage Delete(string name)
        {
            try
            {
                //Delete the requested entry from the Phone book 
                _repository.Delete(filePath, name);
                return Request.CreateResponse(HttpStatusCode.OK, "The recored has been Deleted successfully");
            }
            catch (Exception ex)
            {
                //Capture the error and return only the error message and code in the response 
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, ex.Message);
            }
        }

        /// <summary>
        /// Delete Method
        /// </summary>
        /// <remarks>
        /// This method deletes the whole Phone Book file.
        /// </remarks>
        /// <returns>This method returns a response message of successful or invalid request </returns>
        [Route("api/phonebook/file/")]
        [HttpDelete]
        public HttpResponseMessage DeleteFile()
        {
            try
            {
                //Delete the Phone book 
                _repository.DeleteFile(filePath);
                return Request.CreateResponse(HttpStatusCode.OK, "The PhoneBook has been deleted");
            }
            catch (Exception ex)
            {
                //Capture the error and return only the error message and code in the response 
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        /// <summary>
        /// Delete Method
        /// </summary>
        /// <remarks>
        /// This method gets file info of the Phone Book .
        /// </remarks>
        /// <returns>This method returns a response message of the Phone book file Uri </returns>
        [Route("api/phonebook/file/")]
        [HttpGet]
        public HttpResponseMessage getFile()
        {
            try
            {
                //Read the Phone book file's info and return a Uri 
                string uri = new Uri(_repository.GetFile(filePath)).AbsoluteUri;
                return Request.CreateResponse(HttpStatusCode.OK, uri);
            }
            catch (Exception ex)
            {
                //Capture the error and return only the error message and code in the response 
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        #endregion

    }
}
