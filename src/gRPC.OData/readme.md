# gRPC.OData

This project is for the [gRPC & OData](http://comingsoon) post. Feel free to leave your comments and questions.

## Structure

1) [gRPC.OData.Server](./gRPC.OData.Server) : an ASP.NET Core Web Application
2) [gRPC.OData.Client](./gRPC.OData.Client) : an console application to consume the server

## Play

1) Run the gRPC.OData.Server

![image](https://user-images.githubusercontent.com/9426627/162533441-5463653a-1bdc-418d-ac2a-e0f491fec849.png)


2) Run the gRPC.OData.Client

A simple menu shows:

![image](https://user-images.githubusercontent.com/9426627/162533488-bad81e58-3093-42e7-afb7-8239edff5e15.png)

Type "1" from your keyboard and enter:

![image](https://user-images.githubusercontent.com/9426627/162534898-3eafc4ca-553e-4143-8922-6decadb42857.png)


Then, press any key and type "2" from your keyboard and enter again:

```C#

OData: List Shelves:
--Status code: OK
--Response body:
{
  "@odata.context": "https://localhost:7260/odata/$metadata#Shelves",
  "value": [
    {
      "Id": 1,
      "Theme": "Fiction"
    },
    {
      "Id": 2,
      "Theme": "Classics"
    },
    {
      "Id": 3,
      "Theme": "Computer"
    }
  ]
}

OData: Create shelf:
--Status code: Created
--Response body:
{
  "@odata.context": "https://localhost:7260/odata/$metadata#Shelves/$entity",
  "Id": 4,
  "Theme": "Poetry"
}

OData: Create book for shelf '4':
--Status code: Created
--Response body:
{
  "@odata.context": "https://localhost:7260/odata/$metadata#Books/$entity",
  "Id": 1,
  "Title": "Natasha Wing",
  "Author": "The Night Before Easter"
}

OData: Create book for shelf '4':
--Status code: Created
--Response body:
{
  "@odata.context": "https://localhost:7260/odata/$metadata#Books/$entity",
  "Id": 2,
  "Title": "Rupi Kaur ",
  "Author": "Milk and Honey"
}


OData: List books at shelf '4':
--Status code: OK
--Response body:
{
  "@odata.context": "https://localhost:7260/odata/$metadata#Books",
  "value": [
    {
      "Id": 1,
      "Title": "Natasha Wing",
      "Author": "The Night Before Easter"
    },
    {
      "Id": 2,
      "Title": "Rupi Kaur ",
      "Author": "Milk and Honey"
    }
  ]
}


OData: List Shelves:
--Status code: OK
--Response body:
{
  "@odata.context": "https://localhost:7260/odata/$metadata#Shelves",
  "value": [
    {
      "Id": 1,
      "Theme": "Fiction"
    },
    {
      "Id": 2,
      "Theme": "Classics"
    },
    {
      "Id": 3,
      "Theme": "Computer"
    },
    {
      "Id": 4,
      "Theme": "Poetry"
    }
  ]
}

OData: Delete shelf at '4':
--Status code: NoContent


OData: List Shelves:
--Status code: OK
--Response body:
{
  "@odata.context": "https://localhost:7260/odata/$metadata#Shelves",
  "value": [
    {
      "Id": 1,
      "Theme": "Fiction"
    },
    {
      "Id": 2,
      "Theme": "Classics"
    },
    {
      "Id": 3,
      "Theme": "Computer"
    }
  ]
}


Press any key to continue....
```
