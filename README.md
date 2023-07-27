# Mobility Ways Coding Test
This Repo contains my solution to the coding test for Mobilty Ways.

## Setup
In order to run this you will need to set a local user secret with a 128 bit key for the JWT to use
For exmaple:

```json
{
  "JwtSecretKey": "Hom2FVLQXqKmWT7VGw3PjlQr46YTjSpfOzM1ykx8Y5wQDfzPlaieq4HIk3jTRtcyaWIlO8CCDH4YDaTrQCREHuHcZIbDfgI61hjYZ3BmavGa2J0Bxe2MImROZOKo1VPp"
}
```

The project is configured to run a Swagger host on startup that will allow you to create a User, get a JWT Token and then use that token to get a list of users