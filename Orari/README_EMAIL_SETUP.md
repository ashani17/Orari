# Email Verification Setup

This application now includes email verification functionality. Users must confirm their email address before they can log in.

## Email Configuration

### 1. Update appsettings.json

Edit the `EmailSettings` section in `appsettings.json`:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "Orari University"
  }
}
```

### 2. Gmail Setup (Recommended)

1. **Enable 2-Factor Authentication** on your Gmail account
2. **Generate an App Password**:
   - Go to Google Account settings
   - Security → 2-Step Verification → App passwords
   - Generate a new app password for "Mail"
   - Use this password in `SmtpPassword`

### 3. Other Email Providers

You can use any SMTP provider. Common settings:

**Outlook/Hotmail:**
```json
{
  "SmtpServer": "smtp-mail.outlook.com",
  "SmtpPort": 587
}
```

**Yahoo:**
```json
{
  "SmtpServer": "smtp.mail.yahoo.com",
  "SmtpPort": 587
}
```

## Features Implemented

### Backend
- ✅ Email service with SMTP support
- ✅ Email confirmation tokens
- ✅ Password reset functionality
- ✅ User registration requires email confirmation
- ✅ Admin user creation requires email confirmation
- ✅ Professor creation requires email confirmation

### Frontend
- ✅ Email confirmation page (`/confirm-email`)
- ✅ Resend confirmation email functionality
- ✅ Login page shows email confirmation message
- ✅ Password reset flow

## API Endpoints

### Authentication
- `POST /api/authentication/register` - Register new user (sends confirmation email)
- `POST /api/authentication/login` - Login (requires email confirmation)
- `POST /api/authentication/confirm-email` - Confirm email with token
- `POST /api/authentication/resend-confirmation` - Resend confirmation email
- `POST /api/authentication/forgot-password` - Send password reset email
- `POST /api/authentication/reset-password` - Reset password with token

### Admin
- `POST /api/admin/students` - Create student (sends confirmation email)
- `POST /api/admin/professors` - Create professor (sends confirmation email)

## Email Templates

The application includes HTML email templates for:
- Email confirmation
- Password reset

## Security Features

- Email confirmation tokens expire after 24 hours
- Password reset tokens expire after 1 hour
- Tokens are cryptographically secure
- Email addresses are validated before sending

## Testing

To test email functionality:

1. Configure your email settings
2. Register a new user
3. Check your email for the confirmation link
4. Click the link to confirm your email
5. Try logging in

## Troubleshooting

### Common Issues

1. **"Failed to send email" error**
   - Check your SMTP settings
   - Verify your email credentials
   - Ensure 2FA is enabled for Gmail

2. **Emails not received**
   - Check spam folder
   - Verify email address is correct
   - Check SMTP server settings

3. **"Invalid confirmation link" error**
   - Links expire after 24 hours
   - Use the resend confirmation feature
   - Check that the link is complete

### Development Mode

For development, you can temporarily disable email confirmation by setting `EmailConfirmed = true` in the user creation code, but this is not recommended for production.

## Production Considerations

1. **Use a dedicated email service** like SendGrid, Mailgun, or AWS SES
2. **Set up proper DNS records** (SPF, DKIM, DMARC)
3. **Monitor email delivery rates**
4. **Implement email queue for high volume**
5. **Use environment variables** for sensitive email credentials 