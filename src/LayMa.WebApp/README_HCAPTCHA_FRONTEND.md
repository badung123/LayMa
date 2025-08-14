# hCaptcha Frontend Integration for Traffic Page

This document explains how the hCaptcha verification has been integrated into the Traffic page.

## Overview

The Traffic page now includes hCaptcha verification before allowing users to generate codes. When a user clicks the "LẤY MÃ" (Get Code) button, a modal appears with an hCaptcha widget that must be completed before the code generation process can continue.

## Implementation Details

### 1. HTML Structure

The page now includes:
- Proper HTML5 structure with `<html>`, `<head>`, and `<body>` tags
- hCaptcha script loaded from CDN: `https://js.hcaptcha.com/1/api.js`

### 2. Configuration

The hCaptcha site key is configured in the appsettings files:

```json
{
  "HCaptcha": {
    "SiteKey": "your-hcaptcha-site-key"
  }
}
```

The site key is passed from the controller to the view via ViewBag and then to JavaScript.

### 3. Modal Implementation

When the user clicks the "LẤY MÃ" button:

1. **Incognito Detection**: First checks if the user is in incognito/private mode
2. **Modal Display**: If not in private mode, shows an hCaptcha modal
3. **Verification**: User must complete the hCaptcha challenge
4. **Token Storage**: The hCaptcha token is stored in the element's dataset
5. **Timer Start**: The normal countdown timer begins
6. **API Call**: When the timer completes, the API call includes the hCaptcha token

### 4. Modal Features

The hCaptcha modal includes:
- **Overlay**: Semi-transparent background covering the entire page
- **Content Box**: White modal with rounded corners and shadow
- **Title**: "Xác thực bảo mật" (Security Verification)
- **hCaptcha Widget**: Centered hCaptcha challenge
- **Verify Button**: "Xác thực và lấy mã" (Verify and Get Code)
- **Close Button**: "Đóng" (Close) to cancel the process
- **Click Outside**: Modal can be closed by clicking outside the content area

### 5. API Integration

The modified API calls now include:
- `trafficId`: The campaign ID (renamed from `trafficid` for consistency)
- `solution`: The solution number
- `hCaptchaToken`: The hCaptcha verification token

### 6. Error Handling

- If hCaptcha is not completed, an alert shows "Vui lòng hoàn thành xác thực hCaptcha"
- If the API call fails due to hCaptcha verification, the error is handled gracefully
- The modal can be closed at any time to cancel the process

## User Flow

1. User visits the Traffic page
2. User clicks "LẤY MÃ" button
3. System checks for incognito mode
4. If not incognito, hCaptcha modal appears
5. User completes hCaptcha challenge
6. User clicks "Xác thực và lấy mã"
7. Modal closes and countdown timer starts
8. When timer completes, API call is made with hCaptcha token
9. Code is generated and displayed to user

## Styling

The modal uses inline CSS for consistent styling:
- Fixed positioning with overlay
- Centered content with flexbox
- Responsive design (90% width, max 400px)
- Consistent color scheme with the existing design
- Smooth transitions and hover effects

## Configuration Steps

1. **Get hCaptcha Keys**: Sign up at [hCaptcha.com](https://www.hcaptcha.com/) and get your Site Key
2. **Update Configuration**: Replace `"your-hcaptcha-site-key"` in all appsettings files with your actual site key
3. **Test Integration**: Verify that the modal appears and hCaptcha verification works
4. **Monitor**: Check hCaptcha success rates and adjust settings as needed

## Security Benefits

- **Bot Protection**: Prevents automated bots from generating codes
- **Rate Limiting**: hCaptcha adds an additional layer of rate limiting
- **User Verification**: Ensures real human interaction before code generation
- **Token Validation**: Server-side verification of hCaptcha tokens

## Troubleshooting

### Common Issues

1. **Modal not appearing**: Check if hCaptcha script is loading properly
2. **Site key errors**: Verify the site key is correct in configuration
3. **API errors**: Check that the hCaptcha token is being sent correctly
4. **Styling issues**: Ensure the modal CSS is not being overridden

### Debug Steps

1. Check browser console for JavaScript errors
2. Verify hCaptcha script is loaded
3. Test hCaptcha widget independently
4. Check network tab for API call details
5. Verify configuration values are being passed correctly

## Future Enhancements

- Add loading states during hCaptcha verification
- Implement fallback verification methods
- Add analytics for hCaptcha success/failure rates
- Consider implementing hCaptcha v3 for invisible verification
- Add accessibility improvements for screen readers
