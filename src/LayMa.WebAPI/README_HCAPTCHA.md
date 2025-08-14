# hCaptcha Integration for Code Generation

This document explains how to integrate hCaptcha verification before generating codes in the LayMa API.

## Configuration

### 1. Update appsettings.json

Add the following configuration to your `appsettings.json` file:

```json
{
  "HCaptcha": {
    "Enabled": true,
    "SiteKey": "your-hcaptcha-site-key",
    "SecretKey": "your-hcaptcha-secret-key",
    "VerifyUrl": "https://hcaptcha.com/siteverify"
  }
}
```

### 2. Get hCaptcha Keys

1. Go to [hCaptcha](https://www.hcaptcha.com/)
2. Sign up for an account
3. Create a new site
4. Get your Site Key and Secret Key

## Frontend Integration

### 1. Include hCaptcha Script

Add the hCaptcha script to your HTML:

```html
<script src="https://js.hcaptcha.com/1/api.js" async defer></script>
```

### 2. Add hCaptcha Widget

Add the hCaptcha widget to your form:

```html
<div class="h-captcha" data-sitekey="your-hcaptcha-site-key"></div>
```

### 3. JavaScript Integration

```javascript
// Function to get the hCaptcha token
function getHCaptchaToken() {
    return hcaptcha.getResponse();
}

// Function to reset hCaptcha
function resetHCaptcha() {
    hcaptcha.reset();
}

// Example API call
async function generateCode(trafficId, solution) {
    const hCaptchaToken = getHCaptchaToken();
    
    if (!hCaptchaToken) {
        alert('Please complete the hCaptcha verification');
        return;
    }
    
    const requestData = {
        trafficId: trafficId,
        solution: solution,
        hCaptchaToken: hCaptchaToken
    };
    
    try {
        const response = await fetch('/api/admin/codemanager/getcode', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData)
        });
        
        const result = await response.json();
        
        if (result.success) {
            console.log('Generated code:', result.html);
            resetHCaptcha(); // Reset for next use
        } else {
            console.error('Error:', result.html);
            resetHCaptcha();
        }
    } catch (error) {
        console.error('API call failed:', error);
        resetHCaptcha();
    }
}
```

## API Changes

### Updated GetCode Endpoint

The `/api/admin/codemanager/getcode` endpoint now requires:

- `trafficId`: The campaign ID
- `solution`: The solution number
- `hCaptchaToken`: The hCaptcha verification token

### Request Format

```json
{
  "trafficId": "campaign-guid",
  "solution": "1",
  "hCaptchaToken": "hcaptcha-verification-token"
}
```

### Response Format

```json
{
  "success": true,
  "html": "generated-code"
}
```

## Error Handling

The API will return the following errors:

- `"hCaptcha token is required"` - When no hCaptcha token is provided
- `"hCaptcha verification failed"` - When hCaptcha verification fails
- `"Invalid request"` - When the request format is invalid

## Security Notes

1. Always verify hCaptcha tokens on the server side
2. Never expose your Secret Key in client-side code
3. Consider implementing rate limiting for additional security
4. Monitor hCaptcha success rates to detect potential abuse

## Testing

For testing purposes, you can:

1. Set `"Enabled": false` in the HCaptcha configuration to bypass verification
2. Use hCaptcha's test keys for development
3. Monitor the verification logs in your application

## Troubleshooting

### Common Issues

1. **"hCaptcha verification failed"**
   - Check if your Secret Key is correct
   - Verify the token hasn't expired
   - Ensure the domain matches your hCaptcha configuration

2. **"hCaptcha token is required"**
   - Make sure the hCaptcha widget is properly loaded
   - Verify the user completed the challenge
   - Check if the token is being sent in the request

3. **Widget not loading**
   - Verify your Site Key is correct
   - Check if the hCaptcha script is loaded
   - Ensure your domain is whitelisted in hCaptcha settings
