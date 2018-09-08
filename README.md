# Custom login with ASP.NET MVC + REST API with ASP.NET Web API 

This C# project explores how to implement a very basic login form with ASP.NET MVC and a custom user model with roles. The application also has a REST API endpoint exposing the “User” resource. 

For the learning purpose of this example I have avoided to use the new ASP.NET Identity system and also the previous ASP.NET membership. For this approach I use the AuthorizeAttribute in different ways to perform the authentication and authorization task. For obvious security issues I do not recommend follow this example in a real world application.

## Software versions used

- Visual Studio 2017 Community
- Web API 2
- MVC 5

## How To Use

Start Visual Studio and select **Open Project / Solution** from the Start page. Or, from the File menu, select **Open** and then **Project/Solution**. Press **F5** to start debugging the application. The web page should look like the following:

![](Docs/Images/index.png)


If you try to access one of the links without the right session, you should be redirected to the login page:

![](Docs/Images/login.png)

Once you have entered the username and password with the proper role, you should have access to the requested page:

![](Docs/Images/page1.png)

Otherwise if you don't enter with the right role, you will be redirected to an access denied page.

![](Docs/Images/denied.png)

All the usernames and passwords for this example are stored in **users.xml** file in the **App_Data** folder of the project.

## How it works

The solution has two projects:

![](Docs/Images/solution.png)

The StartUp project has all the files needed to build and run de app. There is also a test project with some UnitTesting files that can be run through **Test Explorer**.

In the **Controllers** folder we can find three separate classes.

- **HomeController** handles the part of the application with the index, login, logout and denied actions. The **login action** uses the **FormsAuthentication** class for maintain an authentication ticket in a cookie. If necessary it also uses the returnUrl parameter for redirect the user to the page that was intended to access. This parameter is injected by the **AuthorizeAttribute** when a user try to access to a private page without the right session established.

    ```c
        if (username != null)
        {
            FormsAuthentication.SetAuthCookie(u.Username, false);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction(returnUrl.Split('/')[2], returnUrl.Split('/')[1]);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    ```

 - **PrivateController** manage the private part of the application. It uses **CustomAuthorize** class inherited from AuthorizeAttribute to restrict the access to an action method in case the user is not authenticated and redirect him to a loginUrl configured in the Web.config file.

    ```xml
        <authentication mode="Forms">
            <forms loginUrl="/Home/Login" slidingExpiration="true" timeout="5"></forms>
        </authentication>
    ```
    In case of unauthorized redirects the user to a denied view.

    ```c
        if (filterContext.HttpContext.User.Identity.IsAuthenticated &&
        filterContext.Result is HttpUnauthorizedResult)
        {
            filterContext.Result = new RedirectResult("~/Home/Denied");
        }
    ```
- **UserController** contains the REST API methods that exposures the User resource. It uses the **BasicAuthentication** class to implement a HTTP basic authentication with the help of an AuthorizeAttribute.

    ```c
    public override void OnAuthorization(HttpActionContext actionContext)
    {
        var authHeader = actionContext.Request.Headers.Authorization;

        if (authHeader != null)
        {
            var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
            var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
            var usernamePasswordArray = decodedAuthenticationToken.Split(':');
            var userName = usernamePasswordArray[0].ToLower();
            var password = usernamePasswordArray[1];

            UserBusinessLayer userBL = new UserBusinessLayer();
            List<User> users = userBL.GetUsers();
            User user = users.Where(item => item.Username.ToLower().Equals(userName) && item.Password.Equals(password)).FirstOrDefault();

            if (user != null && isRoleAuth(user))
            {
                var principal = new GenericPrincipal(new GenericIdentity(userName), null);
                Thread.CurrentPrincipal = principal;

                return;
            }
        }

        HandleUnathorized(actionContext);
    }
    ```
In the Filters folder we have the two implementations of AuthorizeAttribute used by the controllers.

The Models folder contains the **User** model as well as the class **UserBusinessLayer** which is responsible for the business rules that determine how data can be created, stored, and changed. This class is used by de UserController and implements the IUserBusinessLayer interface that can be injected in the constructor for testing purposes. In this folder we also have a custom role provider that overrides the GetRolesForUser method for get the roles of the business layer.

In the DataAccessLayer folder we have the class **XmlDbDAL** responsible of the persistence of the User model that is used by de UserBusinessLayer. In this particular case we are using an xml file for this matter. This class implements de interface IUserDbDAL which is helpful for testing following a dependency injection pattern.

The ViewModels folder has a class that represents the User model for exposure the fields that we want in the read operations through API.  

Finally the **App_Data** folder contains an users.xml file which stores of the user model data.

```xml
  <User>
    <Username>admin</Username>
    <Password>123</Password>
    <Roles>
      <string>ADMIN</string>
      <string>PAGE_1</string>
      <string>PAGE_2</string>
      <string>PAGE_3</string>
    </Roles>
  </User>
```

## API

### Show users

Returns json data about a list of users.

- **URL**: /api/user
- **Method**: GET
- **Success Response**: 
    - Code: 200
    - Content:
        ```json
        [
            {
                "Username": "admin",
                "Roles": [
                    "ADMIN",
                    "PAGE_1",
                    "PAGE_2",
                    "PAGE_3"
                ]
            },
            {
                "Username": "page1",
                "Roles": [
                    "PAGE_1"
                ]
            }
        ]
        ```
- **Error Response**:
    - Code: 401 UNAUTHORIZED 

### Show user

Returns json data about a single user.

- **URL**: /api/user/:username
- **Method**: GET
- **URL Params**:
    - Required: username=[string]
- **Success Response**: 
    - Code: 200
    - Content:
        ```json
        {
            "Username": "admin",
            "Roles": [
                "ADMIN",
                "PAGE_1",
                "PAGE_2",
                "PAGE_3"
            ]
        }
        ```
- **Error Response**:
    - Code: 404 NOT FOUND 
    - Code: 401 UNAUTHORIZED 

### Add user

Adds a single user

- **URL**: /api/user
- **Method**: POST
- **Data params**:
    - Example:
        ```json
        {
            "Username": "page3",
            "Password": "123",
            "Roles": [
                "PAGE_3"
            ]
        }
        ```
- **Success Response**: 
    - Code: 201 CREATED
    - Content:
        ```json
        {
            "Username": "page3",
            "Password": "123",
            "Roles": [
                "PAGE_3"
            ]
        }
        ```
- **Error Response**:
    - Code: 400 BAD REQUEST
    - Code: 409 CONFLICT
    - Code: 401 UNAUTHORIZED 


### Update user

Updates a single user

- **URL**: /api/user/:username
- **Method**: PUT
- **URL Params**:
    - Required: username=[string]
- **Data params**:
    - Example:
        ```json
        {
            "Username": "page4",
            "Password": "123",
            "Roles": [
                "PAGE_4"
            ]
        }
        ```
- **Success Response**: 
    - Code: 204 NO CONTENT

- **Error Response**:
    - Code: 400 BAD REQUEST
    - Code: 401 UNAUTHORIZED 

### Delete user

Deletes a single user

- **URL**: /api/user/:username
- **Method**: DELETE
- **URL Params**:
    - Required: username=[string]
- **Success Response**: 
    - Code: 204 NO CONTENT

- **Error Response**:
    - Code: 404 NOT FOUND 
    - Code: 401 UNAUTHORIZED 