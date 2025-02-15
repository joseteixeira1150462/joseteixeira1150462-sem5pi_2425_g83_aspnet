namespace HealthCare.Domain.Shared.Mailing
{
    public static class EmailTemplates
    {
        public static string AccountSetup(string username, string activationLink) {
            return $@"
            <html>
                <body>
                    <div style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #4CAF50;'>Welcome, {username}!</h2>
                        <p>We're excited to have you on board. To complete your registration, please activate your account by clicking the link below:</p>
                        <a href='{activationLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none;'>Activate Account</a>
                        <br><br>
                        <p>Thank you for joining us,</p>
                        <p>G83 Group</p>
                    </div>
                </body>
            </html>";
        }

        public static string AccountSetupConfirmation() {
            return $@"
            <html>
                <body>
                    <div style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #4CAF50;'>Account setup successfuly!</h2>
                        <p>Your account has been activated and password has been set.</p>
                        <p>You may use your new credentials to login to the Health Care application.</p>
                        <br><br>
                        <p>Thank you for joining us,</p>
                        <p>G83 Group</p>
                    </div>
                </body>
            </html>";
        }

        public static string UserPasswordReset(string username, string resetLink) {
            return $@"
            <html>
                <body>
                    <div style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #4CAF50;'>{username}, You may now reset your password</h2>
                        <p>You have requested a password reset.</p>
                        <a href='{resetLink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none;'>Reset Password</a>
                        <p>The link is valid for 24 hours.</p>
                        <br><br>
                        <p>Best regards,</p>
                        <p>G83 Group</p>
                    </div>
                </body>
            </html>";
        }
        public static string UserEmailUpdate(string username, string confirmationlink) {
            return $@"
            <html>
                <body>
                    <div style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #4CAF50;'>{username}, You may now update your e-mail</h2>
                        <p>You have requested a email edit.</p>
                        <a href='{confirmationlink}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none;'>Update Email</a>
                        <p>The link is valid for 24 hours.</p>
                        <br><br>
                        <p>Best regards,</p>
                        <p>G83 Group</p>
                    </div>
                </body>
            </html>";
        }
    }
}