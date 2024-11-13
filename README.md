To run this project, follow these steps:

Step 1: add .env file for S3 with key props:
- AWS_ACCESS_KEY_ID = ...
- AWS_SECRET_ACCESS_KEY = ...
- AWS_REGION = *AWS Region code*


Step 2: add db connection string



===============================================================================
+ App Features:
- Basic log in (check matching username and password)
- Authorisation: only admin user can see user list and perform CRUD on user details and product details
- Add product to cart
- update cart item and total price in cart page
- store log in and cart detail in session
- store product images on S3
- log out
