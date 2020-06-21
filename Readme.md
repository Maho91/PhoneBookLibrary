# Phone Book Library  

This project satisfies the following use cases:-

1.	Create a new Phone Book Library.
2.	Query all entries in the library. The query can be ordered according to the following parameters

      a- **fn_asc**: Refers to First Name Ascending.

      b. **fn_desc**: Refers to First Name Descending.

      c- **ln_asc**: Refers to Last Name Ascending.

      d- **ln_desc**: Refers to Last Name Descending.
3.	Query specific entry by name.
4.	Update a specific entry by sending the old name as a parameter with the corresponding body.
5.	Delete a specific entry by sending the name as a parameter.
6.	Delete the Phone book library
7.	Get the phone book library’s Uri


## Prerequisites
1-	Visual Studio 2019 installed on your machine.

2-	Postman installed on your machine (to test RESTful API)

## Installation
Clone the project and open the PhoneBook.sln file to display, build, and run the project, or to deploy it to your IIS. As well as you can test the library by specifying a path for the data file and running the test from the Visual Studio app. Alternatively, you can run the test from your command line terminal via “VSTest.Console.exe” (after setting the environment variable in your windows)

#### Rest API Routs 

For testing the REST APIs using postman, you can download the [YAML](https://choosealicense.com/licenses/mit/) documentation file to import it as an Open API option to the postman. By doing so, the documentation file will create the routes automatically to prepare a testing environment within Postman that can be found under the collection tap. Alternatively, you can test the Rest API via using the below information manually.

> #### 1- Create New Entry 

**Post Method**: <Domain>/api/phonebook/

##### Post Headers: 
-	Content-Type: application/json
-	Accept: application json
#####	Post JSON body example
``` 
{
  "Name": " Joseph Tribbiani ",
  "PhoneBookEntryDetails": [
    {
      "Type": "Office",
      "Number": "0553040490"
    },
    {
      "Type": "Cellphone",
      "Number": "0684049385"
    }
  ]
}
```
====================================
> #### 2- Get All Entries in the Phone Book

**Get Method** : <Domain>/api/phonebook/All/{orderby}
##### Parameter:
- {orderby}:can be fn_asc, fn_desc, ln_asc or ln_desc

====================================
> #### 3- Get By name 

**Get Method** : <Domain>/api/phonebook/{name}
- {name}: can be any name at least two parts (e.g. Joseph Tribbiani)

====================================
> #### 4- Update Entry

**Pub Method**: <Domain>/api/phonebook/{name}

##### Post Headers: 
-	Content-Type: application/json
-	Accept: application json
##### Parameter:
- {name}: old entry's name
#####	Post JSON body example
``` 
{
  "Name": " Joseph Tribbiani ",
  "PhoneBookEntryDetails": [
    {
      "Type": "Office",
      "Number": "0553040490"
    },
    {
      "Type": "Cellphone",
      "Number": "0684049385"
    }
  ]
}
```
====================================
> #### 5- Delete Entry 

**Delete Method** : <Domain>/api/phonebook/{name}
- {name}: old entry's name


====================================
> #### 6- Get Phone Book Library File Uri 

**Get Method** : <Domain>/api/phonebook/file

====================================

====================================
> #### 7- Delete Phone Book Library File

**Delete Method** : <Domain>/api/phonebook/file

====================================

