Fabricam Leads
==============

Fabricam Leads is a sample app to show data concurrency and unit testing.


Running the Site
----------------

1. Download and install [Docker CE](https://store.docker.com/search?offering=community&type=edition).
   *Note*: Docker for Windows sits atop Hyper-V which requires Windows 10 Pro or better patched to Anniversary Update or better.

2. Clone this repo

3. `docker-compose up` will build and run all the containers

4. Watch the console output to see the C# unit tests pass and the application start up

5. Browse to [http://localhost:5000/](http://localhost:5000/) and click through the app.


Items Demonstrated
------------------

### Data Concurrency

Look at `Sql/setup.sql` for the stored procedure that checks out exactly one lead, marking it as checked out as we go.  This ensures a lead is only presented to exactly one person.


### Unit Testing

There are unit tests in:

- UserApi.Tests: these tests validate the UserApi service
- WebCore.Tests: these tests validate critical pieces of the Web.Core project
- [http://localhost:5000/Lead/JavaScriptTests](http://localhost:5000/Lead/JavaScriptTests): these tests validate the Vue application services on the Leads page


Items to build
--------------

### Integration Testing

This sample could benefit from integration testing:

- Web.Core/Services/LoadLeadsService: an integration test could validate integration with both the db and http://randomuser.me/

- Web.Core/Services/AuthenticateService: an integration test could validate integration with UserApi microservice

- Web: selenium tests could validate the site pulls up correctly and functions as expected

- Sql: [tSQLt](http://tsqlt.org/user-guide/quick-start/) could validate the stored procedure works as expected


Legal
-----

- License: MIT
- Copyright: Richardson
- IsLawyer: no
- CanUse: yes
