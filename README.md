# Mobility Ways Coding Test
This Repo contains my solution to the coding test for Mobility Ways. This is project runs on .Net 7 to gain some newer functionality. There is a minimal API with JWT bearer token authentication. It's using an in-memory data store to store a user.

## Spec
Create a .Net Minimal API that can offer the following endpoints:
- Create User - User should have Name, Email & Password
- Get JWT Token - Requires Email & Password to create a JWT for the user (validating the user exists first) 
- List Users - Passing a valid JWT Bearer token return a list of all users

## Setup
In order to run this you will need to set a local user secret with a 128 bit key for the JWT to use

For example:

```json
{
  "JwtSecretKey": "Hom2FVLQXqKmWT7VGw3PjlQr46YTjSpfOzM1ykx8Y5wQDfzPlaieq4HIk3jTRtcyaWIlO8CCDH4YDaTrQCREHuHcZIbDfgI61hjYZ3BmavGa2J0Bxe2MImROZOKo1VPp"
}
```

The project is configured to run a Swagger host on startup that will allow you to create a User, get a JWT Token and then use that token to get a list of users

## Notes
- I've tried to cap this to a few hours worth of work. With more time time I'd have included more Unit and Integration tests for the API. Hopefully the included unit tests around the business logic for users is a good example of what can be done.
- I've commented the code a little more than usual to give some thoughts, though it's probably a little overkill in some areas as I'd hope the code is mostly self explanatory.
- Most of the classes are sealed to improve performance and prevent unintended changes down the line, it acts as a bit of a gatekeeper to changes as well.
- Swagger has been configured to pass in the Auth token so it can be used to test the endpoints in order, or you can use a separate tool like postman or insomnia.
- A final note, I enjoyed this test. Always interesting to try and keep it simple but also demonstrate a wider area.
