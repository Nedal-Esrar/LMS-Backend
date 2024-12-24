namespace MLMS.Domain.Identity;

public static class EmailUtils
{
    public static string GetRegistrationEmailBody(string userName, string workId, string password, string loginLink) =>
        $"""
        <!DOCTYPE html>
        <html lang="en">
        
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            <link href="https://fonts.googleapis.com/css2?family=Sora:wght@100..800&display=swap" rel="stylesheet">
            <title>Makassed LMS</title>
        </head>
        
        <body style="margin:0; padding:0; font-family: Sora, sans-serif; background-color: #f4f4f4;">
            <table role="presentation" width="100%" cellspacing="0" cellpadding="0"
                style="background-color: #f4f4f4; padding: 20px;">
                <tr>
                    <td align="center">
                        <!-- Main Container -->
                    <td style="padding-top:0;padding-bottom:0;padding-right:10px;padding-left:10px">
                        <div class="m_-533078734955811462webkit"
                            style="max-width:600px;margin-top:0;margin-bottom:0;margin-right:auto;margin-left:auto">
                            <table role="presentation" class="m_-533078734955811462outer" align="center"
                                style="border-spacing:0;Margin:0 auto;width:100%;max-width:600px">
                                <tbody>
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
                                    <tr>
                                        <td class="m_-533078734955811462one-column" align="center"
                                            style="padding-left:10px;padding-right:10px">
                                            <table role="presentation" align="center" border="0" cellpadding="0" cellspacing="0"
                                                width="100%" style="border-radius:4px;background-color:#efebe4"
                                                bgcolor="#efebe4">
                                                <tbody>
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center"
                                                            style="padding-top:0px;padding-left:20px;padding-right:20px">
                                                            <h2 style="color:#027e7b;font-size28px;font-weight:bold;font-stretch:normal;font-style:normal;line-height:32px;letter-spacing:-0.5px">
                                                                Your account has been created successfully</h2>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
                                                    <tr>
                                                        <td width="0" height="22" style="font-size:1px;line-height:1px"
                                                            bgcolor="#ffffff"></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left"
                                                            style="padding-top:0px;padding-left:20px;padding-bottom:0px;padding-right:20px"
                                                            bgcolor="#ffffff">
                                                            <p style="color:#05192d;line-height:26px;font-weight:normal">
                                                                Hi {userName}, you can now login with the following credentials</p>
                                                            <p style="font-size: 14px;">
                                                                Work Id: {workId},
                                                            </p>
                                                            <p style="font-size: 14px;">
                                                                Password: {password}
                                                            </p>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"
                                                            bgcolor="#ffffff"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-collapse:collapse" bgcolor="#ffffff">
                                                            <table role="presentation" align="center" bgcolor="#02ee61"
                                                                style="border-collapse:collapse;margin:0 auto;padding:0;border-spacing:0;border-radius:4px;background-color:#027e7b;background-image:linear-gradient(#027e7b,#027e7b)"
                                                                cellpadding="0" cellspacing="0" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="border-collapse:collapse;font-family:StudioFeixenSans,'StudioFeixenSans',Arial,sans-serif;font-size:16px;font-weight:bold;padding:0;border:0;text-align:center;border-radius:4px;color:#05192d;background-color:#027e7b;background-image:linear-gradient(#027e7b,#027e7b)"
                                                                            align="center"><a
                                                                                style="text-decoration:none;color:#ffffff;font-family:StudioFeixenSans,'StudioFeixenSans',Arial,sans-serif;font-size:16px;font-weight:bold;text-align:center;border-radius:4px;font-size:16px;line-height:30px"
                                                                                href="${loginLink}" target="_blank">
                                                                                <span
                                                                                    style="padding:9px 19px;border-radius:4px;display:block">
                                                                                    Visit website
                                                                                </span>
                                                                            </a></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="0" height="42" style="font-size:1px;line-height:1px"
                                                            bgcolor="#ffffff"></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                    </td>
                </tr>
            </table>
        </body>
        
        </html>
        """;
    
    public static string GetResetPasswordEmailBody(string userName, string resetPasswordLink) =>
        $"""
        <!DOCTYPE html>
        <html lang="en">
        
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            <link href="https://fonts.googleapis.com/css2?family=Sora:wght@100..800&display=swap" rel="stylesheet">
            <title>Makassed LMS</title>
        </head>
        
        <body style="margin:0; padding:0; font-family: Sora, sans-serif; background-color: #f4f4f4;">
            <table role="presentation" width="100%" cellspacing="0" cellpadding="0"
                style="background-color: #f4f4f4; padding: 20px;">
                <tr>
                    <td align="center">
                        <!-- Main Container -->
                    <td style="padding-top:0;padding-bottom:0;padding-right:10px;padding-left:10px">
                        <div class="m_-533078734955811462webkit"
                            style="max-width:600px;margin-top:0;margin-bottom:0;margin-right:auto;margin-left:auto">
        
                            <table role="presentation" class="m_-533078734955811462outer" align="center"
                                style="border-spacing:0;Margin:0 auto;width:100%;max-width:600px">
                                <tbody>
        
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
        
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
        
                                    <tr>
                                        <td class="m_-533078734955811462one-column" align="center"
                                            style="padding-left:10px;padding-right:10px">
        
        
                                            <table role="presentation" align="center" border="0" cellpadding="0" cellspacing="0"
                                                width="100%" style="border-radius:4px;background-color:#efebe4"
                                                bgcolor="#efebe4">
                                                <tbody>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td align="center"
                                                            style="padding-top:0px;padding-left:20px;padding-right:20px">
                                                            <h2
                                                                style="color:#027e7b;font-size:32px;font-weight:bold;font-stretch:normal;font-style:normal;line-height:36px;letter-spacing:-0.5px">
                                                                Request to Reset Your Password</h2>
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
        
        
                                                    <tr>
                                                        <td width="0" height="22" style="font-size:1px;line-height:1px"
                                                            bgcolor="#ffffff"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td align="left"
                                                            style="padding-top:0px;padding-left:20px;padding-bottom:0px;padding-right:20px"
                                                            bgcolor="#ffffff">
                                                            <p
                                                                style="color:#05192d;font-family:StudioFeixenSans,'StudioFeixenSans',Arial,sans-serif;font-size:16px;line-height:26px;font-weight:normal">
                                                                Hi {userName}, you can now reset your password by clicking the button below. 
                                                            </p>
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"
                                                            bgcolor="#ffffff"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td style="border-collapse:collapse" bgcolor="#ffffff">
        
                                                            <table role="presentation" align="center" bgcolor="#02ee61"
                                                                style="border-collapse:collapse;margin:0 auto;padding:0;border-spacing:0;border-radius:4px;background-color:#027e7b;background-image:linear-gradient(#027e7b,#027e7b)"
                                                                cellpadding="0" cellspacing="0" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="border-collapse:collapse;font-family:StudioFeixenSans,'StudioFeixenSans',Arial,sans-serif;font-size:16px;font-weight:bold;padding:0;border:0;text-align:center;border-radius:4px;color:#05192d;background-color:#027e7b;background-image:linear-gradient(#027e7b,#027e7b)"
                                                                            align="center"><a
                                                                                style="text-decoration:none;color:#ffffff;font-family:StudioFeixenSans,'StudioFeixenSans',Arial,sans-serif;font-size:16px;font-weight:bold;text-align:center;border-radius:4px;font-size:16px;line-height:30px"
                                                                                href="{resetPasswordLink}" target="_blank">
                                                                                <span
                                                                                    style="padding:9px 19px;border-radius:4px;display:block">
                                                                                    Reset Password
                                                                                </span>
                                                                            </a></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
        
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="42" style="font-size:1px;line-height:1px"
                                                            bgcolor="#ffffff"></td>
                                                    </tr>
        
        
        
                                                </tbody>
                                            </table>
        
                                        </td>
                                    </tr>
        
        
                                </tbody>
                            </table>
        
                        </div>
                    </td>
                    </td>
                </tr>
            </table>
        </body>
        
        </html>
        """;

    public static string GetPasswordChangedEmailBody(string userName) =>
        $"""
        <!DOCTYPE html>
        <html lang="en"><head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="">
            <link href="https://fonts.googleapis.com/css2?family=Sora:wght@100..800&amp;display=swap" rel="stylesheet">
            <title>Makassed LMS</title>
        </head>
        
        <body style="margin:0; padding:0; font-family: Sora, sans-serif; background-color: #f4f4f4;">
            <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="background-color: #f4f4f4; padding: 20px;">
                <tbody><tr>
                    <td align="center">
                        <!-- Main Container -->
                    </td><td style="padding-top:0;padding-bottom:0;padding-right:10px;padding-left:10px">
                        <div class="m_-533078734955811462webkit" style="max-width:600px;margin-top:0;margin-bottom:0;margin-right:auto;margin-left:auto">
        
                            <table role="presentation" class="m_-533078734955811462outer" align="center" style="border-spacing:0;Margin:0 auto;width:100%;max-width:600px">
                                <tbody>
        
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
        
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
        
                                    <tr>
                                        <td class="m_-533078734955811462one-column" align="center" style="padding-left:10px;padding-right:10px">
        
        
                                            <table role="presentation" align="center" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-radius:4px;background-color:#efebe4" bgcolor="#efebe4">
                                                <tbody>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td align="center" style="padding-top:0px;padding-left:20px;padding-right:20px">
                                                            <h2 style="color:#027e7b;font-size:28px;font-weight:bold;font-stretch:normal;font-style:normal;line-height:36px;letter-spacing:-0.5px">Your Password Has Been Changed Successfully!</h2>
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
        
        
                                                    <tr>
                                                        <td width="0" height="22" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td align="left" style="padding-top:0px;padding-left:20px;padding-bottom:0px;padding-right:20px" bgcolor="#ffffff">
                                                            <p style="color:#05192d;font-family:StudioFeixenSans,'StudioFeixenSans',Arial,sans-serif;font-size:16px;line-height:26px;font-weight:normal">
                                                                Hi {userName}, your Makassed LMS password has been changed  successfully, if you weren't the one who changed it, please contact the administrators.
                                                            </p>
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                    </tr>
        
        
        
                                                </tbody>
                                            </table>
        
                                        </td>
                                    </tr>
        
        
                                </tbody>
                            </table>
        
                        </div>
                    </td>
                    
                </tr>
            </tbody></table>
        
        
        </body></html>
        """;
}