# ODataProtobufExample

This project is for the protobuf customized OData payload format. Feel free to leave your comments and questions.

## Protobuf writer

GET http://localhost:5023/odata/Shelves?$expand=Books

<img width="584" alt="image" src="https://user-images.githubusercontent.com/9426627/232616917-063cc5e8-79f3-4290-8888-2e641034b1e8.png">

If you specify the Accept header as below, you can get:

<img width="597" alt="image" src="https://user-images.githubusercontent.com/9426627/232617209-e1b2de27-0ff3-4b12-aed9-bd336386723a.png">

## Protobuf reader

POST http://localhost:5023/odata/shelves

![image](https://user-images.githubusercontent.com/9426627/232617632-dbe361e5-fb0a-4158-8e68-61e890f2585a.png)

Here's the response:
<img width="303" alt="image" src="https://user-images.githubusercontent.com/9426627/232617734-50901f70-e271-4072-a68d-2755d686d307.png">

