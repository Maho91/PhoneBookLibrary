using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhoneBookLibrary
{
    /// <summary>
    /// Phone Book Entry Class
    ///  <list>
    /// <item><em>Name</em></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// This class has a main property Name for the entry of a full name and multiple entry details as phone Number and Type
    /// </remarks>

    [Serializable]
    public class PhoneBookEntry
    {
        /// <summary>
        ///  FullName Master Property 
        /// </summary>
        [Required(ErrorMessage = "Name parameter is required")]
        public string Name { get; set; }

        /// <summary>
        ///  Entry Details Property 
        /// <list>
        /// <item><em>Number Type</em></item>
        /// <item><em>Phone Number</em></item>
        /// </list>
        /// </summary>
        public List<PhoneBookEntryDetails> PhoneBookEntryDetails { get; set; }

        #region Ctor
        public PhoneBookEntry()
        {
            PhoneBookEntryDetails = new List<PhoneBookEntryDetails>();
        }
        #endregion
    }
}
