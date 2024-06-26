namespace OnlineShopNET.Domain.Config
{
    public static class Constant_Messages
    {
        public const string NULL_TOKEN = "Access Token cannot be null";
        public const string INVALID_TOKEN = "Invalid Token";
        public const string ALLOWED_ADMINS_ONLY = "Admins Only";
        public const string ALLOWED_USERS_ONLY = "Users Only";
        public const string UNLOGGED_USER = "Please log in to perform this operation";
        public const string DUPLICATED_USERNAME_EMAIL = "There's already an username or email created with that name, Try another one";
        public const string INVALID_CREDENTIALS = "Invalid Credentials";
        public const string NON_EXISTENT_PRODUCT = "This product doesn't exist";
        public const string USER_NOT_FOUND = "User Not found";
        public const string PRODUCT_NOT_FOUND = "Product Not found or available";
        public const string QUANTITY_REQUIRED = "Need to order at least 1 product";
        public const string PRODUCT_LIMIT_EXCEED = "Store doesn't have that quantity for your order, please lower it";
        public const string UNSUCCESSFUL_ORDER_CREATING = "An error occurred while creating the order:";
        public const string SMTP_EMAIL= "smtp-mail.outlook.com";
        public const string STORE_EMAIL = "testNetaerm@hotmail.com";
        public const string STORE_PASSWORD = "AeRmTestAPIs2024@@++";
        public const string STORE_SUBJECT = "Purchase Confirmation";
        public const string INVALID_ROLE = "Role type must be either 'User' or 'Admin'.";
        public const string INVALID_EMAIL = "Invalid email address.";



    }
}
